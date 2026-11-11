// VisioForge Virtual Camera SDK: push YUY2 frames to the virtual camera from C++, without a DirectShow graph.
// The frames go into a named shared memory ring that the "VisioForge Virtual Camera" source filter reads,
// so Zoom, OBS Studio and browsers show them as an ordinary webcam.
//
// Usage: PushDemo.exe [license key]     Ctrl+C stops.

#include <windows.h>
#include <sddl.h>
#pragma comment(lib, "advapi32.lib")
#include <dshow.h>
#include <timeapi.h>
#pragma comment(lib, "winmm.lib")
#include <atomic>
#include <cstdio>
#include <string>

// The shared memory layout the source filter reads: header, AM_MEDIA_TYPE + format block, then the slots.
struct SHARED_DATA
{
    INT      nStreamsCount;
    LONGLONG llStreamSize;
    WORD     wEaters;
    BOOL     bHaveServer;
    WORD     wFlags;
    BOOL     bMany;
    WORD     wMediaTypeSize;
};

// One slot: this header, followed by llStreamSize bytes of frame data.
struct STREAM_SHARED_DATA
{
    LONGLONG       llDataSize;
    INT            nLockMode;
    DWORD          dwTimeStamp;
    WORD           wFlags;
    WORD           wEaters;
    REFERENCE_TIME rtStart;
    REFERENCE_TIME rtStop;
};

// The reader computes every offset from these sizes: a different struct packing (/Zp) breaks the stream.
static_assert(sizeof(SHARED_DATA) == 40, "SHARED_DATA must match the filter's layout");
static_assert(sizeof(STREAM_SHARED_DATA) == 40, "STREAM_SHARED_DATA must match the filter's layout");

const WORD SAMPLE_SYNCPOINT = 0x0001;
const WORD STREAM_EOS = 0x0200;
const INT LOCK_NONE = 0x00;
const INT LOCK_FOR_WRITING = 0x02;
const int SLOT_COUNT = 3;
const int LICENSE_MAX_CCH = 64;

const int WIDTH = 1280;
const int HEIGHT = 720;
const int FPS = 30;
const LONGLONG FRAME_SIZE = WIDTH * HEIGHT * 2; // YUY2: 2 bytes per pixel

static std::atomic<bool> g_stop{ false };

// Closing the console window ends the process as soon as this handler returns, so it never returns:
// wmain stops, publishes the end of stream and ends the process itself.
static BOOL WINAPI OnConsoleCtrl(DWORD)
{
    g_stop = true;
    Sleep(INFINITE);
    return TRUE;
}

// Takes a mutex. A wait without a timeout gives up once Ctrl+C or closing the window asks to stop,
// so a consumer that hangs while holding the mutex cannot keep the demo from ending.
static bool Lock(HANDLE mutex, DWORD timeoutMs = INFINITE)
{
    for (;;)
    {
        const DWORD result = WaitForSingleObject(mutex, timeoutMs == INFINITE ? 100 : timeoutMs);
        if (result == WAIT_OBJECT_0 || result == WAIT_ABANDONED)
        {
            return true;
        }

        if (result == WAIT_FAILED || timeoutMs != INFINITE || g_stop)
        {
            return false;
        }
    }
}

// Open to every process of the session at medium integrity: a camera consumer that is not elevated can then open
// what an elevated producer creates.
static SECURITY_ATTRIBUTES* SharedSecurity()
{
    static SECURITY_ATTRIBUTES sa = { sizeof(sa) };
    if (!sa.lpSecurityDescriptor)
        ConvertStringSecurityDescriptorToSecurityDescriptorW(L"D:(A;;GA;;;WD)S:(ML;;NW;;;ME)", SDDL_REVISION_1, &sa.lpSecurityDescriptor, nullptr);
    return sa.lpSecurityDescriptor ? &sa : nullptr;
}

struct Producer
{
    HANDLE producerMutex = nullptr;
    HANDLE mutex = nullptr;
    HANDLE map = nullptr;
    HANDLE licenseMap = nullptr;
    HANDLE slotMutex[SLOT_COUNT] = {};
    BYTE* base = nullptr;
    size_t streamsOffset = 0;
    DWORD lastStamp = 0;
    int misses = 0;
    bool opened = false;

    SHARED_DATA* Header() { return (SHARED_DATA*)base; }
    STREAM_SHARED_DATA* Slot(int i) { return (STREAM_SHARED_DATA*)(base + streamsOffset + (sizeof(STREAM_SHARED_DATA) + FRAME_SIZE) * i); }
    BYTE* SlotBuffer(int i) { return (BYTE*)Slot(i) + sizeof(STREAM_SHARED_DATA); }

    bool Open(const std::wstring& name, const wchar_t* license)
    {
        VIDEOINFOHEADER vih;
        ZeroMemory(&vih, sizeof(vih));
        vih.AvgTimePerFrame = 10000000 / FPS;
        vih.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
        vih.bmiHeader.biWidth = WIDTH;
        vih.bmiHeader.biHeight = HEIGHT;
        vih.bmiHeader.biPlanes = 1;
        vih.bmiHeader.biBitCount = 16;
        vih.bmiHeader.biCompression = MAKEFOURCC('Y', 'U', 'Y', '2');
        vih.bmiHeader.biSizeImage = (DWORD)FRAME_SIZE;

        // pUnk and pbFormat stay NULL: the reader takes the format from the bytes that follow the struct.
        // Zeroed, padding included: a later run compares these bytes to take the stream over.
        AM_MEDIA_TYPE mt;
        ZeroMemory(&mt, sizeof(mt));
        mt.majortype = MEDIATYPE_Video;
        mt.subtype = MEDIASUBTYPE_YUY2;
        mt.bFixedSizeSamples = TRUE;
        mt.lSampleSize = (ULONG)FRAME_SIZE;
        mt.formattype = FORMAT_VideoInfo;
        mt.cbFormat = sizeof(vih);

        streamsOffset = sizeof(SHARED_DATA) + sizeof(mt) + sizeof(vih);
        const ULONGLONG total = streamsOffset + (sizeof(STREAM_SHARED_DATA) + FRAME_SIZE) * SLOT_COUNT;

        // Owned for as long as this demo streams: a second PushDemo sees it taken and leaves the stream alone.
        producerMutex = CreateMutexW(SharedSecurity(), TRUE, (name + L"Producer").c_str());
        if (producerMutex && GetLastError() == ERROR_ALREADY_EXISTS && !Lock(producerMutex, 0))
        {
            wprintf(L"Another PushDemo is streaming to %s.\n", name.c_str());
            return false;
        }

        mutex = CreateMutexW(SharedSecurity(), FALSE, (name + L"Mutex").c_str());
        if (!mutex)
        {
            wprintf(L"CreateMutex failed: %lu\n", GetLastError());
            return false;
        }

        map = CreateFileMappingW(INVALID_HANDLE_VALUE, SharedSecurity(), PAGE_READWRITE, (DWORD)(total >> 32), (DWORD)total, name.c_str());
        const DWORD mapError = GetLastError();
        if (!map)
        {
            wprintf(L"CreateFileMapping failed: %lu\n", mapError);
            return false;
        }

        base = (BYTE*)MapViewOfFile(map, FILE_MAP_WRITE, 0, 0, 0);
        if (!base)
        {
            wprintf(L"MapViewOfFile failed: %lu\n", GetLastError());
            return false;
        }

        // A mapping left by an earlier producer (still held open by a consumer) is taken over when that producer
        // used the same format (the consumer keeps the format it read when it connected) and has stopped: it said so,
        // or it was killed and has not published a frame for 2 s (another PushDemo is ruled out by producerMutex above).
        const bool existed = mapError == ERROR_ALREADY_EXISTS;
        const WORD mediaTypeSize = (WORD)(sizeof(mt) + sizeof(vih));
        BYTE format[sizeof(mt) + sizeof(vih)];
        CopyMemory(format, &mt, sizeof(mt));
        CopyMemory(format + sizeof(mt), &vih, sizeof(vih));
        if (!Lock(mutex))
        {
            return false;
        }

        SHARED_DATA* header = Header();
        if (existed && (header->nStreamsCount != SLOT_COUNT || header->llStreamSize != FRAME_SIZE || header->wMediaTypeSize != mediaTypeSize
            || memcmp(base + sizeof(SHARED_DATA), format, sizeof(format)) != 0
            || (header->bHaveServer && MsSinceLastFrame() < 2000)))
        {
            ReleaseMutex(mutex);
            wprintf(L"Stream %s is in use by another producer or another frame size. Close the applications using the camera and retry.\n", name.c_str());
            return false;
        }

        // The key goes into its own mapping "<name>License"; it stays alive while the handle is open.
        if (license && *license)
        {
            licenseMap = CreateFileMappingW(INVALID_HANDLE_VALUE, SharedSecurity(), PAGE_READWRITE, 0, LICENSE_MAX_CCH * sizeof(WCHAR), (name + L"License").c_str());
            WCHAR* view = licenseMap ? (WCHAR*)MapViewOfFile(licenseMap, FILE_MAP_WRITE, 0, 0, 0) : nullptr;
            if (view)
            {
                ZeroMemory(view, LICENSE_MAX_CCH * sizeof(WCHAR));
                if (wcsncpy_s(view, LICENSE_MAX_CCH, license, _TRUNCATE) == STRUNCATE)
                {
                    wprintf(L"The license key is longer than %d characters; the TRIAL banner stays on.\n", LICENSE_MAX_CCH - 1);
                }

                UnmapViewOfFile(view);
            }
            else
            {
                wprintf(L"Cannot publish the license key (error %lu); the TRIAL banner stays on.\n", GetLastError());
            }
        }

        header->nStreamsCount = SLOT_COUNT;
        header->llStreamSize = FRAME_SIZE;
        header->bHaveServer = TRUE;
        header->wFlags = STREAM_EOS;
        header->bMany = TRUE; // several consumers may read the camera at once
        header->wMediaTypeSize = mediaTypeSize;
        CopyMemory(base + sizeof(SHARED_DATA), format, sizeof(format));
        ReleaseMutex(mutex);
        opened = true; // from here on the destructor publishes the end of stream, whatever fails next

        for (int i = 0; i < SLOT_COUNT; i++)
        {
            slotMutex[i] = CreateMutexW(SharedSecurity(), FALSE, (name + L"MutexStream" + std::to_wstring(i)).c_str());
            if (!slotMutex[i])
            {
                wprintf(L"CreateMutex failed: %lu\n", GetLastError());
                return false;
            }

            if (Lock(slotMutex[i]))
            {
                // A slot the earlier producer left half written would never be used again.
                STREAM_SHARED_DATA* s = Slot(i);
                if (s->nLockMode == LOCK_FOR_WRITING)
                {
                    s->nLockMode = LOCK_NONE;
                    s->llDataSize = 0;
                }

                s->wEaters = 1;
                ReleaseMutex(slotMutex[i]);
            }
        }

        return true;
    }

    DWORD MsSinceLastFrame()
    {
        DWORD youngest = MAXDWORD;
        const DWORD now = GetTickCount();
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            const DWORD age = now - Slot(i)->dwTimeStamp;
            youngest = age < youngest ? age : youngest;
        }

        return youngest;
    }

    void SetFlags(WORD flags, BOOL haveServer)
    {
        if (opened && Lock(mutex, 1000))
        {
            Header()->wFlags = flags;
            Header()->bHaveServer = haveServer;
            ReleaseMutex(mutex);
        }
    }

    // Takes the oldest empty slot, or the oldest unread one when the consumer is behind, and marks it as being written.
    int LockSlot()
    {
        int slot = -1;
        if (!Lock(mutex))
        {
            return -1;
        }

        Header()->wFlags = 0;

        int empty = -1, unread = -1, locked = -1;
        DWORD emptyTime = 0, unreadTime = 0, lockedTime = 0;
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (!Lock(slotMutex[i], 0))
            {
                continue;
            }

            STREAM_SHARED_DATA* s = Slot(i);
            if (s->nLockMode == LOCK_NONE)
            {
                if (s->llDataSize == 0 && s->wEaters == 0)
                {
                    if (empty < 0 || s->dwTimeStamp < emptyTime)
                    {
                        empty = i;
                        emptyTime = s->dwTimeStamp;
                    }
                }
                else if (unread < 0 || s->dwTimeStamp < unreadTime)
                {
                    unread = i;
                    unreadTime = s->dwTimeStamp;
                }
            }
            else if (locked < 0 || s->dwTimeStamp < lockedTime)
            {
                locked = i;
                lockedTime = s->dwTimeStamp;
            }

            ReleaseMutex(slotMutex[i]);
        }

        // No free slot for a whole second: a consumer was killed while reading one, so the oldest locked slot is taken back.
        int candidate = empty >= 0 ? empty : unread;
        if (candidate < 0 && ++misses >= FPS)
        {
            candidate = locked;
        }

        if (candidate >= 0 && Lock(slotMutex[candidate], 0))
        {
            STREAM_SHARED_DATA* s = Slot(candidate);
            s->wEaters = max((WORD)1, Header()->wEaters);
            s->nLockMode = LOCK_FOR_WRITING;
            ReleaseMutex(slotMutex[candidate]);
            slot = candidate;
            misses = 0;
        }

        ReleaseMutex(mutex);
        return slot;
    }

    // Publishes a slot filled with LockSlot: size, time and LOCK_NONE make it readable.
    void Commit(int slot, REFERENCE_TIME start, REFERENCE_TIME stop)
    {
        if (!Lock(slotMutex[slot]))
        {
            return;
        }

        // The reader takes only a slot stamped later than the last one it read: two frames in one tick need distinct
        // stamps. The tick count wraps after 49.7 days, so stamps are compared by their difference.
        const DWORD now = GetTickCount();
        lastStamp = lastStamp == 0 || (LONG)(now - lastStamp) > 0 ? now : lastStamp + 1;

        STREAM_SHARED_DATA* s = Slot(slot);
        s->llDataSize = FRAME_SIZE;
        s->rtStart = start;
        s->rtStop = stop;
        s->wFlags = SAMPLE_SYNCPOINT;
        s->nLockMode = LOCK_NONE;
        s->dwTimeStamp = lastStamp;
        ReleaseMutex(slotMutex[slot]);
    }

    ~Producer()
    {
        SetFlags(STREAM_EOS, FALSE);
        if (base)
        {
            UnmapViewOfFile(base);
        }

        for (HANDLE h : slotMutex)
        {
            if (h)
            {
                CloseHandle(h);
            }
        }

        if (producerMutex)
        {
            ReleaseMutex(producerMutex);
        }

        for (HANDLE h : { map, licenseMap, mutex, producerMutex })
        {
            if (h)
            {
                CloseHandle(h);
            }
        }
    }
};

int wmain(int argc, wchar_t** argv)
{
    // A 64-bit consumer (Zoom, OBS, browsers) reads the "X64" stream, a 32-bit one "X86": build the bitness it uses.
    const std::wstring name = std::wstring(sizeof(void*) == 8 ? L"X64" : L"X86") + L"VideoStream1";

    SetConsoleCtrlHandler(OnConsoleCtrl, TRUE);

    Producer producer;
    if (!producer.Open(name, argc > 1 ? argv[1] : nullptr))
    {
        return 1;
    }

    // Eight colour bars (BT.601 Y, U, V): one YUY2 row is built, then copied to every row of each frame.
    const BYTE bars[8][3] = {
        { 235, 128, 128 }, { 210, 16, 146 }, { 170, 166, 16 }, { 145, 54, 34 },
        { 106, 202, 222 }, { 81, 90, 240 }, { 41, 240, 110 }, { 16, 128, 128 } };
    BYTE barRow[WIDTH * 2];
    BYTE whiteRow[WIDTH * 2];
    for (int x = 0; x < WIDTH / 2; x++)
    {
        const BYTE* c = bars[x * 16 / WIDTH];
        BYTE* p = barRow + x * 4;
        p[0] = c[0]; p[1] = c[1]; p[2] = c[0]; p[3] = c[2];

        BYTE* w = whiteRow + x * 4;
        w[0] = 235; w[1] = 128; w[2] = 235; w[3] = 128;
    }

    wprintf(L"Streaming %dx%d YUY2 at %d fps to VisioForge Virtual Camera. Ctrl+C stops.\n", WIDTH, HEIGHT, FPS);

    // 1 ms timer resolution, so a frame goes out every 33 ms rather than at 16/47 ms steps.
    timeBeginPeriod(1);

    const REFERENCE_TIME frameDuration = 10000000 / FPS;
    DWORD start = timeGetTime();
    LONGLONG frame = 0;
    LONGLONG dropped = 0;
    while (!g_stop)
    {
        const int slot = producer.LockSlot();
        if (slot >= 0)
        {
            BYTE* dst = producer.SlotBuffer(slot);
            for (int y = 0; y < HEIGHT; y++)
            {
                memcpy(dst + y * WIDTH * 2, barRow, sizeof(barRow));
            }

            // A white band moving down shows the picture is live.
            const int bandTop = (int)(frame * 8 % (HEIGHT - 40));
            for (int y = bandTop; y < bandTop + 40; y++)
            {
                memcpy(dst + y * WIDTH * 2, whiteRow, sizeof(whiteRow));
            }

            producer.Commit(slot, frame * frameDuration, (frame + 1) * frameDuration);
        }
        else if (!g_stop)
        {
            dropped++;
        }

        frame++;
        const DWORD due = (DWORD)(frame * 1000 / FPS);
        const DWORD elapsed = timeGetTime() - start;
        if ((LONG)(due - elapsed) > 0)
        {
            Sleep(due - elapsed);
        }
        else if ((LONG)(elapsed - due) > 1000 / FPS)
        {
            start = timeGetTime() - due; // after a stall (PC sleep, debugger) continue from now rather than catch up
        }
    }

    timeEndPeriod(1);

    wprintf(L"Stopped: %lld frames sent, %lld dropped.\n", frame - dropped, dropped);
    return 0;
}

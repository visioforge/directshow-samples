#pragma once

#pragma warning(disable : 4995)

#include <objbase.h>
#include <initguid.h>

// {299332CF-1791-4301-B043-F06FAF847C52}
static const GUID IID_IConfigureVideoEncoder =
{ 0x299332cf, 0x1791, 0x4301,{ 0xb0, 0x43, 0xf0, 0x6f, 0xaf, 0x84, 0x7c, 0x52 } }; 

interface IConfigureVideoEncoder : public IUnknown
{
    struct Params
    {
        enum Profile // profile values
        {
            PF_AUTOSELECT           = 0,

            // H.264 values
            PF_H264_BASELINE             = 66,
            PF_H264_MAIN                 = 77,
            PF_H264_HIGH                 = 100,
            PF_H264_HIGH10               = 110,
            PF_H264_HIGH422              = 122,

            // MPEG2 values
            PF_MPEG2_SIMPLE              = 80,
            PF_MPEG2_MAIN                = 64,
            PF_MPEG2_SNR                 = 3,
            PF_MPEG2_SPATIAL             = 2,
            PF_MPEG2_HIGH                = 16
        } profile_idc;

        enum Level // level values
        {
            LL_AUTOSELECT           = 0,

            // H.264 values
            LL_1                    = 10,
            LL_1b                   = 9,
            LL_11                   = 11,
            LL_12                   = 12,
            LL_13                   = 13,
            LL_2                    = 20,
            LL_21                   = 21,
            LL_22                   = 22,
            LL_3                    = 30,
            LL_31                   = 31,
            LL_32                   = 32,
            LL_4                    = 40,
            LL_41                   = 41,
            LL_42                   = 42,
            LL_5                    = 50,
            LL_51                   = 51,

            // MPEG2 values
            LL_LOW          = 10,
            LL_MAIN         = 8,
            LL_HIGH1440     = 6,
            LL_HIGH         = 4
        } level_idc;

        struct PSControl // picture sequence control
        {
            DWORD        GopPicSize;           // I-frame interval in frames
            DWORD        GopRefDist;           // Distance between I- or P- key frames;If GopRefDist = 1, there are no B-frames used
            DWORD        NumSlice;             // Number of slices
            DWORD        BufferSizeInKB;       // vbv buffer size
        } ps_control;

        enum PCControl // picture coding control
        {
            PC_FRAME        = 1,
            PC_FIELD_TFF    = 2,
            PC_FIELD_BFF    = 4,
        } pc_control;

        struct FrameControl
        {
            DWORD        width;         // output frame width
            DWORD        height;        // output frame height
        } frame_control;
        struct ThrottleControl
        {
            // See Requirement 34
            enum ThrottlePolicy
            {
                TT_NA,              // no throttle handling
                TT_AUTO,            // auto throttling
            } throttle_policy;

            DWORD bitrate_up;       // range of bitrate increase adjustment.
            DWORD bitrate_down;     // range of bitrate decrease adjustment.
            DWORD qp_up;            // range of qp increase adjustment
            DWORD qp_down;          // range of qp decrease adjustment
        } throttle_control;

        struct RCControl // rate control
        {
            enum RCMethod
            {
                RC_CBR  = 1,                      // Constant Bitrate
                RC_VBR  = 2,                      // Variable Bitrate
            } rc_method;

            DWORD       bitrate;               // specify bit rate in bps
        } rc_control;

        DWORD  preset;
        DWORD  target_usage;

        Params() { memset(this, 0, sizeof(*this)); }
    };

    struct Statistics
    {
        DWORD struct_size;          // size of the Statistics structure
        DWORD width;                // frame width
        DWORD height;               // frame height
        DWORD frame_rate;           // frame rate
        struct
        {
            DWORD horizontal;       // horizontal pixel aspect ratio
            DWORD vertical;         // vertical pixel aspect ratio
        } aspect_ratio;             // aspect ratio
        DWORD real_bitrate;         // average bitrate
        DWORD frames_encoded;       // number of frames encoded
        DWORD requested_bitrate;    // requested bit rate
        DWORD frames_received;      // number of frames received
    };

    // Set parameters to the video encoder. If successful, S_OK is returned.
    // Each codec specific interface must implement this function.
    STDMETHOD(SetParams)(Params *params) { UNREFERENCED_PARAMETER(params); return E_NOTIMPL; }

    // Obtain the parameters currently used by the Encoder.
    // Each codec specific interface must implement this function.
    STDMETHOD(GetParams)(Params *params) { UNREFERENCED_PARAMETER(params); return E_NOTIMPL; }

    // Obtain Encoding Status
    STDMETHOD(GetRunTimeStatistics) (Statistics *statistics) PURE;
}; 
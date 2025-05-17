//---------------------------------------------------------------------------

#ifndef Unit1H
#define Unit1H
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include <Vcl.ExtCtrls.hpp>

#include <strmif.h>
#include <control.h>
#include <Vcl.Dialogs.hpp>

//---------------------------------------------------------------------------
class TForm1 : public TForm
{
__published:	// IDE-managed Components
	TLabel *Label1;
	TEdit *edFilename;
	TButton *btOpenFile;
	TLabel *Label2;
	TLabel *Label3;
	TEdit *edConnectionTimeOut;
	TComboBox *cbBufferingMode;
	TCheckBox *cbUseGPU;
	TScrollBar *tbTimeline;
	TButton *btStart;
	TButton *btPause;
	TButton *btStop;
	TButton *btResume;
	TLabel *lbTime;
	TLabel *Label5;
	TScrollBar *tbSpeed;
	TLabel *Label4;
	TLabel *Label6;
	TComboBox *cbVideoStream;
	TComboBox *cbAudioStream;
	TPanel *pnScreen;
	TTimer *tmProgress;
	TFileOpenDialog *FileOpenDialog1;
	void __fastcall btStartClick(TObject *Sender);
	void __fastcall btResumeClick(TObject *Sender);
	void __fastcall btPauseClick(TObject *Sender);
	void __fastcall tmProgressTimer(TObject *Sender);
	void __fastcall btStopClick(TObject *Sender);
	void __fastcall tbSpeedChange(TObject *Sender);
	void __fastcall tbTimelineChange(TObject *Sender);
	void __fastcall cbVideoStreamChange(TObject *Sender);
	void __fastcall cbAudioStreamChange(TObject *Sender);
	void __fastcall btOpenFileClick(TObject *Sender);
private:	// User declarations
   bool _seekingFlag;

   int WM_GRAPHNOTIFY = 0x8000 + 1;

   IFilterGraph2* filterGraph;

   ICaptureGraphBuilder2* captureGraph;

   IMediaControl* mediaControl;

   IMediaSeeking* mediaSeeking;

   IVideoWindow* videoWindow;

   IMediaEventEx* mediaEventEx;

   IBaseFilter* sourceFilter;

   IBaseFilter* videoRenderer;

   void AddGraph();
   void AddSource();
   void GetStreams();
   void AddVideoRenderer();
   REFERENCE_TIME GetDuration();
   REFERENCE_TIME GetPosition();
   bool PositionSet(long long milliseconds);
public:		// User declarations
	__fastcall TForm1(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TForm1 *Form1;
//---------------------------------------------------------------------------
#endif

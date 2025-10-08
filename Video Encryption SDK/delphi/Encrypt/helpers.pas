unit helpers;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, StrUtils, ActiveX, ENCDirectShow9, encryptor_intf,
  Vcl.ComCtrls, Vcl.ExtCtrls;

type
  TVFPinList = class(TInterfaceList)
  private
    Filter: IBaseFilter;
    function GetConnected(Index: Integer): Boolean;
    function GetPin(Index: Integer): IPin;
    function GetPinInfo(Index: Integer): TPinInfo;
    procedure PutPin(Index: Integer; item: IPin);
  public
    constructor Create(BaseFilter: IBaseFilter); overload;
    destructor Destroy; override;
    function Add(item: IPin): Integer;
    procedure Assign(BaseFilter: IBaseFilter);
    function First: IPin;
    function IndexOf(item: IPin): Integer;
    procedure Insert(Index: Integer; item: IPin);
    function Last: IPin;
    function Remove(item: IPin): Integer;
    procedure Update;
    property Connected[Index: Integer]: Boolean read GetConnected;
    property Items[Index: Integer]: IPin read GetPin write PutPin; default;
    property PinInfo[Index: Integer]: TPinInfo read GetPinInfo;
  end;

function GetPin(pFilter: IBaseFilter; pDir: PIN_DIRECTION; pIndex: byte;
  var pPin: IPin): Boolean;
function PinHaveThisType(Pin: IPin; MediaType: TGUID): Boolean;

implementation

constructor TVFPinList.Create(BaseFilter: IBaseFilter);
begin
  inherited Create;
  Filter := BaseFilter;
  Update;
end;

destructor TVFPinList.Destroy;
begin
  Filter := nil;
  inherited Destroy;
end;

function TVFPinList.Add(item: IPin): Integer;
begin
  result := inherited Add(item);
end;

procedure TVFPinList.Assign(BaseFilter: IBaseFilter);
begin
  Clear;
  Filter := BaseFilter;
  if Filter <> nil then
    Update;
end;

function TVFPinList.First: IPin;
begin
  result := GetPin(0);
end;

function TVFPinList.GetConnected(Index: Integer): Boolean;
var
  Pin: IPin;
begin
  Items[Index].ConnectedTo(Pin);
  result := (Pin <> nil);
end;

function TVFPinList.GetPin(Index: Integer): IPin;
begin
  result := Get(index) as IPin;
end;

function TVFPinList.GetPinInfo(Index: Integer): TPinInfo;
begin
  if Assigned(Items[index]) then
    Items[index].QueryPinInfo(result);
end;

function TVFPinList.IndexOf(item: IPin): Integer;
begin
  result := inherited IndexOf(item);
end;

procedure TVFPinList.Insert(Index: Integer; item: IPin);
begin
  inherited Insert(index, item);
end;

function TVFPinList.Last: IPin;
begin
  result := inherited Last as IPin;
end;

procedure TVFPinList.PutPin(Index: Integer; item: IPin);
begin
  Put(index, item);
end;

function TVFPinList.Remove(item: IPin): Integer;
begin
  result := inherited Remove(item);
end;

procedure TVFPinList.Update;
var
  EnumPins: IEnumPins;
  Pin: IPin;
begin
  Clear;
  if Assigned(Filter) then
    Filter.EnumPins(EnumPins)
  else
    Exit;
  while (EnumPins.Next(1, Pin, nil) = S_OK) do
    Add(Pin);
  EnumPins := nil;
end;

function GetPin(pFilter: IBaseFilter; pDir: PIN_DIRECTION; pIndex: byte;
  var pPin: IPin): Boolean;
var
  pl: TVFPinList;
  i, k: Integer;
begin
  result := false;

  try
    pl := TVFPinList.Create;
    pl.Assign(pFilter);

    k := 0;
    for i := 0 to pl.Count - 1 do
      if pl.PinInfo[i].dir = pDir then
      begin
        k := k + 1;
        if k = pIndex then
        begin
          pPin := pl.Items[i];
          pl.Free;
          result := true;
          Exit;
        end;
      end;

    pl.Free;
  except
    ;
  end;
end;

function PinHaveThisType(Pin: IPin; MediaType: TGUID): Boolean;
var
  mts: IEnumMediaTypes;
  MT: PAMMEDIATYPE;
begin
  try
    result := false;
    Pin.EnumMediaTypes(mts);
    mts.Reset;
    while mts.Next(1, MT, nil) = S_OK do
    begin
      if MT <> nil then
      begin
        if (IsEqualGUID(MT.majortype, MediaType)) then
        begin
          result := true;
          Exit;
        end;
      end;
    end;
    if Assigned(mts) then
      mts := nil;
  except
    result := false;
  end;
end;

end.

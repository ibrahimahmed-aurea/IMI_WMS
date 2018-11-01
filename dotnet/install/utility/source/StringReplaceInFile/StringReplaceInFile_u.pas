unit StringReplaceInFile_u;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs;

type
  TStringReplaceInForm = class(TForm)
    procedure FormShow(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  StringReplaceInForm: TStringReplaceInForm;

implementation

{$R *.dfm}

procedure TStringReplaceInForm.FormShow(Sender: TObject);
var
  f:         TextFile;
  line:      string;
  lines:     TStringList;
  i:         integer;
  go:        boolean;
  fileName:  string;
  oldWords:  TStringList;
  newWords:  TStringList;
  nu:        string;
begin

  go := false;

  if(ParamCount >= 3) then
  begin
    fileName := ParamStr(1);
    if ParamCount mod 2 = 1 then
      if FileExists(fileName) then
        go := true;
  end;

  if not go then
  begin
    Application.Terminate;
    abort;
  end;

  lines := TStringList.Create();
  try
    try
      AssignFile(f, fileName);
      try
        Reset(f);
        while not Eof(f) do
        begin
          Readln(f, line);
          lines.Add(line);
        end;
      except
      end;
    finally
      CloseFile(f);
    end;

    try
      if(lines.Count > 0) then
      begin

        oldWords := TStringList.Create();
        newWords := TStringList.Create();

        try
          for i := 1 to ParamCount - 1 do
          begin
            if i mod 2 = 1 then
              oldWords.Add(ParamStr(i+1))
            else
            begin
              if(ParamStr(i+1) = '/d') then
                nu := ''
              else
                nu := ParamStr(i+1);
              newWords.Add(nu);
            end;
          end;

          for i := 0 to oldWords.Count - 1 do
          begin
            lines.Text := StringReplace(lines.Text,oldWords[i], newWords[i],[rfReplaceAll]);
          end;
        finally
          if Assigned(oldWords) then
          begin
            if oldWords.count > 0 then
              oldWords.Clear;
            oldWords.Free;
          end;
          if Assigned(newWords) then
          begin
           if newWords.count > 0 then
            newWords.Clear;
           newWords.Free;
          end;
        end;

        AssignFile(f, fileName);
        try
          Rewrite(f);
          for i := 0 to lines.Count - 1 do
          begin
            Writeln(f, lines[i]);
          end;
        except
        end;
      end;
    finally
      CloseFile(f);
    end;
  finally
    if Assigned(lines) then
    begin
      if lines.count > 0 then
        lines.Clear;
      lines.Free;
    end;

    Application.Terminate;
    
  end;

end;

end.

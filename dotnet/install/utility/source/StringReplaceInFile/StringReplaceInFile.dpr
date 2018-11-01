program StringReplaceInFile;

uses
  Forms,
  StringReplaceInFile_u in 'StringReplaceInFile_u.pas' {StringReplaceInForm};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TStringReplaceInForm, StringReplaceInForm);
  Application.Run;
end.

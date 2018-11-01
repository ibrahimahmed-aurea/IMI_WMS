@echo off
setlocal
set server=%~1
echo %server%
set service=%~2
echo %service%

SC \\%server% query "%service%" | FIND "STATE" >NUL
IF errorlevel 1 GOTO SystemOffline

:ResolveInitialState
SC \\%server% query "%service%" | FIND "STATE" | FIND "RUNNING" >NUL
IF errorlevel 0 GOTO Stopservice
SC \\%server% query "%service%" | FIND "STATE" | FIND "STOPPED" >NUL
IF errorlevel 0 GOTO Stopedservice
SC \\%server% query "%service%" | FIND "STATE" | FIND "PAUSED" >NUL
IF errorlevel 0 GOTO SystemOffline
echo Service State is changing, waiting for service to resolve its state before making changes
sc \\%server% query "%service%" | Find "STATE"
timeout /t 2 /nobreak >NUL
GOTO ResolveInitialState

:Stopservice
echo Stopping %service% on \\%server%
sc \\%server% stop "%service%" 4:4:14 "Daily Bulid" >NUL

GOTO Stopingservice
:StopingserviceDelay
echo Waiting for service to stop
timeout /t 2 /nobreak >NUL
:Stopingservice
SC \\%server% query "%service%" | FIND "STATE" | FIND "STOPPED" >NUL
IF errorlevel 1 GOTO StopingserviceDelay
GOTO :Stopedservice

:SystemOffline
echo server \\%server% or service %service% is not accessible or is offline
GOTO:eof

:Stopedservice
echo %service% on \\%server% is stopped

endlocal

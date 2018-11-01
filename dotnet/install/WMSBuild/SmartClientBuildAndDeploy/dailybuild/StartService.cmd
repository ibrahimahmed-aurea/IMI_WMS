@echo off
setlocal
set server=%~1
echo %server%
set service=%~2
echo %service%

echo Starting service %service" on %server%
sc \\%server% start "%service%"


endlocal
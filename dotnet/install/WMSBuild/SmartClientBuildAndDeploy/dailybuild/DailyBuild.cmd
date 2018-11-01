C:
cd "C:\Build\Standard\Main"
call Build

c:
cd "C:\Build\Standard\716M"
call Build

c:
cd "C:\Build\Standard\72M"
call Build

c:
cd "C:\Build"
call DiskCleanUp

:End
echo %ERRORLEVEL%
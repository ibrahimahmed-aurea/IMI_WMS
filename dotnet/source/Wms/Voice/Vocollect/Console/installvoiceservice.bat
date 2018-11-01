@echo off
if %1X == X goto usage
if %2X == X goto usage
installservice %1 Imi.Wms.Voice.Vocollect Imi.Wms.Voice.Vocollect.VocollectInstance %2
goto end

:usage
echo. 
echo Installs a IMI iWMS Voice instance as a Windows Service.
echo. 
echo installvoiceservice [/i or /u] instance
echo. 
echo    /i              Installs the service.
echo    /u              Uninstalls the service.
echo    instance        The name of the instance to create a service for.
echo. 
:end

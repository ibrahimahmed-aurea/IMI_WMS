
set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
set TrackDrive=N:
call "setn.bat"


rem Disable smart client build untill we get a license for ActiPro as it blocks the build
rem Echo Building the smart client front end
rem %MsBuild% /p:Configuration=Release /p:Platform=x86 /v:m %TrackDrive%\source\SupplyChain\SmartClient_Frontend.sln

Echo Building the smart client backend end
%MsBuild% /p:Configuration=Release /p:Platform="Mixed Platforms" /v:m %TrackDrive%\source\SupplyChain\Server\SupplyChain.Server.sln

Echo Building the Deployment Manager
%MsBuild% /p:Configuration=Release /p:Platform="Mixed Platforms" /v:m %TrackDrive%\source\SupplyChain\Deployment\SupplyChain.Deployment.sln"

Echo Building the voice server
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m %TrackDrive%\source\Wms\Voice\Vocollect\Wms.Voice.Vocollect.sln

Echo Building the Thin Client Server
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\Wms\Mobile\Server\Wms.Mobile.Server.sln"

Echo Building the Thin Client Desktop App
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\Wms\Mobile\UI\WindowsDesktop\WindowsDesktopClient.sln"

Echo Building the Thin Client Windows 6 Mobile App
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\Wms\Mobile\UI\WindowsMobile\ThinClient.sln"

Echo Building the Meta Manager
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\CodeGenerators\MetaManager\MetaManager.sln"

Echo Building the web services code generator
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\CodeGenerators\WebServices\CodeGenerators.WebServices.sln"

Echo Building the Authorization Manager 
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\Utils\AzmanTool\AuthorizationManagerTool.sln"

Echo Building the Generated WebServices
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\Wms\WebServices\Wms.WebServices.sln"

Echo Building the generated 3pl web services
%MsBuild% /p:Configuration=Release /p:Platform=x86 /v:m %TrackDrive%\source\Wms\WebServices\SyncWS\Wms.WebServices.SyncWS.sln





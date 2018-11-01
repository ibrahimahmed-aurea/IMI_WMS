call "%VCINSTALLDIR%vcvarsall" x64
msbuild /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU" ..\..\Wms\WebServices\Wms.WebServices.sln 
msbuild /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU" ..\..\Wms\WebServices\SyncWS\Wms.WebServices.SyncWS.sln 
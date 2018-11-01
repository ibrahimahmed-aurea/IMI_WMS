call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
@echo off
set ScriptDir=%CD%\..\..
set TrackDrive=B:
set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
set ClearTool=cleartool.exe
set SourceDir=%~1
set Action=%2
set MetaManagerInstanceName=%3
set MetaManagerDir=%~4
set DeploymentManagerInstanceName=%5
set DeploymentManagerDir=%~6

REM Checks parameters and sets the MetaManager and DeploymentManager dirs to point to the SourceDir if they parameters have the value BUILD.
if %1X==X goto Usage

if %MetaManagerDir%==BUILD set MetaManagerDir=%TrackDrive%\source\CodeGenerators\MetaManager\CLI\bin\Release

if %DeploymentManagerDir%==BUILD set DeploymentManagerDir=%TrackDrive%\source\SupplyChain\Deployment\CLI\SupplyChain.Deployment.CLI.exe\bin\Release


REM Updates the dotnet folder in the SourceDir view in clear case
%ClearTool% update "%SourceDir%\dotnet"

if %ERRORLEVEL% NEQ 0 goto End

REM Sets the dotnet folder in the SourceDir to drive
subst %TrackDrive% /d
subst %TrackDrive% "%SourceDir%\dotnet"

if %ERRORLEVEL% NEQ 0 goto End

REM If Action is set to DEPLOY skip the build step and jump to the deploy step.
if %Action%==DEPLOY goto Deploy

REM Updates the metadata for MetaManager in clear case
%ClearTool% update "%SourceDir%\metadata"

if %ERRORLEVEL% NEQ 0 goto End

%TrackDrive%


REM If the parameter for MetaManagerDir where set to BUILD. Build the MetaManager solution. If not skip and continue to generate code.
if not %4==BUILD goto Generate
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\CodeGenerators\MetaManager\MetaManager.sln"
if %ERRORLEVEL% NEQ 0 goto End


:Generate

%MetaManagerDir:~0,2%

cd "%MetaManagerDir%"

REM Generate code fore all Applications.
"%MetaManagerDir%\MetaManager.CLI.exe" /Instance %MetaManagerInstanceName% /Frontend All /Backend All /Action generate /IgnoreCheckOuts true
if %ERRORLEVEL% NEQ 0 goto End

%TrackDrive%

REM Build the frontend and backend solutions. This will result in a new kit for smart clinet frontend and new server dlls to be deployed. 
%MsBuild% /p:Configuration=Release /p:Platform=x86 /v:m %TrackDrive%\source\SupplyChain\SmartClient_Frontend.sln
if ERRORLEVEL 1 goto End
%MsBuild% /p:Configuration=Release /p:Platform="Mixed Platforms" /v:m %TrackDrive%\source\SupplyChain\Server\SupplyChain.Server.sln
if ERRORLEVEL 1 goto End

if %Action%==BUILD goto End

:Deploy

REM If the parameter for DeploymentManagerDir where set to BUILD. Build the DeploymentManager solution. If not skip and continue deploying the system.
if not %6==BUILD goto DoDeploy
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /v:m "%TrackDrive%\source\SupplyChain\Deployment\SupplyChain.Deployment.sln"
if %ERRORLEVEL% NEQ 0 goto End

:DoDeploy

set DeployServer=ukimidev01
set DeployServerShareName=AppServer_Main
set ServiceName=IMI Supply Chain Application Server
set ServiceInstanceSC=WHTRUNK_SMARTCLIENT
set ServiceInstanceRMS=WHTRUNK_RMS
set ServiceInstanceWMS=WHTRUNK_WMS

REM Deploy the Smart Client kit via DeploymentManager
"%DeploymentManagerDir%\Imi.SupplyChain.Deployment.CLI.exe" /Action Import /KitFile %TrackDrive%\source\SupplyChain\UX\Deployment\bin\x86\Release\SmartClient_Latest.zip /Instance %DeploymentManagerInstanceName%
if %ERRORLEVEL% NEQ 0 goto End

REM Stops Application Server Manager GUI and the server instances running on the deploy server.
taskkill /S %DeployServer% /IM Imi.SupplyChain.Server.UI.exe

call "%ScriptDir%\stopservice.cmd" %DeployServer% "%ServiceName% (%ServiceInstanceSC%)"
if %ERRORLEVEL% NEQ 0 goto End
call "%ScriptDir%\stopservice.cmd" %DeployServer% "%ServiceName% (%ServiceInstanceRMS%)"
if %ERRORLEVEL% NEQ 0 goto End
call "%ScriptDir%\stopservice.cmd" %DeployServer% "%ServiceName% (%ServiceInstanceWMS%)"
if %ERRORLEVEL% NEQ 0 goto End

REM Copys the dell files and the authorisation xml files to the deploy server. 
echo Replacing server files 
xcopy "%TrackDrive%\source\SupplyChain\Server\Console\bin\Release\*.dll" "\\%DeployServer%\%DeployServerShareName%\bin\" /Y /R
if %ERRORLEVEL% NEQ 0 goto End
xcopy "%TrackDrive%\source\SupplyChain\Server\Console\instance\SmartClientDevelop\init\*.xml" "\\%DeployServer%\%DeployServerShareName%\instance\%ServiceInstanceSC%\init\AuthorizationMerge" /Y /R
if %ERRORLEVEL% NEQ 0 goto End

REM Starts the server instances on the deploy server.
call "%ScriptDir%\startservice.cmd" %DeployServer% "%ServiceName% (%ServiceInstanceSC%)"
if %ERRORLEVEL% NEQ 0 goto End
call "%ScriptDir%\startservice.cmd" %DeployServer% "%ServiceName% (%ServiceInstanceRMS%)"
if %ERRORLEVEL% NEQ 0 goto End
call "%ScriptDir%\startservice.cmd" %DeployServer% "%ServiceName% (%ServiceInstanceWMS%)"
if %ERRORLEVEL% NEQ 0 goto End

goto End

:Usage
echo Builds and deploys the iWMS Suite.
echo.
echo AUTOBUILDANDDEPLOY SourceDir Action MetaManagerInstanceName MetaManagerDir 
echo                    DeploymentManagerInstanceName DeploymentManagerDir
echo.                                                                                                                           
echo  SourceDir                     Path to directory containing folders for 
echo                                metadata and dotnet source.
echo  Action                        Determines wich action to perform. 
echo                                Can have three different values:
echo                                BUILD  : Only genereates and builds the system.
echo                                DEPLOY : Only deploys the latest build.
echo                                ALL    : Both builds and deploys the system.
echo.                                                                                                                           
echo  MetaManagerInstanceName       The name of the configuration instance
echo                                for MetaManager.
echo  MetaManagerDir                MetaManager installation directory.
echo                                If this parameter is set to BUILD.
echo                                MetaManager will be buildt from SourceDir.
echo  DeploymentManagerInstanceName The name of the instance to update.
echo  DeploymentManagerDir          DeploymentManager installation directory.
echo                                If this parameter is set to BUILD. 
echo                                DeploymentManager will be buildt from SourceDir.
echo.
:End
echo %ERRORLEVEL%
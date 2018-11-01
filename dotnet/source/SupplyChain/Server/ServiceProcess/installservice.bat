@echo off
rem
rem Install script for IMI Supply Chain Application Server
rem 
rem Copyright (c) APTEAN 2012. All rights reserved.
rem

set NetHome=%windir%\microsoft.net\framework\v4.0.30319

if %NetHome%X == X goto NotSet

if NOT EXIST %NetHome%\installutil.exe goto NoUtil

rem ---------------------
rem Check for switch (%1)
rem ---------------------
if %1X == X goto Usage

rem ---------------------
rem Install
rem ---------------------
if NOT %1 == /i goto CheckUnInstall
if %2%X == X goto Usage
%NetHome%\installutil /SystemId=%2 ..\bin\Imi.SupplyChain.Server.ServiceProcess.exe
goto End

rem ---------------------
rem UnInstall
rem ---------------------

:CheckUnInstall

if NOT %1% == /u goto Usage
if %2%X == X goto Usage
%NetHome%\installutil /u /SystemId=%2 ..\bin\Imi.SupplyChain.Server.ServiceProcess.exe
goto End

rem ---------------------
rem NetHome not set
rem ---------------------
:NotSet
echo The environment variable NetHome is not set. Please set it
echo to the base directory for the current .NET framework which
echo is usually found in a subdirectory under 
echo c:\Windows\Microsoft.Net\Framework
goto End

rem ---------------------------
rem Cannot find installutil.exe
rem ---------------------------
:NoUtil
echo Cannot find the installutil.exe program. It should be in the
echo directory pointed to by the NetHome system variable, which
echo is currently set to %NetHome%
goto End

:Usage
echo Error: Bad input parameters.
echo Usage: installservice [ /i OR /u ] InstanceName

:End

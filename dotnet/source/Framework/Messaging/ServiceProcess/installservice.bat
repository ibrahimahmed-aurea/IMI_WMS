@echo off
rem
rem Install script for IMI Supply Chain Windows Service
rem 
rem Copyright (c) APTEAN 2010. All rights reserved.
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
if %3%X == X goto Usage
if %4%X == X goto Usage
pushd ..\bin
%NetHome%\installutil /assemblyName=%2 /instanceTypeName=%3 /instanceName=%4 ..\bin\Imi.Framework.Messaging.Service.exe
popd
goto End

rem ---------------------
rem UnInstall
rem ---------------------

:CheckUnInstall

if NOT %1% == /u goto Usage
if %2%X == X goto Usage
if %3%X == X goto Usage
if %4%X == X goto Usage
pushd ..\bin
%NetHome%\installutil /u /assemblyName=%2 /instanceTypeName=%3 /instanceName=%4 Imi.Framework.Messaging.Service.exe
popd
goto End

rem ---------------------
rem NetHome not set
rem ---------------------
:NotSet
echo The environment variable NetHome is not set. Please set it
echo to the base directory for the current .NET framework which
echo is usually found in a subdirectory under 
echo %WINDIR%\Microsoft.Net\Framework
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
echo Usage: installservice [ /i OR /u ] instanceName

:End

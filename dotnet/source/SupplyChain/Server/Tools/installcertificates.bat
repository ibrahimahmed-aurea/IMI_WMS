@echo off
rem
rem Install script for IMI Supply Chain Application Server
rem 
rem Copyright (c) Aptean. All rights reserved.
rem

set MAKECERT=makecert
set CERTUTIL=certutil

rem ---------------------
rem Check for switches
rem ---------------------
if %1X == X goto Usage
if %2X == X goto Usage

rem ---------------------
rem Install
rem ---------------------

%MAKECERT% -r -pe -sky exchange -n "CN=%1" -ss My -sr LocalMachine %1.cer
%CERTUTIL% -f -v -p %2 -exportPFX "%1" %1.pfx
%CERTUTIL% -delstore My "%1"
%CERTUTIL% -addstore root %1.cer
%CERTUTIL% -p %2 -importpfx %1.pfx

%MAKECERT% -r -pe -sky exchange -n "CN=imi-token-sign" -ss My -sr LocalMachine imi-token-sign.cer
%CERTUTIL% -f -v -p %2 -exportPFX "imi-token-sign" imi-token-sign.pfx
%CERTUTIL% -delstore My "imi-token-sign"
%CERTUTIL% -addstore root imi-token-sign.cer
%CERTUTIL% -p %2 -importpfx imi-token-sign.pfx

goto End

:Usage
echo Error: Bad input parameters.
echo Usage: installcertificates hostname password

:End
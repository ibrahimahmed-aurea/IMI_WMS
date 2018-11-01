set genin=..\wscc\genin
set geninrms=..\wscc\geninrms
set genout=..\wscc\genout
set genoutrms=..\wscc\genoutrms
set hapi=..\wscc\hapi
set tapi=..\wscc\tapi
set tapios=..\wscc\tapios
set intdesc=..\wscc\InterfaceDescription

rmdir /S /Q %intdesc%

unzip -u ..\..\..\..\doc\Integration\InterfaceDescription.zip -d %intdesc%

@ECHO OFF

pushd "%intdesc%\Transportation\TAPI\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

pushd "%intdesc%\Transportation\TAPIOS\Inbound\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

pushd "%intdesc%\Transportation\TAPIOS\Outbound\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

pushd "%intdesc%\Transportation\WebServices\Inbound\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

pushd "%intdesc%\Transportation\WebServices\Outbound\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd


pushd "%intdesc%\Warehouse\HAPI\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

pushd "%intdesc%\Warehouse\WebServices\Inbound\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

pushd "%intdesc%\Warehouse\WebServices\Outbound\Doc"
del /q *.* 
for /f "Tokens=*" %%G in ('dir /B') do rd /s /q "%%G"
popd

@ECHO ON

xcopy /Y /E %tapi%\html %intdesc%\Transportation\TAPI\Doc
xcopy /Y /E %tapios%\html %intdesc%\Transportation\TAPIOS\Inbound\Doc
xcopy /Y /E %genoutrms%\html %intdesc%\Transportation\TAPIOS\Outbound\Doc
xcopy /Y /E %genoutrms%\html %intdesc%\Transportation\WebServices\Outbound\Doc
xcopy /Y /E %geninrms%\html %intdesc%\Transportation\WebServices\Inbound\Doc

xcopy /Y /E %hapi%\html %intdesc%\Warehouse\HAPI\Doc
xcopy /Y /E %genin%\html %intdesc%\Warehouse\WebServices\Inbound\Doc
xcopy /Y /E %genout%\html %intdesc%\Warehouse\WebServices\Outbound\Doc

set genin=
set geninrms=
set genout=
set genoutrms=
set hapi=
set tapi=
set tapios=
set intdesc=

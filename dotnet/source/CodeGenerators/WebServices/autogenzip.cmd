set PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 2010\VC\bin

call envsetup.cmd

msbuild /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild CodeGenerators.WebServices.sln 

call copy_to_wscc.cmd

cd ..\wscc

nmake inmakefile
nmake outmakefile
nmake hapi_makefile
nmake inmakefilerms
nmake outmakefilerms
nmake tapi_makefile
nmake tapios_makefile

cd ..\WebServices

call InterfaceDescription.cmd
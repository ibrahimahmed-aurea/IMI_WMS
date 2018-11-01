call envsetup.cmd
msbuild /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild CodeGenerators.WebServices.sln 
call copy_to_wscc.cmd

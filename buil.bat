set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

%MsBuild%  /p:Configuration=Release /p:Platform="Any CPU" /v:m Test_Jenkins/Test_Jenkins.sln
set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

%MsBuild%  /p:Configuration=Release /p:Platform="Any CPU" /tv:14.0 /v:m  Test_Jenkins/Test_Jenkins.sln
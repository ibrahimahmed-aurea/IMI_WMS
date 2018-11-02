set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
set nuget=c:\tools\nuget.exe

Echo Restore the unit tests solution packages
%nuget% restore Test_Jenkins/Test_Jenkins.sln

%MsBuild%  /p:Configuration=Release /p:Platform="Any CPU" /tv:14.0 /v:m  Test_Jenkins/Test_Jenkins.sln
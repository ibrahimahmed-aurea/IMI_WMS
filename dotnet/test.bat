set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
set TrackDrive=N:

Echo Build the unit tests solution
%MsBuild% /p:Configuration=Release /p:Platform="Any CPU" /tv:14.0  /v:m "%TrackDrive%\source\UnitTests\WMSUnitTests.sln"

Echo Start testing..
"C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe" "%TrackDrive%\source\UnitTests\WMSUnitTests\bin\Release\WMSUnitTests.dll"


cd..\..\..\..\..\

FOR /D %%p IN (".\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\*.*") DO rmdir "%%p" /s /q
FOR /D %%p IN (".\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\*.*") DO rmdir "%%p" /s /q


rmdir .\server\OutputManager\oracle\pak\body\temp
md .\server\OutputManager\oracle\pak\body\temp
for %%1 in (dir .\server\OutputManager\oracle\pak\body\*.body) do wrap iname=%%1 oname=.\server\OutputManager\oracle\pak\body\temp\%%~n1.plb


rmdir .\server\Transportation\oracle\pak\body\temp
md .\server\Transportation\oracle\pak\body\temp
for %%1 in (dir .\server\Transportation\oracle\pak\body\*.body) do wrap iname=%%1 oname=.\server\Transportation\oracle\pak\body\temp\%%~n1.plb


rmdir .\server\Warehouse\oracle\pak\body\temp
md .\server\Warehouse\oracle\pak\body\temp
for %%1 in (dir .\server\Warehouse\oracle\pak\body\*.body) do wrap iname=%%1 oname=.\server\Warehouse\oracle\pak\body\temp\%%~n1.plb

md .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager
md .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation
md .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse

md .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle

md .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle

md .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle

xcopy /S /I /E .\server\OutputManager\oracle\table .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\table\
xcopy /S /I /E .\server\OutputManager\oracle\trigger .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\trigger\
xcopy /S /I /E .\server\OutputManager\oracle\view .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\view\
xcopy /S /I /E .\server\OutputManager\oracle\codes .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\codes\
xcopy /S /I /E .\server\OutputManager\oracle\user .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\user\

xcopy /S /I /E .\server\OutputManager\oracle\pak\spec .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\pak\spec\
xcopy /S /I /E .\server\OutputManager\oracle\pak\body .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\pak\body\
 

xcopy /S /I /E .\server\Transportation\oracle\table .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\table\
xcopy /S /I /E .\server\Transportation\oracle\trigger .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\trigger\
xcopy /S /I /E .\server\Transportation\oracle\view .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\view\
xcopy /S /I /E .\server\Transportation\oracle\codes .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\codes\
xcopy /S /I /E .\server\Transportation\oracle\user .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\user\

xcopy /S /I /E .\server\Transportation\oracle\pak\spec .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\pak\spec\
xcopy /S /I /E .\server\Transportation\oracle\pak\body .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Transportation\oracle\pak\body\
 

xcopy /S /I /E .\server\Warehouse\oracle\table .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\table\
xcopy /S /I /E .\server\Warehouse\oracle\trigger .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\trigger\
xcopy /S /I /E .\server\Warehouse\oracle\view .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\view\
xcopy /S /I /E .\server\Warehouse\oracle\codes .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\codes\
xcopy /S /I /E .\server\Warehouse\oracle\user .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\user\

xcopy /S /I /E .\server\Warehouse\oracle\pak\spec .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\pak\spec\
xcopy /S /I /E .\server\Warehouse\oracle\pak\body .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\pak\body\


xcopy /S /I /E .\server\install\warehouse .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\Warehouse\oracle\
xcopy /S /I /E .\server\install\transportation .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\transportation\oracle\
xcopy /S /I /E .\server\install\OutputManager .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp\OutputManager\oracle\

del .\dotnet\install\WMSBuild\DatabaseSchemaBuild\release\WMSDatabaseObjects.zip
 
.\dotnet\install\utility\zipit .\dotnet\install\WMSBuild\DatabaseSchemaBuild\release\WMSDatabaseObjects.zip .\dotnet\install\WMSBuild\DatabaseSchemaBuild\temp
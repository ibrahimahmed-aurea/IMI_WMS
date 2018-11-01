C:
cd "C:\Build\Standard\Main"
call AutoBuildAndDeploy "C:\Views\pe_Main_ss" ALL Main BUILD WHTRUNK BUILD

REM Deletes all deploy folders older than four days
ForFiles /P C:\Views\pe_Main_ss\dotnet\source\SupplyChain\UX\Deployment\bin\x86\Release /D -4 /C "CMD /C if @ISDIR==TRUE echo RD /Q @FILE &RD /Q /S @FILE"
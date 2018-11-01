REM Deletes all deploy folders older than two days from main track
ForFiles /P C:\Users\wmsauto\latest_ss\dotnet\source\SupplyChain\UX\Deployment\bin\x86\Release /D -4 /C "CMD /C if @ISDIR==TRUE echo RD /Q @FILE &RD /Q /S @FILE"

REM Deletes all deploy folders older than two days from 716M track
ForFiles /P C:\Users\wmsauto\716M_ss\dotnet\source\SupplyChain\UX\Deployment\bin\x86\Release /D -4 /C "CMD /C if @ISDIR==TRUE echo RD /Q @FILE &RD /Q /S @FILE"

REM Deletes all product version folders older than 5 days from staging area of ukimidev01
PushD "\\ukimidev01\StagingArea\SmartClient\Versions" &&("forfiles.exe" /D -5 /C "CMD /C if @ISDIR==TRUE echo RD /Q @FILE &RD /Q /S @FILE") & PopD
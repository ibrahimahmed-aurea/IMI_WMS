rmdir temp /s /q
rmdir Resources /s /q
mkdir temp
mkdir Resources
for %%1 in (dir *.exe,*.dll) do call decompile.bat %%1 %%1
del .\Resources\*.g.resources
for %%1 in (dir .\Resources\*.resources) do resgen .\Resources\%%~n1.resources .\Resources\%%~n1.resx
del .\Resources\*.resources
del .\Resources\*.Properties.Resources.resx
rmdir temp /s /q

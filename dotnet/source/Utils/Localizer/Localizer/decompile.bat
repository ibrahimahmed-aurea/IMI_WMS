del temp\*.* /q
ildasm /unicode /nobar /out=temp\%1 %2
dir temp\imi.*.resources > .\Resources\%1.content
copy temp\imi.*.resources .\Resources

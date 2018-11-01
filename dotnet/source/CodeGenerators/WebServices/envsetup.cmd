mkdir ..\wscc
mkdir ..\wscc\bin
mkdir ..\wscc\xml
mkdir ..\wscc\hapi\html\css
mkdir ..\wscc\hapi\html\img
mkdir ..\wscc\tapi\html\css
mkdir ..\wscc\tapi\html\img
mkdir ..\wscc\tapios\html\css
mkdir ..\wscc\tapios\html\img

copy inmakefile ..\wscc
copy outmakefile ..\wscc
copy inmakefilerms ..\wscc
copy outmakefilerms ..\wscc
copy dictmakefile ..\wscc
copy dictmakefilerms ..\wscc
copy dictmakefilemapi ..\wscc
copy hapi_makefile ..\wscc
copy tapi_makefile ..\wscc
copy tapios_makefile ..\wscc
copy inmakefilemapi ..\wscc
copy outmakefilemapi ..\wscc

copy XML2HAPIHtml\html\master\css\* ..\wscc\hapi\html\css
copy XML2HAPIHtml\html\master\img\* ..\wscc\hapi\html\img
copy XML2HAPIHtml\html\master\hapi_index.html ..\wscc\hapi\html\index.html
copy XML2HAPIHtml\html\master\css\* ..\wscc\tapi\html\css
copy XML2HAPIHtml\html\master\img\* ..\wscc\tapi\html\img
copy XML2HAPIHtml\html\master\tapi_index.html ..\wscc\tapi\html\index.html
copy XML2HAPIHtml\html\master\css\* ..\wscc\tapios\html\css
copy XML2HAPIHtml\html\master\img\* ..\wscc\tapios\html\img
copy XML2HAPIHtml\html\master\tapios_index.html ..\wscc\tapios\html\index.html

copy ..\xml\structure\WSInboundMaster.xml ..\wscc\xml
copy ..\xml\structure\WSOutboundMaster.xml ..\wscc\xml
copy ..\xml\structure\HAPIInboundMaster.xml ..\wscc\xml
copy ..\xml\structure\HAPIOutboundMaster.xml ..\wscc\xml
copy ..\xml\structure\WSRMSInboundMaster.xml ..\wscc\xml
copy ..\xml\structure\WSRMSOutboundMaster.xml ..\wscc\xml
copy ..\xml\structure\WMSDictionary.xml ..\wscc\xml
copy ..\xml\structure\RMSDictionary.xml ..\wscc\xml
copy ..\xml\structure\EmptyDictionary.xml ..\wscc\xml
copy ..\xml\structure\TAPIDictionary.xml ..\wscc\xml
copy ..\xml\structure\HAPIDictionary.xml ..\wscc\xml
copy ..\xml\structure\TAPIInboundMaster.xml ..\wscc\xml
copy ..\xml\structure\TAPIOutboundMaster.xml ..\wscc\xml
copy ..\xml\structure\TAPIOSInboundMaster.xml ..\wscc\xml
rem TAPIOS outbound dos not exist!!! Manually created
copy ..\xml\structure\WSMAPIInboundMaster.xml ..\wscc\xml
copy ..\xml\structure\WSMAPIOutboundMaster.xml ..\wscc\xml

mkdir work
mkdir work\xml
mkdir work\hapi\html\css
mkdir work\hapi\html\img

copy inmakefile work
copy outmakefile work
copy hapi_makefile work
copy XML2HAPIHtml\html\css\* work\hapi\html\css
copy XML2HAPIHtml\html\img\* work\hapi\html\img
copy XML2HAPIHtml\html\index.html work\hapi\html

copy xml\structure\WSInboundMaster.xml work\xml
copy xml\structure\WSOutboundMaster.xml work\xml
copy xml\structure\HAPIInboundMaster.xml work\xml
copy xml\structure\HAPIOutboundMaster.xml work\xml

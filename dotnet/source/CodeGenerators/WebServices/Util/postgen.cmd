rem inmakefile

attrib -R ..\WebService\Inbound\html\*.*
copy /Y ..\wscc\genin\html\*.* ..\WebService\Inbound\html

attrib -R ..\..\..\server\oracle\pak\body\HapiRcv_Clean.body
copy /Y ..\wscc\genin\sql\HapiRcv_Clean.body ..\..\..\server\oracle\pak\body\HapiRcv_Clean.body

attrib -R ..\..\..\server\oracle\pak\body\HAPI_Rcv_Object.body
copy /Y ..\wscc\genin\sql\HAPI_Rcv_Object.body ..\..\..\server\oracle\pak\body\HAPI_Rcv_Object.body

attrib -R ..\..\..\server\oracle\pak\spec\HAPI_Rcv_Object.spec
copy /Y ..\wscc\genin\sql\HAPI_Rcv_Object.spec ..\..\..\server\oracle\pak\spec\HAPI_Rcv_Object.spec

attrib -R ..\..\..\server\oracle\table\Web_Services_RCV_Column.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_Column.def ..\..\..\server\oracle\table\Web_Services_RCV_Column.def

attrib -R ..\..\..\server\oracle\table\Web_Services_RCV_Comment.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_Comment.def ..\..\..\server\oracle\table\Web_Services_RCV_Comment.def

attrib -R ..\..\..\server\oracle\table\Web_Services_RCV_Drop.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_Drop.def ..\..\..\server\oracle\table\Web_Services_RCV_Drop.def

attrib -R ..\..\..\server\oracle\table\Web_Services_RCV_FK.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_FK.def ..\..\..\server\oracle\table\Web_Services_RCV_FK.def

attrib -R ..\..\..\server\oracle\table\Web_Services_RCV_PK.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_PK.def ..\..\..\server\oracle\table\Web_Services_RCV_PK.def

attrib -R ..\..\..\server\oracle\trigger\Web_Services_RCV_Trace.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_Trace.def ..\..\..\server\oracle\trigger\Web_Services_RCV_Trace.def

attrib -R ..\..\..\server\oracle\view\Web_Services_RCV_View.def
copy /Y ..\wscc\genin\sql\Web_Services_RCV_View.def ..\..\..\server\oracle\view\Web_Services_RCV_View.def

attrib -R ..\WebService\Inbound\auto\*.cs
copy /Y ..\wscc\genin\src\*.* ..\WebService\Inbound\auto

rem outmakefile

attrib -R ..\WebService\WebServiceSnd\html\*.*
copy /Y ..\wscc\genout\html\*.* ..\WebService\WebServiceSnd\html

attrib -R ..\WebService\WebServiceSnd\auto\*.*
copy /Y ..\wscc\genout\src\*.* ..\WebService\WebServiceSnd\auto

attrib -R ..\WebService\WSOutboundTester\auto\*.cs
copy /Y ..\wscc\genout\testsrc\*.cs ..\WebService\WSOutboundTester\auto

attrib -R ..\WebService\WSOutboundTester\setup\*.*
copy /Y ..\wscc\genout\testsrc\*.sql ..\WebService\WSOutboundTester\setup

rem BizTalk starter kit

attrib -R ..\WebService\BizTalk\FromWMS\BZTProxy\auto\*.cs
copy /Y ..\wscc\genout\bzt\*.cs ..\WebService\BizTalk\FromWMS\BZTProxy\auto

rem inmakefile

cleartool mklabel -rep -rec %1 ..\WebService\Inbound\html
cleartool mklabel -rep %1 ..\..\..\server\oracle\pak\body\HapiRcv_Clean.body
cleartool mklabel -rep %1 ..\..\..\server\oracle\pak\body\HAPI_Rcv_Object.body
cleartool mklabel -rep %1 ..\..\..\server\oracle\pak\spec\HAPI_Rcv_Object.spec
cleartool mklabel -rep %1 ..\..\..\server\oracle\table\Web_Services_RCV_Column.def
cleartool mklabel -rep %1 ..\..\..\server\oracle\table\Web_Services_RCV_Comment.def
cleartool mklabel -rep %1 ..\..\..\server\oracle\table\Web_Services_RCV_Drop.def
cleartool mklabel -rep %1 ..\..\..\server\oracle\table\Web_Services_RCV_FK.def
cleartool mklabel -rep %1 ..\..\..\server\oracle\table\Web_Services_RCV_PK.def
cleartool mklabel -rep %1 ..\..\..\server\oracle\trigger\Web_Services_RCV_Trace.def
cleartool mklabel -rep %1 ..\..\..\server\oracle\view\Web_Services_RCV_View.def
cleartool mklabel -rep -rec %1 ..\WebService\Inbound\auto

rem outmakefile

cleartool mklabel -rep -rec %1 ..\WebService\WebServiceSnd\html
cleartool mklabel -rep -rec %1 ..\WebService\WebServiceSnd\auto
cleartool mklabel -rep -rec %1 ..\WebService\WSOutboundTester\auto
cleartool mklabel -rep -rec %1 ..\WebService\WSOutboundTester\setup
cleartool mklabel -rep -rec %1 ..\WebService\BizTalk\FromWMS\BZTProxy\auto

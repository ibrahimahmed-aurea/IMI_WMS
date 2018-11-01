set WebServiceInbound=..\..\Wms\WebServices\ExternalInterfaceRMS
set wmsoracle=..\..\..\..\server\route\oracle
set WebServiceSend=..\..\Wms\Server\Job\WebServiceSendRMS
set WebServiceOutboundTester=..\..\Wms\WebServices\OutboundTesterRMS
set WebServiceBizTalkProxy=..\..\Wms\WebServices\BizTalk\FromWMS\BZTProxyRMS


rem inmakefile

cleartool mklabel -rep -rec %1 %WebServiceInbound%\html
cleartool mklabel -rep %1 %wmsoracle%\pak\body\MessageRcv_Clean.body
cleartool mklabel -rep %1 %wmsoracle%\pak\body\MessageRcv_Object.body
cleartool mklabel -rep %1 %wmsoracle%\pak\spec\MessageRcv_Object.spec
cleartool mklabel -rep %1 %wmsoracle%\table\auto\Web_Services_RCV_Column.def
cleartool mklabel -rep %1 %wmsoracle%\table\auto\Web_Services_RCV_Comment.def
cleartool mklabel -rep %1 %wmsoracle%\table\auto\Web_Services_RCV_Drop.def
cleartool mklabel -rep %1 %wmsoracle%\table\auto\Web_Services_RCV_FK.def
cleartool mklabel -rep %1 %wmsoracle%\table\auto\Web_Services_RCV_PK.def
cleartool mklabel -rep %1 %wmsoracle%\trigger\Web_Services_RCV_Trace.trigger
cleartool mklabel -rep %1 %wmsoracle%\view\Web_Services_RCV_View.def
cleartool mklabel -rep -rec %1 %WebServiceInbound%\auto

rem outmakefile

cleartool mklabel -rep -rec %1 %WebServiceSend%\html
cleartool mklabel -rep -rec %1 %WebServiceSend%\auto
cleartool mklabel -rep -rec %1 %WebServiceOutboundTester%\auto
cleartool mklabel -rep -rec %1 %WebServiceOutboundTester%\setup
cleartool mklabel -rep -rec %1 %WebServiceBizTalkProxy%\auto

set WebServiceBizTalkProxy=
set WebServiceOutboundTester=
set WebServiceSend=
set wmsoracle=
set WebServiceInbound=

set genin=..\wscc\geninrms
set genout=..\wscc\genoutrms
set WebServiceInbound=..\..\Wms\WebServices\ExternalInterfaceRMS
set wmsoracle=..\..\..\..\server\Transportation\oracle
set WebServiceSend=..\..\SupplyChain\Server\Job\WebServiceSendRMS
set WebServiceOutboundTester=..\..\Wms\WebServices\OutboundTesterRMS
set WebServiceBizTalkProxy=..\..\Wms\WebServices\BizTalk\FromWMS\BZTProxyRMS

rem inmakefile

attrib -R %WebServiceInbound%\html\*.*
copy /Y %genin%\html\*.* %WebServiceInbound%\html

attrib -R %wmsoracle%\pak\body\MessageRcv_Clean.body
copy /Y %genin%\sql\MessageRcv_Clean.body %wmsoracle%\pak\body\MessageRcv_Clean.body

attrib -R %wmsoracle%\pak\body\MessageRcv_Object.body
copy /Y %genin%\sql\MessageRcv_Object.body %wmsoracle%\pak\body\MessageRcv_Object.body

attrib -R %wmsoracle%\pak\spec\MessageRcv_Object.spec
copy /Y %genin%\sql\MessageRcv_Object.spec %wmsoracle%\pak\spec\MessageRcv_Object.spec

attrib -R %wmsoracle%\table\auto\Web_Services_RCV_Column.def
copy /Y %genin%\sql\Web_Services_RCV_Column.def %wmsoracle%\table\auto\Web_Services_RCV_Column.def

attrib -R %wmsoracle%\table\auto\Web_Services_RCV_Comment.def
copy /Y %genin%\sql\Web_Services_RCV_Comment.def %wmsoracle%\table\auto\Web_Services_RCV_Comment.def

attrib -R %wmsoracle%\table\auto\Web_Services_RCV_Drop.def
copy /Y %genin%\sql\Web_Services_RCV_Drop.def %wmsoracle%\table\auto\Web_Services_RCV_Drop.def

attrib -R %wmsoracle%\table\auto\Web_Services_RCV_FK.def
copy /Y %genin%\sql\Web_Services_RCV_FK.def %wmsoracle%\table\auto\Web_Services_RCV_FK.def

attrib -R %wmsoracle%\table\auto\Web_Services_RCV_PK.def
copy /Y %genin%\sql\Web_Services_RCV_PK.def %wmsoracle%\table\auto\Web_Services_RCV_PK.def

attrib -R %wmsoracle%\trigger\Web_Services_RCV_Trace.trigger
copy /Y %genin%\sql\Web_Services_RCV_Trace.trigger %wmsoracle%\trigger\Web_Services_RCV_Trace.trigger

attrib -R %wmsoracle%\view\Web_Services_RCV_View.def
copy /Y %genin%\sql\Web_Services_RCV_View.def %wmsoracle%\view\Web_Services_RCV_View.def

attrib -R %WebServiceInbound%\auto\*.cs
copy /Y %genin%\src\*.* %WebServiceInbound%\auto

rem outmakefile

attrib -R %WebServiceSend%\html\*.*
copy /Y %genout%\html\*.* %WebServiceSend%\html

attrib -R %WebServiceSend%\auto\*.*
copy /Y %genout%\src\*.* %WebServiceSend%\auto

attrib -R %WebServiceOutboundTester%\auto\*.cs
copy /Y %genout%\testsrc\*.cs %WebServiceOutboundTester%\auto

attrib -R %WebServiceOutboundTester%\setup\*.*
copy /Y %genout%\testsrc\*.sql %WebServiceOutboundTester%\setup

rem BizTalk starter kit

attrib -R %WebServiceBizTalkProxy%\auto\*.cs
copy /Y %genout%\bzt\*.cs %WebServiceBizTalkProxy%\auto

set WebServiceBizTalkProxy=
set WebServiceOutboundTester=
set WebServiceSend=
set wmsoracle=
set WebServiceInbound=
set genout=
set genin=

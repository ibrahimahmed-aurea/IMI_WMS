#
# makefile 17/12/2004
# 
# Copyright (c) 2004 - 2005 Industri-Matematik AB. All rights reserved.
#
ROOT=n:\source
WEBSERVICESROOT=$(ROOT)\Wms\WebServices

DEPLOYDIR=$(ROOT)\..\install\WebServices\Files

WMSDEPLOYDIR=$(DEPLOYDIR)\WMS
WMSDEPLOYBINDIR=$(DEPLOYDIR)\WMS\bin
WMSSITEROOT=$(WEBSERVICESROOT)\WebSite
WMSSITEBINROOT=$(WEBSERVICESROOT)\WebSite\bin

WMSDEPLOYHTMLIN=$(WMSDEPLOYDIR)\doc\recipient
WMSDEPLOYHTMLOUT=$(WMSDEPLOYDIR)\doc\sender
WMSSITEHTMLIN=$(WEBSERVICESROOT)\ExternalInterface\html
WMSSITEHTMLOUT=$(ROOT)\SupplyChain\Server\Job\WebServiceSend\html

RMSDEPLOYDIR=$(DEPLOYDIR)\RMS
RMSDEPLOYBINDIR=$(DEPLOYDIR)\RMS\bin
RMSSITEROOT=$(WEBSERVICESROOT)\WebSiteRMS
RMSSITEBINROOT=$(WEBSERVICESROOT)\WebSiteRMS\bin

RMSDEPLOYHTMLIN=$(RMSDEPLOYDIR)\doc\recipient
RMSDEPLOYHTMLOUT=$(RMSDEPLOYDIR)\doc\sender
RMSSITEHTMLIN=$(WEBSERVICESROOT)\ExternalInterfaceRMS\html
RMSSITEHTMLOUT=$(ROOT)\SupplyChain\Server\Job\WebServiceSendRMS\html

WMSIRENDEPLOYDIR=$(DEPLOYDIR)\WMSiRen
WMSIRENDEPLOYBINDIR=$(DEPLOYDIR)\WMSiRen\bin
WMSSITEIRENROOT=$(WEBSERVICESROOT)\WebSiteiRen
WMSSITEIRENBINROOT=$(WEBSERVICESROOT)\WebSiteiRen\bin

WMSIRENDEPLOYHTMLIN=$(WMSIRENDEPLOYDIR)\doc\recipient
WMSIRENDEPLOYHTMLOUT=$(WMSIRENDEPLOYDIR)\doc\sender
WMSSITEIRENHTMLIN=$(WEBSERVICESROOT)\ExternalInterface\html
WMSSITEIRENHTMLOUT=$(ROOT)\SupplyChain\Server\Job\WebServiceSend\html

WOTDEPLOYDIR=$(DEPLOYDIR)\WMSOutboundTester
WOTDEPLOYBINDIR=$(DEPLOYDIR)\WMSOutboundTester\bin
WOTSITEROOT=$(WEBSERVICESROOT)\WebSiteOutboundTester
WOTSITEBINROOT=$(WEBSERVICESROOT)\WebSiteOutboundTester\bin

ROTDEPLOYDIR=$(DEPLOYDIR)\RMSOutboundTester
ROTDEPLOYBINDIR=$(DEPLOYDIR)\RMSOutboundTester\bin
ROTSITEROOT=$(WEBSERVICESROOT)\WebSiteOutboundTesterRMS
ROTSITEBINROOT=$(WEBSERVICESROOT)\WebSiteOutboundTesterRMS\bin

WOTIRENDEPLOYDIR=$(DEPLOYDIR)\WMSOutboundTesteriRen
WOTIRENDEPLOYBINDIR=$(DEPLOYDIR)\WMSOutboundTesteriRen\bin
WOTSITEIRENROOT=$(WEBSERVICESROOT)\WebSiteOutboundTesteriRen
WOTSITEIRENBINROOT=$(WEBSERVICESROOT)\WebSiteOutboundTesteriRen\bin

C3PLDEPLOYDIR=$(DEPLOYDIR)\C3PL
C3PLDEPLOYBINDIR=$(DEPLOYDIR)\C3PL\bin
C3PLSITEROOT=$(WEBSERVICESROOT)\SyncWS\Wms.WebServices.SyncWS.WebSite
C3PLSITEBINROOT=$(WEBSERVICESROOT)\SyncWS\Wms.WebServices.SyncWS.WebSite\bin

RD=rd /s /q
MD=md
CP=copy /y
RM=del

WMSBIN= \
    "$(WMSDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(WMSDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(WMSDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(WMSDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(WMSDEPLOYBINDIR)\Wms.WebServices.ExternalInterface.dll"

WMSCONFIG= \
    "$(WMSDEPLOYDIR)\Web.Config" \
    "$(WMSDEPLOYDIR)\ExternalInterface.asmx" \
    "$(WMSDEPLOYDIR)\icon\IMI Warehouse.ico" \
    "$(WMSDEPLOYHTMLIN)\index.html" \
    "$(WMSDEPLOYHTMLOUT)\index.html"

RMSBIN= \
    "$(RMSDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(RMSDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(RMSDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(RMSDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(RMSDEPLOYBINDIR)\Wms.WebServices.ExternalInterfaceRMS.dll"

RMSCONFIG= \
    "$(RMSDEPLOYDIR)\Web.Config" \
    "$(RMSDEPLOYDIR)\ExternalInterface.asmx" \
    "$(RMSDEPLOYDIR)\icon\IMI Warehouse.ico" \
    "$(RMSDEPLOYHTMLIN)\index.html" \
    "$(RMSDEPLOYHTMLOUT)\index.html"

WMSIRENBIN= \
    "$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(WMSIRENDEPLOYBINDIR)\Wms.WebServices.iRenInterface.dll"

WMSIRENCONFIG= \
    "$(WMSIRENDEPLOYDIR)\Web.Config" \
    "$(WMSIRENDEPLOYDIR)\ExternalInterface.asmx" \
    "$(WMSIRENDEPLOYDIR)\icon\IMI Warehouse.ico" \
    "$(WMSIRENDEPLOYHTMLIN)\index.html" \
    "$(WMSIRENDEPLOYHTMLOUT)\index.html"

WOTBIN= \
    "$(WOTDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(WOTDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(WOTDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(WOTDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(WOTDEPLOYBINDIR)\Wms.WebServices.OutboundTester.dll"

WOTCONFIG= \
    "$(WOTDEPLOYDIR)\Web.Config" \
    "$(WOTDEPLOYDIR)\ExternalInterface.asmx" \
    "$(WOTDEPLOYDIR)\icon\IMI Warehouse.ico"

ROTBIN= \
    "$(ROTDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(ROTDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(ROTDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(ROTDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(ROTDEPLOYBINDIR)\Wms.WebServices.OutboundTesterRMS.dll"

ROTCONFIG= \
    "$(ROTDEPLOYDIR)\Web.Config" \
    "$(ROTDEPLOYDIR)\ExternalInterface.asmx" \
    "$(ROTDEPLOYDIR)\icon\IMI Warehouse.ico"

WOTIRENBIN= \
    "$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(WOTIRENDEPLOYBINDIR)\Wms.WebServices.OutboundTesteriRen.dll"

WOTIRENCONFIG= \
    "$(WOTIRENDEPLOYDIR)\Web.Config" \
    "$(WOTIRENDEPLOYDIR)\ExternalInterface.asmx" \
    "$(WOTIRENDEPLOYDIR)\icon\IMI Warehouse.ico"

C3PLBIN= \
    "$(C3PLDEPLOYBINDIR)\Imi.Framework.Job.dll" \
    "$(C3PLDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll" \
    "$(C3PLDEPLOYBINDIR)\Imi.Framework.Shared.dll" \
	"$(C3PLDEPLOYBINDIR)\Imi.Framework.Versioning.dll" \
    "$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Framework.dll" \
    "$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.Customer.dll" \
    "$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.Order.dll" \
    "$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch.dll" \
    "$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.Warehouse.dll" \
    "$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.OrderPortal.dll"

C3PLCONFIG= \
    "$(C3PLDEPLOYDIR)\Web.Config" \
    "$(C3PLDEPLOYDIR)\Interface.c3PL.Customer.asmx" \
    "$(C3PLDEPLOYDIR)\Interface.c3PL.Order.asmx" \
    "$(C3PLDEPLOYDIR)\Interface.c3PL.ProductSearch.asmx" \
    "$(C3PLDEPLOYDIR)\Interface.c3PL.Warehouse.asmx" \
    "$(C3PLDEPLOYDIR)\Interface.OrderPortal.asmx"

ALLDIR= \
    "$(WMSDEPLOYHTMLIN)" \
    "$(WMSDEPLOYHTMLOUT)" \
    "$(WMSDEPLOYDIR)\icon" \
    "$(WMSDEPLOYBINDIR)" \
    "$(RMSDEPLOYHTMLIN)" \
    "$(RMSDEPLOYHTMLOUT)" \
    "$(RMSDEPLOYDIR)\icon" \
    "$(RMSDEPLOYBINDIR)" \
    "$(WMSIRENDEPLOYHTMLIN)" \
    "$(WMSIRENDEPLOYHTMLOUT)" \
    "$(WMSIRENDEPLOYDIR)\icon" \
    "$(WMSIRENDEPLOYBINDIR)" \
    "$(WOTDEPLOYDIR)\icon" \
    "$(WOTDEPLOYBINDIR)" \
    "$(ROTDEPLOYDIR)\icon" \
    "$(ROTDEPLOYBINDIR)" \
    "$(WOTIRENDEPLOYDIR)\icon" \
    "$(WOTIRENDEPLOYBINDIR)" \
    "$(C3PLDEPLOYDIR)" \
    "$(C3PLDEPLOYBINDIR)"

# --------------------------------------------
# Description Blocks
# --------------------------------------------

ALL: $(ALLDIR) $(WMSCONFIG) $(WMSBIN) $(RMSCONFIG) $(RMSBIN) $(WMSIRENCONFIG) $(WMSIRENBIN) $(WOTCONFIG) $(WOTBIN) $(ROTCONFIG) $(ROTBIN) $(WOTIRENCONFIG) $(WOTIRENBIN) $(C3PLCONFIG) $(C3PLBIN) 

CLEAN: 
	$(RD) $(DEPLOYDIR)

# --------------------------------------------
# Directories
# --------------------------------------------

$(WMSDEPLOYDIR)\icon:
	if NOT EXIST $@ $(MD) $@

$(WMSDEPLOYDIR)\bin:
	if NOT EXIST $@ $(MD) $@

$(WMSDEPLOYDIR)\doc\recipient:
	if NOT EXIST $@ $(MD) $@

$(WMSDEPLOYDIR)\doc\sender:
	if NOT EXIST $@ $(MD) $@

$(WMSIRENDEPLOYDIR)\icon:
	if NOT EXIST $@ $(MD) $@

$(WMSIRENDEPLOYDIR)\bin:
	if NOT EXIST $@ $(MD) $@

$(WMSIRENDEPLOYDIR)\doc\recipient:
	if NOT EXIST $@ $(MD) $@

$(WMSIRENDEPLOYDIR)\doc\sender:
	if NOT EXIST $@ $(MD) $@

$(RMSDEPLOYDIR)\icon:
	if NOT EXIST $@ $(MD) $@

$(RMSDEPLOYDIR)\bin:
	if NOT EXIST $@ $(MD) $@

$(RMSDEPLOYDIR)\doc\recipient:
	if NOT EXIST $@ $(MD) $@

$(RMSDEPLOYDIR)\doc\sender:
	if NOT EXIST $@ $(MD) $@

$(WOTDEPLOYDIR)\icon:
	if NOT EXIST $@ $(MD) $@

$(WOTDEPLOYDIR)\bin:
	if NOT EXIST $@ $(MD) $@

$(ROTDEPLOYDIR)\icon:
	if NOT EXIST $@ $(MD) $@

$(ROTDEPLOYDIR)\bin:
	if NOT EXIST $@ $(MD) $@

$(WOTIRENDEPLOYDIR)\icon:
	if NOT EXIST $@ $(MD) $@

$(WOTIRENDEPLOYDIR)\bin:
	if NOT EXIST $@ $(MD) $@

$(C3PLDEPLOYDIR):
	if NOT EXIST $@ $(MD) $@

$(C3PLDEPLOYBINDIR):
	if NOT EXIST $@ $(MD) $@
	
# --------------------------------------------
# Config files
# --------------------------------------------

$(WMSDEPLOYDIR)\Web.config: $(WMSSITEROOT)\Web.Config
	$(CP) $? $@

$(WMSDEPLOYDIR)\ExternalInterface.asmx: $(WMSSITEROOT)\ExternalInterface.asmx
	$(CP) $? $@

"$(WMSDEPLOYDIR)\icon\IMI Warehouse.ico": "$(ROOT)\..\graphics\icon\IMI Warehouse.ico"
	$(CP) $? $@

$(RMSDEPLOYDIR)\Web.config: $(RMSSITEROOT)\Web.Config
	$(CP) $? $@

$(RMSDEPLOYDIR)\ExternalInterface.asmx: $(RMSSITEROOT)\ExternalInterface.asmx
	$(CP) $? $@

"$(RMSDEPLOYDIR)\icon\IMI Warehouse.ico": "$(ROOT)\..\graphics\icon\IMI Warehouse.ico"
	$(CP) $? $@

$(WMSIRENDEPLOYDIR)\Web.config: $(WMSSITEIRENROOT)\Web.Config
	$(CP) $? $@

$(WMSIRENDEPLOYDIR)\ExternalInterface.asmx: $(WMSSITEIRENROOT)\ExternalInterface.asmx
	$(CP) $? $@

"$(WMSIRENDEPLOYDIR)\icon\IMI Warehouse.ico": "$(ROOT)\..\graphics\icon\IMI Warehouse.ico"
	$(CP) $? $@

$(WOTDEPLOYDIR)\Web.config: $(WOTSITEROOT)\Web.Config
	$(CP) $? $@

$(WOTDEPLOYDIR)\ExternalInterface.asmx: $(WOTSITEROOT)\ExternalInterface.asmx
	$(CP) $? $@

"$(WOTDEPLOYDIR)\icon\IMI Warehouse.ico": "$(ROOT)\..\graphics\icon\IMI Warehouse.ico"
	$(CP) $? $@

$(ROTDEPLOYDIR)\Web.config: $(ROTSITEROOT)\Web.Config
	$(CP) $? $@

$(ROTDEPLOYDIR)\ExternalInterface.asmx: $(ROTSITEROOT)\ExternalInterface.asmx
	$(CP) $? $@

"$(ROTDEPLOYDIR)\icon\IMI Warehouse.ico": "$(ROOT)\..\graphics\icon\IMI Warehouse.ico"
	$(CP) $? $@

$(WOTIRENDEPLOYDIR)\Web.config: $(WOTSITEIRENROOT)\Web.Config
	$(CP) $? $@

$(WOTIRENDEPLOYDIR)\ExternalInterface.asmx: $(WOTSITEIRENROOT)\ExternalInterface.asmx
	$(CP) $? $@

"$(WOTIRENDEPLOYDIR)\icon\IMI Warehouse.ico": "$(ROOT)\..\graphics\icon\IMI Warehouse.ico"
	$(CP) $? $@

$(C3PLDEPLOYDIR)\Web.config: $(C3PLSITEROOT)\Web.Config
	$(CP) $? $@

$(C3PLDEPLOYDIR)\Interface.c3PL.Customer.asmx: $(C3PLSITEROOT)\Interface.c3PL.Customer.asmx
	$(CP) $? $@

$(C3PLDEPLOYDIR)\Interface.c3PL.Order.asmx: $(C3PLSITEROOT)\Interface.c3PL.Order.asmx
	$(CP) $? $@

$(C3PLDEPLOYDIR)\Interface.c3PL.ProductSearch.asmx: $(C3PLSITEROOT)\Interface.c3PL.ProductSearch.asmx
	$(CP) $? $@

$(C3PLDEPLOYDIR)\Interface.c3PL.Warehouse.asmx: $(C3PLSITEROOT)\Interface.c3PL.Warehouse.asmx
	$(CP) $? $@

$(C3PLDEPLOYDIR)\Interface.OrderPortal.asmx: $(C3PLSITEROOT)\Interface.OrderPortal.asmx
	$(CP) $? $@	
	
# --------------------------------------------
# Binaries
# --------------------------------------------

$(WMSDEPLOYBINDIR)\Imi.Framework.Job.dll: $(WMSSITEBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(WMSDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(WMSSITEBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(WMSDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(WMSSITEBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@

$(WMSDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(WMSSITEBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(WMSDEPLOYBINDIR)\Wms.WebServices.ExternalInterface.dll: $(WMSSITEBINROOT)\Wms.WebServices.ExternalInterface.dll
	$(CP) $? $@

$(RMSDEPLOYBINDIR)\Imi.Framework.Job.dll: $(RMSSITEBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(RMSDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(RMSSITEBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(RMSDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(RMSSITEBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@
	
$(RMSDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(RMSSITEBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(RMSDEPLOYBINDIR)\Wms.WebServices.ExternalInterfaceRMS.dll: $(RMSSITEBINROOT)\Wms.WebServices.ExternalInterfaceRMS.dll
	$(CP) $? $@

$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Job.dll: $(WMSSITEIRENBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(WMSSITEIRENBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(WMSSITEIRENBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@
	
$(WMSIRENDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(WMSSITEIRENBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(WMSIRENDEPLOYBINDIR)\Wms.WebServices.iRenInterface.dll: $(WMSSITEIRENBINROOT)\Wms.WebServices.iRenInterface.dll
	$(CP) $? $@

$(WOTDEPLOYBINDIR)\Imi.Framework.Job.dll: $(WOTSITEBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(WOTDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(WOTSITEBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(WOTDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(WOTSITEBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@
	
$(WOTDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(WOTSITEBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(WOTDEPLOYBINDIR)\Wms.WebServices.OutboundTester.dll: $(WOTSITEBINROOT)\Wms.WebServices.OutboundTester.dll
	$(CP) $? $@

$(ROTDEPLOYBINDIR)\Imi.Framework.Job.dll: $(ROTSITEBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(ROTDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(ROTSITEBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(ROTDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(ROTSITEBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@
	
$(ROTDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(ROTSITEBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(ROTDEPLOYBINDIR)\Wms.WebServices.OutboundTesterRMS.dll: $(ROTSITEBINROOT)\Wms.WebServices.OutboundTesterRMS.dll
	$(CP) $? $@

$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Job.dll: $(WOTSITEIRENBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(WOTSITEIRENBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(WOTSITEIRENBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@

$(WOTIRENDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(WOTSITEIRENBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(WOTIRENDEPLOYBINDIR)\Wms.WebServices.OutboundTesteriRen.dll: $(WOTSITEIRENBINROOT)\Wms.WebServices.OutboundTesteriRen.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Imi.Framework.Job.dll: $(C3PLSITEBINROOT)\Imi.Framework.Job.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Imi.Framework.Job.RemoteInterface.dll: $(C3PLSITEBINROOT)\Imi.Framework.Job.RemoteInterface.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Imi.Framework.Shared.dll: $(C3PLSITEBINROOT)\Imi.Framework.Shared.dll
	$(CP) $? $@
	
$(C3PLDEPLOYBINDIR)\Imi.Framework.Versioning.dll: $(C3PLSITEBINROOT)\Imi.Framework.Versioning.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Framework.dll: $(C3PLSITEBINROOT)\Wms.WebServices.SyncWS.Framework.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.Customer.dll: $(C3PLSITEBINROOT)\Wms.WebServices.SyncWS.Interface.c3PL.Customer.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.Order.dll: $(C3PLSITEBINROOT)\Wms.WebServices.SyncWS.Interface.c3PL.Order.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch.dll: $(C3PLSITEBINROOT)\Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.c3PL.Warehouse.dll: $(C3PLSITEBINROOT)\Wms.WebServices.SyncWS.Interface.c3PL.Warehouse.dll
	$(CP) $? $@

$(C3PLDEPLOYBINDIR)\Wms.WebServices.SyncWS.Interface.OrderPortal.dll: $(C3PLSITEBINROOT)\Wms.WebServices.SyncWS.Interface.OrderPortal.dll
	$(CP) $? $@
	
# --------------------------------------------
# HTML
# --------------------------------------------

$(WMSDEPLOYHTMLIN)\index.html: $(WMSSITEHTMLIN)\index.html
	$(CP) $(WMSSITEHTMLIN)\*.* $(WMSDEPLOYHTMLIN)\.

$(WMSDEPLOYHTMLOUT)\index.html: $(WMSSITEHTMLOUT)\index.html
	$(CP) $(WMSSITEHTMLOUT)\*.* $(WMSDEPLOYHTMLOUT)\.

$(RMSDEPLOYHTMLIN)\index.html: $(RMSSITEHTMLIN)\index.html
	$(CP) $(RMSSITEHTMLIN)\*.* $(RMSDEPLOYHTMLIN)\.

$(RMSDEPLOYHTMLOUT)\index.html: $(RMSSITEHTMLOUT)\index.html
	$(CP) $(RMSSITEHTMLOUT)\*.* $(RMSDEPLOYHTMLOUT)\.

$(WMSIRENDEPLOYHTMLIN)\index.html: $(WMSSITEIRENHTMLIN)\index.html
	$(CP) $(WMSSITEIRENHTMLIN)\*.* $(WMSIRENDEPLOYHTMLIN)\.

$(WMSIRENDEPLOYHTMLOUT)\index.html: $(WMSSITEIRENHTMLOUT)\index.html
	$(CP) $(WMSSITEIRENHTMLOUT)\*.* $(WMSIRENDEPLOYHTMLOUT)\.

# end makefile

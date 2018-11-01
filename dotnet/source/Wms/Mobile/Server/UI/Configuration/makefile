#
# makefile 17/12/2004
# 
# Copyright (c) 2004 - 2005 Industri-Matematik AB. All rights reserved.
#
CSDIR=.\auto
XSDDIR=.\xsd
RD=rd /s /q

ALLCS= \
    "$(CSDIR)\ServerConfig.cs" 


# --------------------------------------------
# Description Blocks
# --------------------------------------------

ALL: $(ALLCS)

$(CSDIR)\ServerConfig.cs: $(XSDDIR)\ServerConfig.xsd
	xsd $(XSDDIR)\ServerConfig.xsd /classes /namespace:Imi.Wms.Mobile.Server.UI.Configuration
	move ServerConfig.cs $(CSDIR)\ServerConfig.cs

# end makefile
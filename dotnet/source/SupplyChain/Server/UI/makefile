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
	xsd $(XSDDIR)\ServerConfig.xsd /classes /namespace:Warehouse.Config.Client
	move ServerConfig.cs $(CSDIR)\ServerConfig.cs

# end makefile
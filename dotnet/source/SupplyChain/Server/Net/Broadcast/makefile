#
# makefile 17/12/2004
# 
# Copyright (c) 2004 - 2005 Industri-Matematik AB. All rights reserved.
#
CSDIR=.\auto
XSDDIR=.\xsd
RD=rd /s /q

ALLCS= \
    "$(CSDIR)\BroadcastSetup.cs" 


# --------------------------------------------
# Description Blocks
# --------------------------------------------

ALL: $(ALLCS)

$(CSDIR)\BroadcastSetup.cs: $(XSDDIR)\BroadcastSetup.xsd makefile
	xsd $(XSDDIR)\BroadcastSetup.xsd /classes /namespace:Imi.SupplyChain.Server.Net.Broadcast
	move BroadcastSetup.cs $(CSDIR)\BroadcastSetup.cs

# end makefile
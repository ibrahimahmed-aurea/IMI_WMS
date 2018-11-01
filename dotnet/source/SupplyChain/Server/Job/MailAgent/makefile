#
# makefile 17/12/2004
# 
# Copyright (c) 2004 - 2005 Industri-Matematik AB. All rights reserved.
#
CSDIR=C:\project\views\togr_temp_ss\client\C^#\Server\MailAgent\auto
XSDDIR=.\xsd
RD=rd /s /q

ALLCS= \
	"$(CSDIR)\EmailMessage.cs" 


# --------------------------------------------
# Description Blocks
# --------------------------------------------

ALL: $(ALLCS)

$(CSDIR)\EmailMessage.cs: $(XSDDIR)\EmailMessage.xsd makefile
	xsd $(XSDDIR)\EmailMessage.xsd /classes /namespace:Imi.SupplyChain.Server.Job.MailAgent.Config
	move EmailMessage.cs $(CSDIR)\EmailMessage.cs

# end makefile
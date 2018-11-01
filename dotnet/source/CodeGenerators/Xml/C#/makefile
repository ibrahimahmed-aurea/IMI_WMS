#
# makefile 17/12/2004
# 
# Copyright (c) 2004 - 2005 Industri-Matematik AB. All rights reserved.
#

XSDDIR=..\xsd
CSDIR=..\..\C^#\ClassLib
XSD=xsd
RD=rd /s /q
NAMESPACE=Warehouse.SchemaStructures
MAKEFILE=..\C^#\makefile

ALLDIR= \
	"$(CSDIR)" 
    	
ALLCS= \
    "$(CSDIR)\StructureDescription.cs" \
    "$(CSDIR)\WebServiceConfig.cs"

# --------------------------------------------
# Description Blocks
# --------------------------------------------

ALL : $(ALLCS)

$(CSDIR)\Package.cs : $(XSDDIR)\Package.xsd $(MAKEFILE)

$(CSDIR)\StructureDescription.cs : $(XSDDIR)\StructureDescription.xsd $(XSDDIR)\Package.xsd $(MAKEFILE)

$(CSDIR)\MetaDataDefinition.cs : $(XSDDIR)\MetaDataDefinition.xsd $(MAKEFILE)

$(CSDIR)\WebServiceConfig.cs : $(XSDDIR)\WebServiceConfig.xsd $(MAKEFILE)

CLEAN :
        @nmake /nologo ALL

# --------------------------------------------
# Inference Rules
# --------------------------------------------

{$(XSDDIR)}.xsd{$(CSDIR)}.cs :
    cd "$(XSDDIR)"
    $(XSD) $< /classes /namespace:$(NAMESPACE) /out:"$(CSDIR)"


.SUFFIXES : .xsd .cs

# end makefile

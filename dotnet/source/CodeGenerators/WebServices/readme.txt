==================================
To setup the generator environment
==================================
run envsetup.cmd


============================
To build the generator tools
============================
open solution WebServiceTools.sln
rebuild solution
run copy_to_wscc.cmd


=========================================
To generate incoming webservice auto-code
=========================================
Start a visual studio cmd-prompt
cd c:\temp\wscc
nmake inmakefile


==============================================
To generate outgoing webservice-call auto-code
==============================================
Start a visual studio cmd-prompt
cd c:\temp\wscc
nmake outmakefile


=======================================
To build a PL/SQL-package wrapper in C#
=======================================
PackageGenerator.exe <packagename> <filename>


===============================
To build the inbound webservice
===============================
Generatate the auto-code (above)
Copy all files from c:\temp\wscc\genin\src to Inbound\auto
Create a virtual directory in your web server pointing to Inbound. Read, Write and Browse access needed.
Create a new solution and add existing projects ClassLib and WebServiceConfiguration.
Add existing Web-Project Inbound from the virutal directory.
Rebuild all.


================================
To deploy the inbound webservice
================================
Create a directory on the web server. Copy these files to the web server.
ExternalInterface.asmx
Global.asax
Web.config
bin\ClassLib.dll
bin\Inbound.dll
bin\WebServiceConfiguration.dll

Modify Web.config to have proper external_id mapped to connect strings.
Create an application in the IIS in this directory.


===========================
To build web service sender
===========================
Generatate the auto-code (above)
Copy all files from c:\temp\wscc\genout\src to WebServiceSnd\auto
Open the solution WebServiceSnd.
Rebuild all.


=========================================================
To build the tester webservice for the web service sender
=========================================================
Generatate the auto-code (above)
Copy *.cs files from c:\temp\wscc\genin\testsrc to Inbound\auto
Create a virtual directory in your web server pointing to WSOutboundTester. Read, Write and Browse access needed.
Create a new solution and add existing projects ClassLib and WebServiceConfiguration.
Add existing Web-Project WSOutboundTester from the virutal directory.
Rebuild all.


===============================
To deploy the tester webservice
===============================
Create a directory on the web server. Copy these files to the web server.
ExternalInterface.asmx
Global.asax
Web.config
bin\ClassLib.dll
bin\Inbound.dll
bin\OutboundTester.dll

Modify Web.config to have proper external_id mapped to connect strings.
Create an application in the IIS in this directory.
Create tables in C:\TEMP\wscc\genout\testsrc\create_test_tables.sql.


==========================================================
How to update Schema files and generate related .cs files.
==========================================================

Check out StructureDescription.cs in the C# \ ClassLib directory
Check Out StructureDescription.xsd in the xml\xsd directory

Edit the xsd file and save the changes.

Start a VS command prompt and cd to the xml \ c# directory.
Type nmake

If no generation takes place it's usually because of Clearcase, 
to force a regeneration Save or touch the makefile since all builds 
are depending on the makefie. (This will regenerate WebServiceConfig
As well so you might need to check it out first)


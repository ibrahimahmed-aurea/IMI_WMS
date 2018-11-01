Förberedelser
=============

Öppna IIS Admin
Stoppa websiten
Öppna services
Stoppa World Wide Web publishing service
Döp om gamla client och server-katalogerna i vyn
Öppna view-properties
Lägg till rad i config spec (andra raden):
element * <checkpoint_label>
vyn skall ha load rules:
  client\c#
  client\xml
  client\delphi\images\imi warehouse.ico
  server\oracle
  server\distrib
  server\route\oracle
  server\route\distrib
synka vyn
Öppna services
starta World Wide Web publishing service
Öppna IIS Admin
starta websiten
Kör generators\envsetup.cmd

Skapa verktyg
=============

Öppna Visual Studio
Öppna WebServiceTools-solution
Bygg solution (Debug)
Kör generators\copy_to_wscc.cmd

Generera
========

starta VS-cmd-promt och cd till client\C#\wscc
redigera inmakefile och outmakefile till att ansluta till rätt användare i oracle (default är owuser, det skall eventuellt vara websuser istället)
kör nmake inmakefile
kör nmake outmakefile
kör nmake hapi_makefile
obs att ordningen på makefilerna är viktig, outmakefile använder inmakefile som uppslagsverk för namn
kör generators\postgen.cmd
öppna clearcase explorer och gör "find modified files"
checka ut alla hijacked files, lämpligt att sätta kommentar till checkpointens nummer

Förbered att bygga ExternalInterface
====================================
Detta måste bara göras en gång på din dator eller när du bytt namn på vyn

Create a virtual directory in your web server pointing to Inbound. Read, Write and Browse access needed.
Create a new solution and add existing projects ClassLib and WebServiceConfiguration.
Add existing Web-Project Inbound from the virutal directory.
Spara ändringar.
Synka upp de hijackade csproj-filerna igen, annars försvinner kopplingen mellan delprojekten.

Create a virtual directory in your web server pointing to WSOutboundTester. Read, Write and Browse access needed.
Create a new solution and add existing projects ClassLib and WebServiceConfiguration.
Add existing Web-Project WSOutboundTester from the virutal directory.
Spara ändringar.
Synka upp de hijackade csproj-filerna igen, annars försvinner kopplingen mellan delprojekten.


Bygg ExternalInterface
======================

Det går att göra steget "checka in" innan man gör detta.
Öppna din inbound-solution (skapad i steget ovan)
ställ in att bygg release-version
rebuild solution
Öppna din wsoutboundtester-solution (skapad i steget ovan)
ställ in att bygg release-version
rebuild solution
öppna server\server.sln
ställ in att bygg release-version
rebuild solution

OBS! du kan inte bygga något annat mellan servern och att du skapar
installationsfilerna nedan eftersom scripten för server kopierar
classlib.dll från classlib-katalogen istället för från serverns egen
katalog. det blir fel version om du bygger något annat mellan.
När steget (*) inkl. nmake nedan är gjort är det fritt att bygga igen.

Bygg installationsfiler
=======================

Öppna VS-cmd-promt i C#\WebService\Install\Inbound
kör nmake

flytta till Install\WSOutboundTester
kör nmake

(*) flytta till C#\Server\Install
kör setq.bat
kör nmake

öppna IMIWebService.ism
bygg single-exe-image

öppna IMIWebServiceOutboundTester.ism
bygg single-exe-image

öppna IMIServer.ism
bygg uncompessed

kopiera ut från
WebService\Install\Inbound\IMIWebService\Media\SINGLE_EXE_IMAGE\Package
WebService\Install\WSOutboundTester\IMIWebServiceOutboundTester\Media\SINGLE_EXE_IMAGE\Package
och
Server\Install\IMIServer\Product Configuration\UNCOMPRESSED\DiskImages
till release-area

Provinstallera
==============

Öppna kontrollpanelen - add/remove programs
ta bort IMI Server 5.0
ta bort IMIWebService
ta bort IMIWebServiceOutboundTester

installera de tre komponenterna från release-arean.

Checka in
=========

Öppna clearcase explorer och gör "find checkouts" i client\c# OCH (OBS!) i server\oracle
checka in alla filer som är utcheckade. obs! kryssa i check in even if identical to precedessor
Öppna view-properties
Ta bort extra raden i config spec (andra raden):
element * <checkpoint_label>
synka vyn
kör generators\movelabel.cmd <label> från en cmd-promt

Generera versionsinfo
=====================

Detta kan bara göras när man har byggt rätt version av interfacet.
Checka ut alla EI*-filer i WebService\ChangeLog\ExternalInterface
Kör getei.cmd från en cmd-prompt
kör sortei.cmd
Checka in alla EI*-filer. kryssa i check in even if identical to precedessor. Ge kommentar = labeln.

Verifiera/regressionstesta releasen
===================================

Gör compare with previous version på EI-filerna i WebService\ChangeLog\ExternalInterface
kontrollera WebService\WebServiceSnd\auto\SelectHandler.cs:
Klassen GeneratedComparer i slutet på filen har ett antal metoder. Dessa skall inte vara tomma. Jämför gärna med tidigare version.

klart!

F�rberedelser
=============

�ppna IIS Admin
Stoppa websiten
�ppna services
Stoppa World Wide Web publishing service
D�p om gamla client och server-katalogerna i vyn
�ppna view-properties
L�gg till rad i config spec (andra raden):
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
�ppna services
starta World Wide Web publishing service
�ppna IIS Admin
starta websiten
K�r generators\envsetup.cmd

Skapa verktyg
=============

�ppna Visual Studio
�ppna WebServiceTools-solution
Bygg solution (Debug)
K�r generators\copy_to_wscc.cmd

Generera
========

starta VS-cmd-promt och cd till client\C#\wscc
redigera inmakefile och outmakefile till att ansluta till r�tt anv�ndare i oracle (default �r owuser, det skall eventuellt vara websuser ist�llet)
k�r nmake inmakefile
k�r nmake outmakefile
k�r nmake hapi_makefile
obs att ordningen p� makefilerna �r viktig, outmakefile anv�nder inmakefile som uppslagsverk f�r namn
k�r generators\postgen.cmd
�ppna clearcase explorer och g�r "find modified files"
checka ut alla hijacked files, l�mpligt att s�tta kommentar till checkpointens nummer

F�rbered att bygga ExternalInterface
====================================
Detta m�ste bara g�ras en g�ng p� din dator eller n�r du bytt namn p� vyn

Create a virtual directory in your web server pointing to Inbound. Read, Write and Browse access needed.
Create a new solution and add existing projects ClassLib and WebServiceConfiguration.
Add existing Web-Project Inbound from the virutal directory.
Spara �ndringar.
Synka upp de hijackade csproj-filerna igen, annars f�rsvinner kopplingen mellan delprojekten.

Create a virtual directory in your web server pointing to WSOutboundTester. Read, Write and Browse access needed.
Create a new solution and add existing projects ClassLib and WebServiceConfiguration.
Add existing Web-Project WSOutboundTester from the virutal directory.
Spara �ndringar.
Synka upp de hijackade csproj-filerna igen, annars f�rsvinner kopplingen mellan delprojekten.


Bygg ExternalInterface
======================

Det g�r att g�ra steget "checka in" innan man g�r detta.
�ppna din inbound-solution (skapad i steget ovan)
st�ll in att bygg release-version
rebuild solution
�ppna din wsoutboundtester-solution (skapad i steget ovan)
st�ll in att bygg release-version
rebuild solution
�ppna server\server.sln
st�ll in att bygg release-version
rebuild solution

OBS! du kan inte bygga n�got annat mellan servern och att du skapar
installationsfilerna nedan eftersom scripten f�r server kopierar
classlib.dll fr�n classlib-katalogen ist�llet f�r fr�n serverns egen
katalog. det blir fel version om du bygger n�got annat mellan.
N�r steget (*) inkl. nmake nedan �r gjort �r det fritt att bygga igen.

Bygg installationsfiler
=======================

�ppna VS-cmd-promt i C#\WebService\Install\Inbound
k�r nmake

flytta till Install\WSOutboundTester
k�r nmake

(*) flytta till C#\Server\Install
k�r setq.bat
k�r nmake

�ppna IMIWebService.ism
bygg single-exe-image

�ppna IMIWebServiceOutboundTester.ism
bygg single-exe-image

�ppna IMIServer.ism
bygg uncompessed

kopiera ut fr�n
WebService\Install\Inbound\IMIWebService\Media\SINGLE_EXE_IMAGE\Package
WebService\Install\WSOutboundTester\IMIWebServiceOutboundTester\Media\SINGLE_EXE_IMAGE\Package
och
Server\Install\IMIServer\Product Configuration\UNCOMPRESSED\DiskImages
till release-area

Provinstallera
==============

�ppna kontrollpanelen - add/remove programs
ta bort IMI Server 5.0
ta bort IMIWebService
ta bort IMIWebServiceOutboundTester

installera de tre komponenterna fr�n release-arean.

Checka in
=========

�ppna clearcase explorer och g�r "find checkouts" i client\c# OCH (OBS!) i server\oracle
checka in alla filer som �r utcheckade. obs! kryssa i check in even if identical to precedessor
�ppna view-properties
Ta bort extra raden i config spec (andra raden):
element * <checkpoint_label>
synka vyn
k�r generators\movelabel.cmd <label> fr�n en cmd-promt

Generera versionsinfo
=====================

Detta kan bara g�ras n�r man har byggt r�tt version av interfacet.
Checka ut alla EI*-filer i WebService\ChangeLog\ExternalInterface
K�r getei.cmd fr�n en cmd-prompt
k�r sortei.cmd
Checka in alla EI*-filer. kryssa i check in even if identical to precedessor. Ge kommentar = labeln.

Verifiera/regressionstesta releasen
===================================

G�r compare with previous version p� EI-filerna i WebService\ChangeLog\ExternalInterface
kontrollera WebService\WebServiceSnd\auto\SelectHandler.cs:
Klassen GeneratedComparer i slutet p� filen har ett antal metoder. Dessa skall inte vara tomma. J�mf�r g�rna med tidigare version.

klart!

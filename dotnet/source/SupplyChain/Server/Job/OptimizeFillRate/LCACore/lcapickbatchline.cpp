#include "lcapickbatchline.h"

// Constructs a LCAPickBatchLine
LCAPickBatchLine::LCAPickBatchLine(	const QString sPBRowId, const int pickSeq, const QString sArtId, const QString sCompanyId, 
	const double ordQty, const double volume, const double weight, const QString sWSId, const QString sAisle,
	const QString sArtGroup, QString sCatGroup, const double xCord, const double yCord, const QString sWPAdr )
{
	msPBRowId = sPBRowId;
	mPickSeq = pickSeq;
	msArtId = sArtId;
	msCompanyId = sCompanyId;
	mOrdQty = ordQty;
	mVolume = volume;
	mWeight = weight;
	msWSId = sWSId;
	msAisle = sAisle;
	msArtGroup = sArtGroup;
	msCatGroup = sCatGroup;
	mXCord = xCord;
	mYCord = yCord;
	msWPAdr = sWPAdr;
}
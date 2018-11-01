#include "lcaresultline.h"

LCAResultLine::LCAResultLine( const QString sPBRowId, const QString sPBRowSplitId, const double ordQty,
	const QString sPBCarIdVirtual, const int pickSeq, const QString sCatGroup, const QString sWsId,
	const QString sAisle, const double xCoord, const double yCoord, const QString sWPAdr )
{
	msPBRowId = sPBRowId;
	msPBRowSplitId = sPBRowSplitId;
	mOrdQty = ordQty;
	msPBCarIdVirtual = sPBCarIdVirtual;
	mPickSeq = pickSeq;
	msCatGroup = sCatGroup;
	msWsId = sWsId;
	msAisle = sAisle;
	mXCoord = xCoord;
	mYCoord = yCoord;
	msWPAdr = sWPAdr;
}
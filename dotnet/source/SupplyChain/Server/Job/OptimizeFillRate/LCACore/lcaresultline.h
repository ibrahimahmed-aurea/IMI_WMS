#pragma once

#ifndef LCARESULTLINE_H
#define LCARESULTLINE_H

#include <QtCore/qstring.h>

class LCAResultLine
{
	public:

		LCAResultLine(	const QString sPBRowId,
						const QString sPBRowSplitId,
						const double  ordQty,
						const QString sPBCarIdVirtual,
						const int pickSeq,
						const QString sCatGroup,
						const QString sWsId,
						const QString sAisle,
						const double xCoord,
						const double yCoord,
						const QString sWPAdr);

		QString getPBCarIdVirtual() const	{ return msPBCarIdVirtual; }
		int		getPickSeq() const			{ return mPickSeq; }
		QString getPBRowId() const			{ return msPBRowId; }
		double  getOrdQty()	const			{ return mOrdQty; }
		QString	getPBRowSplitId() const		{ return msPBRowSplitId; }
		QString getCatGroup() const			{ return msCatGroup; }
		QString getWsId() const				{ return msWsId; }
		QString getAisle() const			{ return msAisle; }
		double	getXCoord() const			{ return mXCoord; }
		double  getYCoord() const			{ return mYCoord; }
		QString getWPAdr() const			{ return msWPAdr; }

	protected:

		QString	msPBRowId;			// Unique id of the pick batch line within a pick batch
		QString msPBRowSplitId;		// Unique id for the split of the pick batch line. Empty is not splitted
		double	mOrdQty;			// Number of the pick packages in this result
		QString	msPBCarIdVirtual;	// Unique id for the virtual load carrier 
		int     mPickSeq;			// The original pick sequence
		QString msCatGroup;			// The Categorygroup of the pick line
		QString msWsId;				// The area for the pick location
		QString msAisle;			// The aisle for the pick location
		double  mXCoord;				// The x coordinate of the picklocation, NOT corrected for offset of the area
		double  mYCoord;				// The y coordinate of the picklocation, NOT corrected for offset of the area
		QString msWPAdr;				// The WPAdress of the result line
};


// This compare operator sorts the elements in decending order of weight to volume ratio
class LCAResultLineCompare
{
	public:

		bool operator()(const LCAResultLine * const pLCA1, const LCAResultLine * const pLCA2 ) const
		{
			if ( pLCA1->getPBCarIdVirtual() != pLCA2->getPBCarIdVirtual() ) return pLCA1->getPBCarIdVirtual() < pLCA2->getPBCarIdVirtual();
			if ( pLCA1->getPickSeq() != pLCA2->getPickSeq() ) return pLCA1->getPickSeq() < pLCA2->getPickSeq();
			if ( pLCA1->getWsId() != pLCA2->getWsId() ) return pLCA1->getWsId() < pLCA2->getWsId();
			if ( pLCA1->getWPAdr() != pLCA2->getWPAdr() ) return pLCA1->getWPAdr() < pLCA2->getWPAdr();

			return pLCA1->getPBRowId() < pLCA2->getPBRowId();
		}
};

#endif
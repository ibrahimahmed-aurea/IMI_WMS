#pragma once

#ifndef LCAANNEALPICKBATCHLINE_H
#define LCAANNEALPICKBATCHLINE_H

#include <QtCore/qstring.h>
#include "lcapickbatch.h"
#include "lcapickbatchline.h"
#include "lcawarehouse.h"
#include <set>

class LCAAnnealPickBatchLine
{
	public:

		LCAAnnealPickBatchLine( const LCAPickBatchLine* pPickBatchLine, double ordQty, int uniqueId, const LCAWarehouse& rWarehouse );

		int getPickSeq() const			{ return mpPickBatchLine->getPickSeq(); }
		QString getPBRowId() const		{ return mpPickBatchLine->getPBRowId(); }
		int getUniqueId() const			{ return mUniqueId; }
		double getTotalVolume() const	{ return mTotalVolume; }
		double getTotalWeight() const	{ return mTotalWeight; }
		QString getWsId() const			{ return mpPickBatchLine->getWsId(); }
		double getOrdQty() const		{ return mOrdQty; }
		QString getCatGroup() const     { return mpPickBatchLine->getCatGroup(); }
		QString getAisle() const		{ return mpPickBatchLine->getAisle(); }
		double getXCord() const			{ return mpPickBatchLine->getXCord(); }
		double getYCord() const			{ return mpPickBatchLine->getYCord(); }
		QString getWPAdr() const		{ return mpPickBatchLine->getWPAdr(); }

		const LCASplittedAisle* getSplittedAisle() const	{ return mpSplittedAisle; }
		double getAdjustedXCord() const	{ return mAdjustedXCoord; }
		double getAdjustedYCord() const	{ return mAdjustedYCoord; }

	protected:

		const	LCAPickBatchLine* mpPickBatchLine;
		const	LCASplittedAisle* mpSplittedAisle;

		double	mOrdQty;
		double  mTotalVolume;
		double  mTotalWeight;
		int     mUniqueId;
		double	mAdjustedXCoord;
		double	mAdjustedYCoord;

};


class LCAAnnealPickBatchLineCompare
{
	public:

		bool operator()(const LCAAnnealPickBatchLine * const pLine1, const LCAAnnealPickBatchLine * const pLine2 ) const
		{
			if ( pLine1->getPickSeq() != pLine2->getPickSeq() ) return pLine1->getPickSeq() < pLine2->getPickSeq();
			if ( pLine1->getWsId() != pLine1->getWsId() ) return pLine1->getWsId() < pLine1->getWsId();
			if ( pLine1->getWPAdr() != pLine2->getWPAdr() ) return pLine1->getWPAdr() < pLine2->getWPAdr();
			if ( pLine1->getPBRowId() != pLine2->getPBRowId() ) return pLine1->getPBRowId() < pLine2->getPBRowId();
			return pLine1->getUniqueId() < pLine2->getUniqueId();
		}
};

typedef std::set< const LCAAnnealPickBatchLine *const, LCAAnnealPickBatchLineCompare > AnnealLineSet;
#endif
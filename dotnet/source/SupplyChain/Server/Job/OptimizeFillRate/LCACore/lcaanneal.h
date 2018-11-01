#pragma once
#ifndef LCAANNEAL_H
#define LCAANNEAL_H

#include "lcaanneallc.h"
#include "lcawarehouse.h"
#include "lcaannealcriterium.h"
#include "lcarandom.h"
#include <QtCore/qstringlist.h>

// Twn: dit moet er nog uit
#include <stdio.h>

class LCAResult;
class LCAParameters;

class LCAAnnealChangeItem
{
	public:

		LCAAnnealChangeItem( int loadCarrierIndex, const LCAAnnealPickBatchLine* pLine, bool isAdd );
		bool isAdd() const { return mIsAdd; }
		const LCAAnnealPickBatchLine* const getLine() const { return mpLine; }
		int getLoadCarrierIndex() const { return mLoadCarrierIndex; }

	protected:

		int mLoadCarrierIndex;
		const LCAAnnealPickBatchLine* const mpLine;
		bool mIsAdd;
};

typedef std::vector< LCAAnnealChangeItem > ChangeVector;

class LCAAnneal
{
	public:	

		LCAAnneal( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters, const LCAWarehouse& rWarehouse );
		LCAResult* run( const int lowerBoundLCs, const double apteanDistance,  LCAResult *pResult  );

		//void setAnnealFile( FILE *pFile ) { mpAnnealFile = pFile; }

	protected:

		void buildLoadCarrierVector( const LCAResult* pResult, bool forbidSplit );
		LCAResult* runWithCriterium( LCAAnnealCriterium& rCriterium, double maxRunTime );

		void doChange( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium );
		void undoChange( ChangeVector &rChanges );
		void updateValues();

		LCAResult* createSolution();

		void doChangeExchange( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium, bool doBestOption );
		void doChangeMove( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium, bool forceBest );
		void doChangeMoveMany( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium );

		std::vector< LCAAnnealLC > mcLoadCarriers;
		const LCAWarehouse& mrWarehouse;
		const LCAParameters* mpParameters;
		const LCAPickBatch *mpPickbatch;

		double mAdjustedXStrek;
		double mAdjustedYStrek;

		LCARandom random;

		QStringList msTimeOuts;

		//FILE* mpAnnealFile;
};

#endif
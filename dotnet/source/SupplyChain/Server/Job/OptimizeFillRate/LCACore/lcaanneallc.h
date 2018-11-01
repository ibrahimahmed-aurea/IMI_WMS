#pragma once
#ifndef LCAANNEALLC_H
#define LCAANNEALLC_H

#include "lcaannealpickbatchline.h"
#include "lcawarehouse.h"
#include "lcarandom.h"
#include <set>

class LCAAnnealLC
{
	public:

		LCAAnnealLC();
		~LCAAnnealLC();

		void insertLine( const LCAAnnealPickBatchLine* const pLine, const LCAWarehouse& rWarehouse, double strekX, double strekY );
		void removeLine( const LCAAnnealPickBatchLine* const pLine, const LCAWarehouse& rWarehouse, double strekX, double strekY );

		const LCAAnnealPickBatchLine* const selectRandomLine( LCARandom& rRandom );
		const LCAAnnealPickBatchLine* const selectRandomMinorityVolumeLine( LCARandom& rRandom );
		const LCAAnnealPickBatchLine* const selectRandomMinorityAisleLine( LCARandom& rRandom );
		std::vector<const LCAAnnealPickBatchLine*> const selectSmallAisle( LCARandom& rRandom );
		std::vector<const LCAAnnealPickBatchLine*> const selectLongDistanceLines( LCARandom& rRandom, const LCAWarehouse& rWarehouse, double strekX, double strekY, double maxVolume );

		double getTotalDistance() const { return mTotalDistance; }
		double getTotalWeight() const	{ return mTotalWeight; }
		double getTotalVolume() const	{ return mTotalVolume; }
		long   getPBRowCount() const	{ return mPBRows; }
		int    getNumberAisles() const  { return mNumberAisles; }

		AnnealLineSet::const_iterator beginLines() const { return mcLines.begin(); }
		AnnealLineSet::const_iterator endLines() const { return mcLines.end(); }

		bool isBeauty() const  { return mcCategoryGroupVolume.size() == 1; }
		double getUglyVolume() const;
		double getSmallestAisleVolume() { return mSmallestAisleVolume; } 
		bool containsAisle( QString sAisle ) const { return mcAisleVolume.find( sAisle ) != mcAisleVolume.end(); }

	protected:

		void updateSmallestAisleVolume();

		AnnealLineSet mcLines;

		double	mTotalVolume;
		double  mTotalWeight;
		double  mTotalDistance;
		long	mPBRows;
		int     mNumberAisles;
		double  mSmallestAisleVolume;
		QString msAisleWithSmallestVolume;
		std::map< QString, double > mcCategoryGroupVolume;
		std::map< QString, double > mcAisleVolume;

};

#endif
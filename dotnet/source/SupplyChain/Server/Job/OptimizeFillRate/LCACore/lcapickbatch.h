#pragma once

#ifndef LCAPICKBATCH_H
#define LCAPICKBATCH_H

#include <QtCore/qstring.h>
#include <vector>

class LCAPickBatchLine;
class LCASplittedAisle;

/* This class defines a pick batch, i.e., the total set of pick batch lines that together
 * must be compiled into load carriers.
 * The LCAPickBatch contains all rows from the PBROW table that must be processed together.
 * Information that is common for all rows in the PBROW table, is stored on the LCAPickBatch
 * Information that can be unique for each row, is stored on the LCAPickBatchLine
 */
class LCAPickBatch
{
	public:

		LCAPickBatch(	const QString sNewGroupId, 
						const QString sWHId, 
						const QString sPZId,
						const QString sStrekArea, 
						const double strekXCoord, 
						const double strekYCoord );

		~LCAPickBatch();

		int addPickBatchLine(	const QString sPBRowId, 
								const int pickSeq,
								const QString sArtId,
								const QString sCompanyId,
								const double ordQty,
								const double volume,
								const double weight,
								const QString sWSId,
								const QString sAisle,
								const QString sArtGroup,
								const QString sCatGroup,
								const double xCord,
								const double yCord,
								const QString sWPAdr
								);

		QString getNewGroupId() const;
		QString getWHId() const;
		int getPickLineCount() const;
		const LCAPickBatchLine* const getPickLine( int pickLineNr ) const;

		QString getStrekWsId() const  { return msStrekArea; }
		double getStrekXCoord() const { return mStrekXCoord; }
		double getStrekYCoord() const { return mStrekYCoord; }

		const double getTotalVolume() const;
		const double getTotalWeight() const;
	
	protected:

		void clear();

		QString msNewGroupId;	// Unique Id of the pick batch, i.e., the set of rows from the PBROW table that should be processed together
		QString msWHId;			// Unique Id of the warehouse
		QString msPZId;			// Unique Id of the pick zone ( within the warehouse )
		QString msStrekArea;	// Unique Id of the area in which the strek is located
		double	mStrekXCoord;	// The X coordinate of the location at which the load carrier should be dropped
		double	mStrekYCoord;	// The Y coordinate of the location at which the load carrier should be dropped

		std::vector< const LCAPickBatchLine* > mcPickLines;	// The set of pick batch lines corresponding to this batch

		double mTotalVolume;
		double mTotalWeight;
};

#endif
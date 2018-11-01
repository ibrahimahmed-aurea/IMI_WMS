#pragma once

#ifndef LCARESULT_H
#define LCARESULT_H

#include <vector>
#include <set>
#include <QtCore/qstring.h>
#include <QtCore/qstringlist.h>
#include "lcawarehouse.h"
#include "lcaanneal.h"

class LCAResultLine;

typedef std::vector<const LCAResultLine*>::const_iterator LCAResultLineIterator;

/* This class stores the results for a run of the load carrier algorithm. It contains all output required
 * to construct the LCAPBROW_OUTPUT table.
 */
class LCAResult
{
	public:

		LCAResult( const QString sNewGroupId );
		~LCAResult();

		LCAResultLineIterator beginResultLine() { return mcResultLines.begin(); }
		LCAResultLineIterator endResultLine() { return mcResultLines.end(); }
		
		std::set< QString >::iterator beginCarIds() { return mcCarIds.begin(); }
		std::set< QString >::iterator endCarIds() { return mcCarIds.end(); }

		int  addResultLine( const QString sPBRowId,
							const QString sPBRowSplitId,
							const double  ordQty,
							const QString sPBCarIdVirtual,
							const int pickSeq,
							const QString sCatGroup,
							const QString sWsId,
							const QString sAisle,
							const double xCoord,
							const double yCoord,
							const QString sWPAdr );


		int	getLoadCarrierCount() const;
		int	getResultCode() const;
		int getBeautyCount();
		int getLowerBound() const { return mLowerBound; }

		std::map< int, std::set<QString> > getErrors() const;
		QString getNewGroupId() const { return msNewGroupId; }
		
		int getResultLineCount() const;
		const LCAResultLine* const getResultLine( int resultLineNr ) const;

		void	sortResultLines();
		double	calculatePickDistance( const LCAWarehouse& rWarehouse, bool crowDistance, QString sStrekWsId, double strekX, double strekY, bool extraAisle );
		double	getDistanceForCar( QString sCarId );
		double  getTotalDistance();
		void	addError( int errorCode, const QString sMessage );

		void    copyErrors( LCAResult* pOther );
		void    setLowerBound( int lowerBound ) { mLowerBound = lowerBound; }


	protected:

		void clear();

		QString	msNewGroupId;											// Unique Id of the pick batch, i.e., the set of rows from the PBROW table that should be processed together

		std::vector<const LCAResultLine*> mcResultLines;				// The lca result lines resulting from the run of the algorithm
		std::set< QString > mcCarIds;									// The set of used car ids
		std::map< QString, double > mcPickDistancePerCar;				// The pick distance per car

		std::map< int, std::set<QString> >	mcErrors;					// List of error messages
		int mWorstErrorCode;											// The worst error code
		double mTotalDistance;											// The total distance for the batch
		int mLowerBound;												// Lower bound on the number of load carriers
};

class LCAResultCompare
{
	public:

		bool operator()(const LCAResult * const pLCA1, const LCAResult * const pLCA2 ) const
		{
			return pLCA1->getNewGroupId() < pLCA2->getNewGroupId();
		}
};

#endif
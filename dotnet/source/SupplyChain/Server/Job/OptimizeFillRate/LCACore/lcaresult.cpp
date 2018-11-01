#include "lcaresult.h"
#include "lcaresultline.h"
#include "lcaerrors.h"
		
LCAResult::LCAResult( const QString sNewGroupId )
{
	msNewGroupId = sNewGroupId;
	mWorstErrorCode = gcLCAOk;
	mTotalDistance = 0;
	mLowerBound = 0;
}

LCAResult::~LCAResult()
{
	clear();
}

// Adds a new resultline to the result
int LCAResult::addResultLine( const QString sPBRowId, const QString sPBRowSplitId, const double ordQty,
	const QString sPBCarIdVirtual, const int pickSeq, const QString sCatGroup, const QString sWsId,
	const QString sAisle, const double xCoord, const double yCoord, const QString sWPAdr )
{
	// First create the new result line in memory
	LCAResultLine* pLine = new LCAResultLine( sPBRowId, sPBRowSplitId, ordQty, sPBCarIdVirtual, pickSeq, sCatGroup, sWsId, sAisle, xCoord, yCoord, sWPAdr );
	
	// If the memory cannot be allocated, return a memory error
	if ( pLine == 0 ) return gcLCAErrorNoMemory;

	// Add the newly created result line to the collection
	mcResultLines.push_back( pLine );
	
	// Add the car id to the set
	mcCarIds.insert( sPBCarIdVirtual );

	return gcLCAOk;
}
		
// This method returns the number of load carriers used
int LCAResult::getLoadCarrierCount() const
{
	return mcCarIds.size();
}

// This message returns the worst error code
// The result is only valid if the last error code is gcLCAOk
// If the result is not gcLCAOk but below gcLCAMaxAcceptableError, the result is valid except for distance criteria
int	LCAResult::getResultCode() const
{
	return mWorstErrorCode;
}

// This method returns the full list of errors that were encountered
std::map< int, std::set<QString> > LCAResult::getErrors() const
{
	return mcErrors;
}

// Returns the number of result lines
int LCAResult::getResultLineCount() const
{
	return mcResultLines.size();
}
		
// Returns the requested resultline
const LCAResultLine* const LCAResult::getResultLine( int resultLineNr ) const
{
	if ( resultLineNr < 0 ) return 0;
	if ( resultLineNr >= mcResultLines.size() ) return 0;
	return mcResultLines[ resultLineNr ];
}

// This method sorts the result lines in ascending order of picksequence
void LCAResult::sortResultLines()
{
	std::sort( mcResultLines.begin(), mcResultLines.end(), LCAResultLineCompare() );
}

// This method returns the total pick distance for the pickbatch and sets the pick distances per load carrier
double LCAResult::calculatePickDistance( const LCAWarehouse& rWarehouse, bool crowDistance, QString sStrekWsId, double strekX, double strekY, bool extraAisle )
{
	// We start by sorting the resultlines in order of pick sequence
	// They are not necessarily sorted in this way by all construction methods
	sortResultLines();
	mcPickDistancePerCar.clear();

	// We adjust the coordinates of the strek to take the offset of the area into account
	std::pair<double,double> offSetStrek = rWarehouse.getAreaOffset( sStrekWsId );
	double adjustedXStrek = strekX + offSetStrek.first;
	double adjustedYStrek = strekY + offSetStrek.second;

	// We calculate all distances between stops
	for ( int line = 1; line < mcResultLines.size(); ++line )
	{
		const LCAResultLine * const pPrevLine = mcResultLines[line-1];
		const LCAResultLine * const pCurLine = mcResultLines[line];

		const LCASplittedAisle* pAislePrev = 
					rWarehouse.getSplittedAisle( pPrevLine->getWsId(), pPrevLine->getAisle(), pPrevLine->getXCoord(), pPrevLine->getYCoord() );
		const LCASplittedAisle* pAisleCur = 
					rWarehouse.getSplittedAisle( pCurLine->getWsId(), pCurLine->getAisle(), pCurLine->getXCoord(), pCurLine->getYCoord() );
		
		std::pair<double,double> offSetPrev = rWarehouse.getAreaOffset( pPrevLine->getWsId() );
		std::pair<double,double> offSetCur = rWarehouse.getAreaOffset( pCurLine->getWsId() );

		double adjustedXPrev = pPrevLine->getXCoord() + offSetPrev.first;
		double adjustedYPrev = pPrevLine->getYCoord() + offSetPrev.second;
		double adjustedXCur = pCurLine->getXCoord() + offSetCur.first;
		double adjustedYCur = pCurLine->getYCoord() + offSetCur.second;
		
		// If the results are not on the same load carrier, we have to calculate the distance 
		// from the prev to the strek and from the start to the current
		if ( pPrevLine->getPBCarIdVirtual() != pCurLine->getPBCarIdVirtual() )
		{
			if ( crowDistance )
			{
				mcPickDistancePerCar[pPrevLine->getPBCarIdVirtual()] += rWarehouse.getExitDistanceCrow();
				mcPickDistancePerCar[pCurLine->getPBCarIdVirtual()] += rWarehouse.getEntryDistanceCrow();
			}
			else
			{
				mcPickDistancePerCar[pPrevLine->getPBCarIdVirtual()] += 
					rWarehouse.getExitDistance( pAislePrev, adjustedXPrev, adjustedYPrev, adjustedXStrek, adjustedYStrek, extraAisle );

				mcPickDistancePerCar[pCurLine->getPBCarIdVirtual()] += 
					rWarehouse.getEntryDistance( pAisleCur, adjustedXCur, adjustedYCur, adjustedXStrek, adjustedYStrek, extraAisle  );
			}
			continue;
		}

		// In all other cases we calculate the distance between the prev and the current
		if ( crowDistance )
		{
			mcPickDistancePerCar[pCurLine->getPBCarIdVirtual()] += 
				rWarehouse.getDistanceCrow( adjustedXPrev, adjustedYPrev, adjustedXCur, adjustedYCur );
		}
		else
		{
			mcPickDistancePerCar[pCurLine->getPBCarIdVirtual()] += 
				rWarehouse.getDistance( pAislePrev, adjustedXPrev, adjustedYPrev, pAisleCur, adjustedXCur, adjustedYCur );
		}
	}
	
	// And add the distance from the start of the LC to the first stop and from the last stop
	// to the strek
	if ( mcResultLines.size() > 0 )
	{
		const LCAResultLine * const pFirstLine = mcResultLines[0];
		const LCAResultLine * const pLastLine = mcResultLines[mcResultLines.size()-1];

		const LCASplittedAisle* pAisleFirst = 
					rWarehouse.getSplittedAisle( pFirstLine->getWsId(), pFirstLine->getAisle(), pFirstLine->getXCoord(), pFirstLine->getYCoord() );
		const LCASplittedAisle* pAisleLast = 
					rWarehouse.getSplittedAisle( pLastLine->getWsId(), pLastLine->getAisle(), pLastLine->getXCoord(), pLastLine->getYCoord() );
		
		std::pair<double,double> offSetFirst = rWarehouse.getAreaOffset( pFirstLine->getWsId() );
		std::pair<double,double> offSetLast = rWarehouse.getAreaOffset( pLastLine->getWsId() );

		double adjustedXFirst = pFirstLine->getXCoord() + offSetFirst.first;
		double adjustedYFirst = pFirstLine->getYCoord() + offSetFirst.second;
		double adjustedXLast  = pLastLine->getXCoord() + offSetLast.first;
		double adjustedYLast  = pLastLine->getYCoord() + offSetLast.second;


		if ( crowDistance )
		{
			mcPickDistancePerCar[pFirstLine->getPBCarIdVirtual()] += rWarehouse.getEntryDistanceCrow();
			mcPickDistancePerCar[pLastLine->getPBCarIdVirtual()] += rWarehouse.getExitDistanceCrow();
		}
		else
		{
			mcPickDistancePerCar[pFirstLine->getPBCarIdVirtual()] += rWarehouse.getEntryDistance( pAisleFirst, adjustedXFirst, adjustedYFirst, adjustedXStrek, adjustedYStrek, extraAisle );
			mcPickDistancePerCar[pLastLine->getPBCarIdVirtual()] += rWarehouse.getExitDistance( pAisleLast, adjustedXLast, adjustedYLast, adjustedXStrek, adjustedYStrek, extraAisle );
		}
	}

	mTotalDistance = 0;

	for ( std::map< QString, double >::iterator iCar = mcPickDistancePerCar.begin(); iCar != mcPickDistancePerCar.end(); ++iCar )
	{
		mTotalDistance += iCar->second;
	}
	
	return mTotalDistance;
}

// Adds a new error with the given error code and message
// If the error code is worse than the previous error, this will replace the previous error code
// The error text is always added to te list
void LCAResult::addError( int errorCode, const QString sMessage )
{
	if ( errorCode != gcLCAOk )
	{
		if ( errorCode > mWorstErrorCode )
		{
			mWorstErrorCode = errorCode;
		}
		mcErrors[errorCode].insert( sMessage );
	}
}

// Frees the memory claimed by the LCAResult
void LCAResult::clear()
{
	// Delete all referenced pick batch lines and clear the collection
	for ( int i=0; i<mcResultLines.size(); i++ )
	{
		if ( mcResultLines[i] ) 
		{
			delete mcResultLines[i];
			mcResultLines[i] = 0;
		}
	}
	mcResultLines.clear();
	msNewGroupId.clear();
	mcCarIds.clear();
}

// Returns the total pick distance for a car. This method can only be called if the distance calculation has
// be performed previously with calculatePickDistance
double LCAResult::getDistanceForCar( QString sCarId )
{
	if( mcPickDistancePerCar.find( sCarId ) != mcPickDistancePerCar.end() )
	{
		return mcPickDistancePerCar[sCarId];
	}
	return 0.0;

}

// Returns the total pick distance. This method can only be called if the distance calculation has
// be performed previously with calculatePickDistance
double  LCAResult::getTotalDistance()
{
	return mTotalDistance;
}

// Returns the total number of beauties in the result
int LCAResult::getBeautyCount()
{	
	sortResultLines();
	std::map< QString, std::set< QString > > cCatGroupsPerCar;
	int beauties = 0;

	for ( int i=0; i<mcResultLines.size(); i++ )
	{
		cCatGroupsPerCar[mcResultLines[i]->getPBCarIdVirtual()].insert( mcResultLines[i]->getCatGroup() );
	}
	for ( std::map< QString, std::set< QString > >::iterator iCar = cCatGroupsPerCar.begin(); iCar != cCatGroupsPerCar.end(); ++iCar )
	{
		if ( iCar->second.size() == 1 )
		{
			beauties ++;
		}
	}
	return beauties;
}

// Copies all errors from another result and add them to the current result
// Used to copy all errors from the check to the final result
void  LCAResult::copyErrors( LCAResult* pOther )
{
	mWorstErrorCode = pOther->getResultCode() > mWorstErrorCode ? pOther->getResultCode() : mWorstErrorCode;
	
	for ( std::map< int, std::set<QString> >::iterator itErrors = pOther->mcErrors.begin(); itErrors != pOther->mcErrors.end(); itErrors++ )
	{
		mcErrors[itErrors->first].insert( itErrors->second.begin(), itErrors->second.end() );
	}
}
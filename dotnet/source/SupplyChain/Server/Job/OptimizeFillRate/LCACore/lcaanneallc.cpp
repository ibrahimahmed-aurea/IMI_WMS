#include "lcaanneallc.h"
#include "lcawarehouse.h"
#include "lcaerrors.h"

LCAAnnealLC::LCAAnnealLC()
{
	mTotalVolume = 0.0;
	mTotalWeight = 0.0;
	mTotalDistance = 0.0;
	mPBRows = 0;
	mNumberAisles = 0;
	mSmallestAisleVolume = 0;
	msAisleWithSmallestVolume = "";
}

// We have to delete all created lines
LCAAnnealLC::~LCAAnnealLC()
{
	for ( AnnealLineSet::const_iterator iLine = mcLines.begin();  iLine != mcLines.end(); ++iLine )
	{
		delete *iLine;
	}
	mcLines.clear();
}

// Insert a new line to the container and update volume, weight, distance, unique pbrowcount and volume per catgroup
void LCAAnnealLC::insertLine(  const LCAAnnealPickBatchLine* const pLine , const LCAWarehouse& rWarehouse, double strekX, double strekY )
{
	bool mustUpdateSmallestAisle = false;

	if ( pLine == 0 ) return;

	// the volume and weight increase with the volume and weight of the line
	mTotalVolume += pLine->getTotalVolume();
	mTotalWeight += pLine->getTotalWeight();

	// We update the volume per category group and per aisle
	mcCategoryGroupVolume[ pLine->getCatGroup() ] += pLine->getTotalVolume();
	mcAisleVolume[ pLine->getAisle() ] += pLine->getTotalVolume();

	if ( (pLine->getTotalVolume() < mSmallestAisleVolume) && ( mcAisleVolume[ pLine->getAisle() ] < mSmallestAisleVolume ) )
	{
		mustUpdateSmallestAisle = true;
	}

	// To determine the change in distance and number of unique pbrows, we need to now the successor and the predecessor in the picksequence
	// If the successor or predecessor dont exist, the are initialized with mcLines.end()
	
	// First search for the successor, using the upper bound
	AnnealLineSet::const_iterator iNext = mcLines.upper_bound( pLine );
	AnnealLineSet::const_iterator iPrev;

	// If the successor is not the first in the list, then there is a predecessor as well
	if ( iNext != mcLines.begin() )
	{
		iPrev = iNext;
		iPrev--;
	}
	// Otherwise there is no predecessor
	else
	{
		iPrev = mcLines.end();
	}

	
	// Scenario 1: There is not successor, and no predecessor
	if ( iPrev == mcLines.end() && iNext == mcLines.end() )
	{
		// Since there is no other line, this means the pbrowid must be unique
		mPBRows++;

		// The total distance is equal to the start point of the load carrier to location of this line
		// plus the distance from the location of this line to the end point of the load carrier
		mTotalDistance += rWarehouse.getEntryDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance += rWarehouse.getExitDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(), strekX, strekY, true );
	}
	// Scenario 2: There is a successor, but no predecessor
	else if ( iPrev == mcLines.end() ) // && iNext != mcLines.end()
	{
		// If there is a successor, but no predecessor, the only pbrowid that can match, is that of the successor
		// If the successor has another pbrowid, then this one is unique
		if (  (*iNext)->getPBRowId() != pLine->getPBRowId() ) 
		{
			mPBRows++;
		}

		// Since this line is the new first line, we have to remove the distance from the start point of the load carrier to the 
		// line that was the first line and replace is by the distance from the start point of the load carrier to the new line
		// plus the distance from the new line to the line that was the first line before the insertion
		mTotalDistance -= rWarehouse.getEntryDistance( (*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance += rWarehouse.getEntryDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance += rWarehouse.getDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(),
											    (*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord() );
	}
	// Scenario 3: There is a predecessor, but no successor
	else if ( iNext == mcLines.end() ) // && iPrev != mcLines.end()
	{
		// See comments for scenario 2
		if (  (*iPrev)->getPBRowId() != pLine->getPBRowId() ) 
		{
			mPBRows++;
		}

		mTotalDistance -= rWarehouse.getExitDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance += rWarehouse.getExitDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance += rWarehouse.getDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(),
													pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord() );
	}
	// Scenario 4: Both the predecessor and the successor exist
	else 
	{
		// The pbrouw is unique if it is not equal to the previous and not equal to the next
		if ( (*iPrev)->getPBRowId() != pLine->getPBRowId() && (*iNext)->getPBRowId() != pLine->getPBRowId() )
		{
			mPBRows++;
		}

		// Since we insert the new line between prev and next, we have to remove the distance between prev and next and
		// add the distance between prev and new and new and next.
		mTotalDistance -= rWarehouse.getDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(),
													(*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord() );
		mTotalDistance += rWarehouse.getDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(), 
													pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord() );
		mTotalDistance += rWarehouse.getDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(),
													(*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord() );
	}

	// Finally add the line
	mcLines.insert( pLine );

	// And update the smallest aisle volume if it might have gotten bigger
	if ( pLine->getAisle() == msAisleWithSmallestVolume || msAisleWithSmallestVolume.isEmpty() || mustUpdateSmallestAisle )
	{
		updateSmallestAisleVolume();
	}
}

// Remove a line from the load carrier, update volume, weight, distance, unique pbrowcount and volume per catgroup
void LCAAnnealLC::removeLine(  const LCAAnnealPickBatchLine* const pLine, const LCAWarehouse& rWarehouse, double strekX, double strekY )
{
	bool mustUpdateSmallestAisle = false;

	if ( pLine == 0 ) return;

	// The total volume and weight are lowered by the volume and weight for this line
	mTotalVolume -= pLine->getTotalVolume();
	mTotalWeight -= pLine->getTotalWeight();

	// The same goes for the volume for the categorygroup that is associated with this line
	// If the total volume of the categorygroup drops to zero, than we remove the categorygroup as a whole from the load carrier
	std::map< QString, double >::iterator iCatVol = mcCategoryGroupVolume.find( pLine->getCatGroup() );
	iCatVol->second -= pLine->getTotalVolume();
	if ( iCatVol->second < gcLCAEpsilon )
	{
		mcCategoryGroupVolume.erase( iCatVol );
	}

	std::map< QString, double >::iterator iAisleVol = mcAisleVolume.find( pLine->getAisle() );
	iAisleVol->second -= pLine->getTotalVolume();
	if (iAisleVol->second < gcLCAEpsilon )
	{
		mcAisleVolume.erase( iAisleVol );
		if ( msAisleWithSmallestVolume == pLine->getAisle() ) 
		{
			mustUpdateSmallestAisle = true;
		}
	}
	else if ( iAisleVol->second < mSmallestAisleVolume )
	{
		// By removing this line, the smallest aisle volume has switched to this aisle
		mSmallestAisleVolume = iAisleVol->second;
		msAisleWithSmallestVolume = iAisleVol->first;
	}
	
	// We determine the predecessor and the successor of the line that has to be removed
	// The get value mcLines.end() if they dont exist
	AnnealLineSet::iterator iCurrent = mcLines.find( pLine );
	AnnealLineSet::iterator iNext = iCurrent;
	iNext++;
	AnnealLineSet::iterator iPrev = iCurrent;
	if ( iCurrent != mcLines.begin() )
	{
		iPrev = iCurrent;
		iPrev--;
	}
	else
	{
		iPrev = mcLines.end();
	}

	// Scenario 1: No successor and no predecessor
	if ( iPrev == mcLines.end() && iNext == mcLines.end() )
	{
		// We removed the last item, so all is zero
		// We also reset the volume and weight, in case the double precision makes us be stuck with epsilon weight and volume
		mTotalVolume = 0;
		mTotalWeight = 0;
		mTotalDistance = 0;
		mPBRows = 0;
	}
	// Scenario 2: There is a successor, but no predecessor
	else if ( iPrev == mcLines.end() )
	{
		// If there is a successor, but no predecessor, the only pbrowid that can match, is that of the successor
		// If the successor has another pbrowid, then this one is unique
		if (  (*iNext)->getPBRowId() != pLine->getPBRowId() ) 
		{
			mPBRows--;
		}

		// Since the succesor becomes the new first line, we have to add the distance from the start point of the load carrier to the 
		// successor and have to remove the distance from the startpoint to the current line and the distance from the current line 
		// to its successor
		mTotalDistance += rWarehouse.getEntryDistance( (*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance -= rWarehouse.getEntryDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance -= rWarehouse.getDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(),
													(*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord() );
	}
	// Scenario 3: There is a predecessor, but no successor
	else if ( iNext == mcLines.end() )
	{
		// See comments for scenario 2
		if (  (*iPrev)->getPBRowId() != pLine->getPBRowId() ) 
		{
			mPBRows--;
		}

		mTotalDistance += rWarehouse.getExitDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance -= rWarehouse.getExitDistance( pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(), strekX, strekY, true );
		mTotalDistance -= rWarehouse.getDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(), 
													pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord() );
	}
	// Scenario 4: Both the predecessor and the successor exist
	else 
	{
		if (  (*iPrev)->getPBRowId() != pLine->getPBRowId() && (*iNext)->getPBRowId() != pLine->getPBRowId() ) 
		{
			mPBRows--;
		}

		mTotalDistance += rWarehouse.getDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(),
													(*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord() );
		mTotalDistance -= rWarehouse.getDistance( (*iPrev)->getSplittedAisle(), (*iPrev)->getAdjustedXCord(), (*iPrev)->getAdjustedYCord(), 
													pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord() );
		mTotalDistance -= rWarehouse.getDistance(  pLine->getSplittedAisle(), pLine->getAdjustedXCord(), pLine->getAdjustedYCord(),
													(*iNext)->getSplittedAisle(), (*iNext)->getAdjustedXCord(), (*iNext)->getAdjustedYCord() );
	}

	mcLines.erase( iCurrent );

	if ( mustUpdateSmallestAisle )
	{
		updateSmallestAisleVolume();
	}
}

// This method selects a random line from the load carrier
// Since the lines are organized in a set in order to keep the pick sequence correct, we have only linear access to the items
const LCAAnnealPickBatchLine* const LCAAnnealLC::selectRandomLine( LCARandom& rRandom )
{
	if ( mcLines.size() == 0 ) return 0;

	long line = rRandom.draw( 0, mcLines.size()-1 );

	// Depending on if our line is in the first half or in the second half of the set, we start counting
	// from the front or from the back.
	if ( line <= mcLines.size()/2 )
	{
		AnnealLineSet::iterator iLine = mcLines.begin();
		long current = 0;
		while ( current < line )
		{
			iLine++;
			current++;
		}

		return *iLine;
	}
	else
	{
		line = mcLines.size() - 1 - line;
		AnnealLineSet::reverse_iterator iLine = mcLines.rbegin();
		long current = 0;
		while ( current < line )
		{
			iLine++;
			current++;
		}

		return *iLine;
	}

}

// This method selects a random line from the load carrier, except that the line is not from the productcategory that
// has the largest volume
const LCAAnnealPickBatchLine* const LCAAnnealLC::selectRandomMinorityVolumeLine( LCARandom& rRandom )
{
	if ( mcLines.size() == 0 ) return 0;

	double maxVolumeForCat = 0;
	QString sMaxVolumeCat = "";

	// First determine the category that has the largest volume
	for ( std::map< QString, double>::const_iterator iCat = mcCategoryGroupVolume.begin(); iCat != mcCategoryGroupVolume.end(); iCat++ )
	{
		if ( iCat->second > maxVolumeForCat ) 
		{
			sMaxVolumeCat = iCat->first;
			maxVolumeForCat = iCat->second;
		}
	}
	
	// Collect all lines that are from other categories than the one with the largest volume
	std::vector< const LCAAnnealPickBatchLine* > cMinorityLines;
	for ( AnnealLineSet::iterator iLine = mcLines.begin(); iLine != mcLines.end(); ++iLine )
	{
		if ( (*iLine)->getCatGroup() != sMaxVolumeCat )
		{
			cMinorityLines.push_back( *iLine );
		}
	}

	// If all lines are from the largest volume, we are done
	if ( cMinorityLines.empty() ) return 0;

	// Otherwise, select a random line and return it
	long line = rRandom.draw( 0, cMinorityLines.size()-1 );

	return cMinorityLines[line];
}

// This method selects a random line from the least visited aisle of load carrier
const LCAAnnealPickBatchLine* const  LCAAnnealLC::selectRandomMinorityAisleLine( LCARandom& rRandom )
{
	if ( mcLines.size() == 0 ) return 0;

	std::vector< const LCAAnnealPickBatchLine* > cMinorityLines;
	for ( AnnealLineSet::iterator iLine = mcLines.begin(); iLine != mcLines.end(); ++iLine )
	{
		if ( (*iLine)->getAisle() == msAisleWithSmallestVolume )
		{
			cMinorityLines.push_back( *iLine );
		}
	}

	if ( cMinorityLines.empty() ) return 0;

	long line = rRandom.draw( 0, cMinorityLines.size()-1 );

	return cMinorityLines[line];
}

// Selects all lines from the aisle that has the smallest total volume on this load carrier
std::vector<const LCAAnnealPickBatchLine*> const LCAAnnealLC::selectSmallAisle( LCARandom& rRandom )
{
	std::vector<const LCAAnnealPickBatchLine*> selection;
	if ( mcLines.empty() ) return selection;

	std::vector< const LCAAnnealPickBatchLine* > cMinorityLines;
	for ( AnnealLineSet::iterator iLine = mcLines.begin(); iLine != mcLines.end(); ++iLine )
	{
		if ( (*iLine)->getAisle() == msAisleWithSmallestVolume )
		{
			selection.push_back( *iLine );
		}
	}

	return selection;
}

// Select a set of lines that, when all removed, create a reduction in the total distance the load carrier has to travel
std::vector<const LCAAnnealPickBatchLine*> const LCAAnnealLC::selectLongDistanceLines( LCARandom& rRandom, const LCAWarehouse& rWarehouse, double strekX, double strekY, double maxVolume )
{
	std::vector<const LCAAnnealPickBatchLine*> selection;
	if ( mcLines.empty() ) return selection;

	std::vector< const LCASplittedAisle* > cAisles;
	std::vector< double > cVolume;
	std::vector< std::pair<const LCASplittedAisle*,const LCASplittedAisle*> > cRemovalCandidates;

	const LCAAnnealPickBatchLine* pLine = 0;
	const LCAAnnealPickBatchLine* pPrev = 0;

	// We first determine which aisle we visit in which order and how much volume we see in each aisle
	for ( AnnealLineSet::iterator iLine = mcLines.begin(); iLine != mcLines.end(); ++iLine )
	{
		pLine = *iLine;
		if ( !pPrev || ( pPrev->getSplittedAisle() != pLine->getSplittedAisle() ) )
		{
			cAisles.push_back( pLine->getSplittedAisle() );
			cVolume.push_back( 0 );
		}
		cVolume.back() += pLine->getTotalVolume();
		pPrev = pLine;
	}

	// We check if removing the section from and including start to and including end will be an option
	// It is an option if
	// - The volume of the section is smaller than 10% of the maximum
	// - The total distance gets smaller if the section is removed
	for ( int start = 0; start < cAisles.size(); start++ )
	{
		double volume = 0.0;
		double distancePrevToStart = 0.0;
		double lengthStartEnd = 0.0;
		
		const LCASplittedAisle* pStart = cAisles[start];
		const LCASplittedAisle* pPrev = 0; 

		if ( start > 0 )
		{
			pPrev = cAisles[start-1];
			distancePrevToStart = rWarehouse.getDistance( pPrev, pPrev->getToXCord(), pPrev->getToYCord(), pStart, pStart->getFrmXCord(), pStart->getFrmYCord() );
		}
		else
		{
			distancePrevToStart = rWarehouse.getEntryDistance( pStart, pStart->getFrmXCord(), pStart->getFrmYCord(), strekX, strekY, true );
		}


		for ( int end=start; end < cAisles.size(); end++ )
		{
			volume += cVolume[end];
			if ( volume > 0.1 * maxVolume ) break;

			const LCASplittedAisle* pEnd = cAisles[end];

			double distanceEndToNext = 0.0;
			double distancePrevToNext = 0.0;

			lengthStartEnd += pEnd->getLength();

			if ( end < cAisles.size()-1 )
			{
				const LCASplittedAisle* pNext = cAisles[end+1];
				distanceEndToNext = rWarehouse.getDistance( pEnd, pEnd->getToXCord(), pEnd->getToYCord(), pNext, pNext->getFrmXCord(), pNext->getFrmYCord() );
				if ( pPrev ) 
				{
					distancePrevToNext = rWarehouse.getDistance( pPrev, pPrev->getToXCord(), pPrev->getToYCord(), pNext, pNext->getFrmXCord(), pNext->getFrmYCord() );
				}
				else
				{
					distancePrevToNext = rWarehouse.getEntryDistance( pNext, pNext->getFrmXCord(), pNext->getFrmYCord(), strekX, strekY, true );
				}
			}
			else
			{
				distanceEndToNext = rWarehouse.getExitDistance( pEnd, pEnd->getToXCord(), pEnd->getToYCord(), strekX, strekY, true );
				if ( pPrev ) distancePrevToNext = rWarehouse.getExitDistance( pPrev, pPrev->getToXCord(), pPrev->getToYCord(), strekX, strekY, true );
				// If not pPrev and not pNext, then distancePrevToNext remains zero
			}

			if ( distancePrevToStart + distanceEndToNext + lengthStartEnd > distancePrevToNext + gcLCAEpsilon )
			{
				if ( !cRemovalCandidates.empty() && cRemovalCandidates.back().first == pStart )
				{
					cRemovalCandidates.pop_back();
				}
				cRemovalCandidates.push_back( std::make_pair( pStart, pEnd ) );
			}
		}
	}

	if ( cRemovalCandidates.empty() ) return selection;

	// From the candidates for removal, we randomly select one
	int candidate = rRandom.draw( 0,cRemovalCandidates.size()-1 );
	
	bool addModus = false;
	for ( AnnealLineSet::iterator iLine = mcLines.begin(); iLine != mcLines.end(); ++iLine )
	{
		pLine = *iLine;
		if ( pLine->getSplittedAisle() == cRemovalCandidates[candidate].first )
		{
			addModus = true;
		}
		if ( pLine->getSplittedAisle() == cRemovalCandidates[candidate].second )
		{
			addModus = false;
		}

		if ( addModus || (pLine->getSplittedAisle() == cRemovalCandidates[candidate].second) )
		{
			selection.push_back( pLine );
		}
	}

	return selection;
}


// This method returns the total volume that has to be removed from the load carrier in order to make it a beauty, i.e., all volume
// except for the volume of the most volumous category group
double LCAAnnealLC::getUglyVolume() const
{
	double maxVolumeForCat = 0;

	for ( std::map< QString, double>::const_iterator iCat = mcCategoryGroupVolume.begin(); iCat != mcCategoryGroupVolume.end(); iCat++ )
	{
		if ( iCat->second > maxVolumeForCat ) maxVolumeForCat = iCat->second;
	}

	return mTotalVolume - maxVolumeForCat;
}

// This method is used to update the variable containing the smallest volume of an aisle in a load carrier.
// It is called by the insertLine and removeLine methods whenever an insert or removal is done that might
// effect this value
void LCAAnnealLC::updateSmallestAisleVolume()
{
	mSmallestAisleVolume = -1;
	msAisleWithSmallestVolume = "";

	for ( std::map< QString, double>::const_iterator iAisle = mcAisleVolume.begin(); iAisle != mcAisleVolume.end(); iAisle++ )
	{
		if ( iAisle->second < mSmallestAisleVolume || mSmallestAisleVolume < 0) 
		{
			mSmallestAisleVolume = iAisle->second;
			msAisleWithSmallestVolume = iAisle->first;
		}
	}
}
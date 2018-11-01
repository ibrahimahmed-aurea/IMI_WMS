#include "lcaannealcriterium.h"

// Initialize the criterium
// All solutions that are above maximumDistance for the full batch or above maxVolume, maxWeight or maxLines for an individual LC will be invalid
// All solutions that are below minBeauties for the full pickbatch are rejected
// The algoritm may run for a maximum of iterations
LCAAnnealCriterium::LCAAnnealCriterium( double maximumDistance, double maxVolume, double maxWeight, int maxLines, int minBeauties, int iterations )
{
	mValue = 0;
	mFinished = false;
	mValid = false;
	mIterationsPerLC = iterations;
	mMaxIterations = 0;
	mStartCool = 0;

	mMaxVolume = maxVolume;
	mMaxWeight = maxWeight;
	mMaxLines = maxLines;
	mMaximumDistance = maximumDistance;
	mMinBeauties = minBeauties;

	mTotalDistance = 0;
	mLoadCarrierCount = 0;
	mLoadCarriersAboutVolume = 0;
	mLoadCarriersAboutWeight = 0;
	mLoadCarriersAboutLineCount = 0;
	mBeautyCount = 0;
	mVolumeSmallestLC = 0;
}

// This method updates all values that are used to make decisions on how good a given configuration
// of load carriers is
void LCAAnnealCriterium::updateValues( std::vector< LCAAnnealLC >& rcLoadCarriers )
{
	mTotalDistance = 0;
	mLoadCarrierCount = 0;
	mLoadCarriersAboutVolume = 0;
	mLoadCarriersAboutWeight = 0;
	mLoadCarriersAboutLineCount = 0;
	mBeautyCount = 0;
	mResultLines = 0;
	mVolumeSmallestLC = mMaxVolume;

	// We loop over all load carriers in the solution and do some checks on them
	for ( long i=0; i<rcLoadCarriers.size(); i++ )
	{
		// We only do the checks for load carriers that are not empty
		if ( rcLoadCarriers[i].getTotalVolume() >= gcLCAEpsilon ) 
		{
			mLoadCarrierCount++;
			mTotalDistance +=  rcLoadCarriers[i].getTotalDistance();	
			mResultLines += rcLoadCarriers[i].getPBRowCount();

			if ( rcLoadCarriers[i].getTotalVolume() > mMaxVolume ) mLoadCarriersAboutVolume++;
			if ( rcLoadCarriers[i].getTotalWeight() > mMaxWeight ) mLoadCarriersAboutWeight++;
			if ( rcLoadCarriers[i].getPBRowCount() > mMaxLines ) mLoadCarriersAboutLineCount++;
			if ( rcLoadCarriers[i].isBeauty() ) mBeautyCount++;

			if ( (rcLoadCarriers[i].getTotalVolume() < mVolumeSmallestLC) && 
			 (rcLoadCarriers[i].getTotalVolume() > gcLCAEpsilon ) )
			{
				mVolumeSmallestLC = rcLoadCarriers[i].getTotalVolume();
			}

		}
	}

	// For a configuration to be valid, it must at least adhere to these criteria
	mValid = ( mLoadCarriersAboutVolume==0 ) && 
		     ( mLoadCarriersAboutWeight==0 ) && 
			 ( mLoadCarriersAboutLineCount==0 ) && 
			 ( mTotalDistance < mMaximumDistance ) &&
			 ( mBeautyCount >= mMinBeauties );
			 
}


// The minimize LC criterium has one extra parameter, the optimum number of load carriers. This can be used
// to check if the optimum number is found
LCAAnnealCriteriumMinimizeLCs::LCAAnnealCriteriumMinimizeLCs( int optimumLoadCarriers, 
	double maximumDistance, double maxVolume, double maxWeight, int maxLines, int iterationsPerLC )
:LCAAnnealCriterium( maximumDistance, maxVolume, maxWeight, maxLines, 0, iterationsPerLC )
{
	mOptimumLoadCarriers = optimumLoadCarriers;
}

// We evaluate the current configuration on how good it is with respect to minimize the number of load carriers
void LCAAnnealCriteriumMinimizeLCs::evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues )
{
	// We first run the update values method, to calculate all relevent values to make a decision
	updateValues(rcLoadCarriers);
	
	// For this criterium we need one extra value, the squared fill. This value gets higher when more load carriers
	// get closer to their maximum volume. In the search process we prefer a a couple of full containers and a couple 
	// of empty containers over all half full containers.
	double squaredFill = 0;
	for ( long i=0; i<rcLoadCarriers.size(); i++ )
	{
		squaredFill += ( rcLoadCarriers[i].getTotalVolume() / mMaxVolume ) * ( rcLoadCarriers[i].getTotalVolume() / mMaxVolume );
	}

	// The value of a solution consists of a weighted sum of some parts:
	// - Most important is the number of load carriers
	// - Second important is how full the least filled container is. Every step we try to make the least filled 
	//   container even less filled, so it ends up empty in the end
	// - About equally important are the distance used (since there is a constraint on this) and the configuration
	//   of the other load carriers. Higher squared fill is better.

	double valueLoadCarriers = (10*(mLoadCarrierCount - 1)) + ( mVolumeSmallestLC / mMaxVolume );
	double weightedValueLoadCarriers = (1000 * valueLoadCarriers);
	double weightedDistance = (1500 * mTotalDistance / mMaximumDistance);
	mValue = weightedValueLoadCarriers + weightedDistance - ( 20 * squaredFill);
	
	// We have found the optimum solution if the number of containers is equal to the optimum number
	// AND the solutuin is valid
	mFinished = ( mLoadCarrierCount <= mOptimumLoadCarriers )  && mValid;
	
	// At the start of the annealing this method will be called with updateStartValues = true
	// We then have to set the inial cool value and the number of iterations
	if ( updateStartValues )
	{
		mStartCool = 20;
		mMaxIterations = rcLoadCarriers.size() * mIterationsPerLC;
	}
}

// This is the criterium used for maximizing store friendly delivery, i.e., maximizing the number of beauties
LCAAnnealCriteriumSFD::LCAAnnealCriteriumSFD( double maximumDistance, double maxVolume, double maxWeight, int maxLines, int iterationsPerLC )
:LCAAnnealCriterium( maximumDistance, maxVolume, maxWeight, maxLines, 0, iterationsPerLC )
{
	mOptimumBeauties = 0;
}
		
// We evaluate the current configuration on how good it is with respect to maximize the number of beauties
void LCAAnnealCriteriumSFD::evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues )
{
	// We first run the update values method, to calculate all relevent values to make a decision
	updateValues(rcLoadCarriers);

	double uglyVolume = mMaxVolume;

	// We determine the smallest amount of ugly volume over all load carriers that are not a beauty
	// The smaller this volume is, the bigger the change we can create a beauty by removing this 
	// volume and placing it in another load carrier
	// The ugly volume is the total volume of the load carier minus the volume of the product category with 
	// the largest volume.
	for ( int i = 0; i < rcLoadCarriers.size(); i++ )
	{
		if ( !rcLoadCarriers[i].isBeauty() )
		{
			double uglyVolumeLC = rcLoadCarriers[i].getUglyVolume();
			if ( uglyVolumeLC < uglyVolume ) uglyVolume = uglyVolumeLC;
		}
	}

	// The value is composed of the number of beauties (negative, since we are minimizing), the amount of ugly volume
	// compared to the total amount of volume for one load carrier and a tiebreaker on distance.
	// To this value we add the number of resultlines, since this reduces the number of SKUs that are divided over 
	// multiple load carriers
	mValue = (10 * (-1 *mBeautyCount + ( uglyVolume / mMaxVolume ))) + (mTotalDistance / mMaximumDistance);
	mValue *= 10;
	mValue += mResultLines;

	// At the start of the annealing this method will be called with updateStartValues = true
	// We then have to set the inial cool value and the number of iterations
	if ( updateStartValues )
	{
		mStartCool = 1;
		mMaxIterations = rcLoadCarriers.size() * mIterationsPerLC;
		calcOptimumBeauties( rcLoadCarriers );
	}

	mFinished = ( mBeautyCount == mOptimumBeauties ) && mValid;

}

// This method calculates the maximum number of beauties that is possible for a given order
// It is used to stop the beauty phase of the load carrier algorithm as soon as the optimum
// is reached. This is done to speed up the algorithm
// Note: this method gives an upperbound. It may not always be possible to create this many
// beauties, but it sure is not possible to create more
void LCAAnnealCriteriumSFD::calcOptimumBeauties( std::vector< LCAAnnealLC >& rcLoadCarriers )
{
	double totalVolume = 0;
	std::map< QString, double > cCatgroupToVolume;
	mOptimumBeauties = 0;


	// We calculate the total volume of the pickbatch
	for ( int i=0; i<rcLoadCarriers.size(); i++ )
	{
		for ( AnnealLineSet::const_iterator iLine = rcLoadCarriers[i].beginLines(); iLine != rcLoadCarriers[i].endLines(); ++iLine )
		{
			cCatgroupToVolume[(*iLine)->getCatGroup()] += (*iLine)->getTotalVolume();
		}

		totalVolume += rcLoadCarriers[i].getTotalVolume();
	}

	// We remove the volume of the largest categorygroup ( up to a maximum of one load carrier at a time )
	// We assume we can put all these items in one load carrier and make it into a beauty. The remaining
	// volume will have to fit in the remaining load carriers (i.e. the current number of load carriers minus
	// the number of load carriers already fixed). 
	while ( true )
	{
		std::map< QString, double>::iterator iLargest = cCatgroupToVolume.end();
		double largestVolume = 0;

		// Look for the largest productcategory
		for ( std::map< QString, double>::iterator iCat = cCatgroupToVolume.begin(); iCat != cCatgroupToVolume.end(); ++iCat )
		{
			if ( iCat->second > largestVolume )
			{
				largestVolume = iCat->second;
				iLargest = iCat;
			}
		}

		// If no category is left, we are done
		if ( iLargest == cCatgroupToVolume.end() )
		{
			break;
		}

		// If there is more than one full load carrier of volume available in this category, we 
		// take the volume of one load carrier, otherwise we take the remaining volume
		if ( largestVolume > mMaxVolume )
		{
			cCatgroupToVolume[iLargest->first] -= mMaxVolume;
			largestVolume = mMaxVolume;
		}
		else
		{
			cCatgroupToVolume.erase(iLargest);
		}

		// The total volume is reduced by the volume that we are going to assign
		totalVolume -= largestVolume;

		// We check if we can make a beauty with the volume that we want to assign and still have enough
		// space for the other items in the remaining load carriers. If this is the case, then we assign
		// the selected volume to a "beauty" load carrier and continue;
		// If not, we are done and cannot make any more beauties
		if ( mOptimumBeauties + 1  + ceil( totalVolume / mMaxVolume ) <= rcLoadCarriers.size() )
		{
			mOptimumBeauties++;
		}
		else
		{
			break;
		}
	}
}

// This is the criterium used for minimizing distance
LCAAnnealCriteriumMinimizeDist::LCAAnnealCriteriumMinimizeDist( double maximumDistance, double maxVolume, double maxWeight, 
	int maxLines, int minBeauties, int iterationsPerLC  )
:LCAAnnealCriterium( maximumDistance, maxVolume, maxWeight, maxLines, minBeauties, iterationsPerLC )
{
	mMaxResultLines = 0;
}

// We evaluate the current configuration on how good it is with respect to minimizing the distance travelled by the LC
void LCAAnnealCriteriumMinimizeDist::evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues )
{
	// We first run the update values method, to calculate all relevent values to make a decision
	updateValues(rcLoadCarriers);

	// We determine the volume of the aisle-loadcarrier combination that has the smallest volume
	// By moving the volume that that load carrier has to do in that aisle, to another load carrier
	// We can eliminate the need for the load carrier to visit that aisle and thus reduce the total
	// distance. This is used as a tie-breaker when the actual distance is eqaul
	double aisleVolume = mMaxVolume;

	for ( int i = 0; i < rcLoadCarriers.size(); i++ )
	{
		if ( rcLoadCarriers[i].getTotalVolume() > gcLCAEpsilon )
		{
			double aisleVolumeLC = rcLoadCarriers[i].getSmallestAisleVolume();
			if ( aisleVolumeLC < aisleVolume ) aisleVolume = aisleVolumeLC;
		}
	}

	mValue = (100*mTotalDistance/mMaximumDistance) + aisleVolume/( rcLoadCarriers.size()*mMaxVolume );

	mFinished = false;

	// At the start of the annealing this method will be called with updateStartValues = true
	// We then have to set the inial cool value and the number of iterations
	if ( updateStartValues )
	{
		mStartCool = 3.0/rcLoadCarriers.size();
		mMaxIterations = rcLoadCarriers.size() * mIterationsPerLC;
		mMaxResultLines = mResultLines;
	}

	// We only accept a change if it does not lead to an increase in the number of SKUs that are divided over 
	// multiple load carriers.
	mValid = mValid & ( mResultLines <= mMaxResultLines );
	if ( mValid && mResultLines < mMaxResultLines ) 
	{
		mMaxResultLines = mResultLines;
	}
}
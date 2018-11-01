#include "lcaanneal.h"
#include "lcapickbatch.h"
#include "lcapickbatchline.h"
#include "lcaparameters.h"
#include "lcaresult.h"
#include "lcaresultline.h"
#include "lcaerrors.h"
#include <math.h>
#include <QtCore/qdatetime.h>


// An LCAAnnealChangeItem stores a change made by the annealing algorithm. When the annealing goes from one solution
// to another solution, it stores all changes made in a vector. The changes can than be reverted by performing the 
// opposite changes in the reverse order.
// The change is stored in the most basic form: the addition or removal of a line from a loadcarrier. A real move
// in the anneal algorithm is composed of a series of these changes
LCAAnnealChangeItem::LCAAnnealChangeItem( int loadCarrierIndex, const LCAAnnealPickBatchLine* pLine, bool isAdd )
:mpLine( pLine )
{
	mLoadCarrierIndex = loadCarrierIndex;
	mIsAdd = isAdd;
}

// Initializes the anneal algorithm for processing a pick batch. It initializes the random generator with a fixed number
// such that running the algorithm twice with exactly the same settings, will result in the same output.
LCAAnneal::LCAAnneal( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters, const LCAWarehouse& rWarehouse )
:mrWarehouse( rWarehouse )
{
	random.initSeed( 481 );

	mpParameters = pParameters;
	mpPickbatch = pPickbatch;

	std::pair< double, double > offsetStrek = rWarehouse.getAreaOffset( pPickbatch->getStrekWsId() );
	mAdjustedXStrek = pPickbatch->getStrekXCoord() + offsetStrek.first;
	mAdjustedYStrek = pPickbatch->getStrekYCoord() + offsetStrek.second;
}

// The actual simulated annealing algorithm is performed a number of times, with different criteria
// This method makes sure that for the pick batch all annealing steps are performed.
// We currently do the following steps:
// 1. Optimize the number of load carriers, without splitting any additional pick order lines
// 2. Optimize the number of load carriers, allowing extra splitting of pick order lines
// 3. Optimize storefriendliness, without splitting any additional pick order lines
// 4. Optimize storefriendliness, allowing extra splitting of pick order lines
// 5. Optimize distance, allowing extra splitting of pick order lines
// 6. Optimize distance, without splitting any additional pick order lines
// 7. Optimize distance, without splitting any additional pick order lines
//
// lowerBoundLCs is the theoretical lower bound on the number of load carriers, it is not possible to make a solution with fewer lcs
// apteanDistance is the distance used by the solution that is constructed with the base algorithm
// result is the best result found so far, and is used as a start solution for the annealing
LCAResult* LCAAnneal::run( const int lowerBoundLCs, const double apteanDistance, LCAResult *pResult )
{
	// Based on the aptean distance and the parameter that gives the maximum deterioration of the distance,
	// calculate the maximum total distance that may be used for this store order.
	double maximumDistance = apteanDistance * mpParameters->getDistanceFactor();
	
	LCAResult* pLocalResult = pResult;
	
	msTimeOuts.clear();

	if ( mpParameters->getDoLCPhase() )
	{
		// Step one 1. Optimize the number of load carriers, without splitting any additional pick order lines
		// First build a load carrier vector based one the previous result. ( The load carrier vector stores the solution in a way suitable for annealing )
		buildLoadCarrierVector( pLocalResult, true /*Forbid splitting lines, even if the parameters allow it*/ );

		pLocalResult = runWithCriterium( LCAAnnealCriteriumMinimizeLCs( lowerBoundLCs, maximumDistance, mpParameters->getMaxLDVol(), 
			mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), mpParameters->getIterationsLC() ), lowerBoundLCs*mpParameters->getMaxmSecsLC()/2.0 );

	
		// 2. Optimize the number of load carriers, allowing extra splitting of pick order lines
		// Build a load carrier vector based on the solution of step 1
		buildLoadCarrierVector( pLocalResult, false  /*Follow parameters for splitting lines*/ );
		delete pLocalResult;
		pLocalResult = 0;
		pLocalResult = runWithCriterium( LCAAnnealCriteriumMinimizeLCs( lowerBoundLCs, maximumDistance, mpParameters->getMaxLDVol(), 
												mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), mpParameters->getIterationsLC() ), 
												lowerBoundLCs*mpParameters->getMaxmSecsLC()/2.0  );
	}

	// Twn
	//pLocalResult->calculatePickDistance( mrWarehouse, false, mpPickbatch->getStrekWsId(), mpPickbatch->getStrekXCoord(), mpPickbatch->getStrekYCoord(), true );
	//maximumDistance = pLocalResult->getTotalDistance();

	if ( mpParameters->getDoBeautyPhase() )
	{
		// 3. Optimize storefriendliness, without splitting any additional pick order lines
		buildLoadCarrierVector( pLocalResult, true /*Forbid splitting lines, even if the parameters allow it*/ );
		delete pLocalResult;
		pLocalResult = 0;
		pLocalResult = runWithCriterium( LCAAnnealCriteriumSFD( maximumDistance, mpParameters->getMaxLDVol(), 
												mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), mpParameters->getIterationsBeauty() ), 
												lowerBoundLCs*mpParameters->getMaxmSecsBeauty()/2.0  );
	
		// 4. Optimize storefriendliness, allowing extra splitting of pick order lines
		buildLoadCarrierVector( pLocalResult, false  /*Follow parameters for splitting lines*/ );
		delete pLocalResult;
		pLocalResult = 0;
		pLocalResult = runWithCriterium( LCAAnnealCriteriumSFD( maximumDistance, mpParameters->getMaxLDVol(), 
												mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), mpParameters->getIterationsBeauty() ), 
												lowerBoundLCs*mpParameters->getMaxmSecsBeauty()/2.0 );
	}

	// We optimize the distance. Note that the steps for allowing and not allowing extra splits are the other way around this time
	if ( mpParameters->getDoDistPhase() )
	{
		// 5. Optimize distance, allowing extra splitting of pick order lines
		// Set the minimum required amount of beauties in a solution equal to the best number found so far
		int minimumBeauties = mpParameters->getDoBeautyPhase()?pLocalResult->getBeautyCount():0;
		buildLoadCarrierVector( pLocalResult, false /*Follow parameters for splitting lines*/ );
		delete pLocalResult;
		pLocalResult = 0;
		pLocalResult = runWithCriterium( LCAAnnealCriteriumMinimizeDist( maximumDistance, mpParameters->getMaxLDVol(), 
												mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), minimumBeauties, mpParameters->getIterationsDistance() ), 
												lowerBoundLCs*mpParameters->getMaxmSecsDistance()/3.0 );


		// 6. Optimize distance, without splitting any additional pick order lines
		minimumBeauties =  mpParameters->getDoBeautyPhase()?pLocalResult->getBeautyCount():0;
		buildLoadCarrierVector( pLocalResult, true /*Forbid splitting lines, even if the parameters allow it*/ );
		delete pLocalResult;
		pLocalResult = 0;
		pLocalResult = runWithCriterium( LCAAnnealCriteriumMinimizeDist( maximumDistance, mpParameters->getMaxLDVol(), 
			mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), minimumBeauties, mpParameters->getIterationsDistance() ), 
			lowerBoundLCs*mpParameters->getMaxmSecsDistance()/3.0 );

		// 7. Optimize distance, without splitting any additional pick order lines
		//    In the previous step, orderlines may have been put in the same load carrier and can now be merged
		//    This makes the switches in this step more effective
		buildLoadCarrierVector( pLocalResult, true /*Forbid splitting lines, even if the parameters allow it*/ );
		delete pLocalResult;
		pLocalResult = 0;
		pLocalResult = runWithCriterium( LCAAnnealCriteriumMinimizeDist( maximumDistance, mpParameters->getMaxLDVol(), 
			mpParameters->getMaxLDWgt(), mpParameters->getMaxPBRowCar(), minimumBeauties, mpParameters->getIterationsDistance() ),
			lowerBoundLCs*mpParameters->getMaxmSecsDistance()/3.0 );
	}

	for ( int timeoutIdx = 0; timeoutIdx < msTimeOuts.size(); timeoutIdx++ )
	{
		pLocalResult->addError( gcLCATimeout, msTimeOuts[timeoutIdx] );
	}

	return pLocalResult;
}

// Initialize load carrier vector based on a previous solution
void LCAAnneal::buildLoadCarrierVector( const LCAResult* pResult, bool forbidSplit )
{
	// First clean up the current load carrier vector
	mcLoadCarriers.clear();

	// We create a mapping from PBRowID to pickbacthline, for easier lookup 
	long uniqueId = 0;
	std::map< QString, const LCAPickBatchLine* > cPBLines;
	for ( long i=0; i<mpPickbatch->getPickLineCount(); i++ )
	{
		const LCAPickBatchLine* pPBLine = mpPickbatch->getPickLine(i);
		cPBLines[pPBLine->getPBRowId()] = pPBLine;
	}
	
	// We create a mapping from the virtual carId to an index position for the LCAAnnealLC that will represent the load carrier
	// in the vector of load carriers. We make sure the vector of load carriers is large enough to contain all load carriers
	// in the start solution, but no more. This means by default that the annealing algorithm will not be able to use more
	// load carriers than the start solution
	std::map< QString, int>  cCarIdToIdx;
	mcLoadCarriers.resize( pResult->getLoadCarrierCount() );

	// We fill the LCAnnealLC's with the pick lines that they have in the start solution
	// If splitting of pick batch lines is allowed, we make an LCAAnnealPickBatchLine for every individual unit
	// otherwise we make such a line for every pick batch line. If the start solution has more than one pick batch
	// line for one pbrow, than this split will remain in the initial vector. 
	for ( long i=0; i<pResult->getResultLineCount(); i++ )
	{
		// We process the lines in the solution one by one.
		const LCAResultLine* pLine = pResult->getResultLine(i);
		double ordQty = pLine->getOrdQty();

		// We make as many lcaannealpickbatchlines as there are indivudual items in de ordQty
		// If ordQty is not an integer number, we divide it such that all lines contain 1 unit
		// except for the last line, which contains the remainder. So an ordQty of 3,21 will be
		// split in 3 lines with ordQty 1 and 1 line with ordQty 0,21
		long splitCount = ceil( ordQty );
		
		// If splitting the lines is forbidden, either by the user, or because of the current step in tha annealling,
		// we only make one split batch, containing the full ordQty
		if ( (!mpParameters->getAllowPBRowSplit()) || forbidSplit  ) splitCount = 1;

		// Als long as we have splits to make, we make a split of size one, except when it is the last split, then 
		// it gets the remainder. This remainder can be of any size, depending on if splitting was allowed or not
		while ( splitCount > 0 )
		{
			// We make lines with ordQty 1
			double ordQtyForLine = 1;
			if ( splitCount == 1 )
			{	
				// Except for the last line, which gets the remaineder
				ordQtyForLine = ordQty;
			}
			// And we update the ordQty that we still have to assign.
			ordQty -= ordQtyForLine;


			// We lookup the corresponding pichbatch line to get additional information, that we need in the algorithm
			const LCAPickBatchLine* pPickbatchLine = cPBLines[ pLine->getPBRowId() ];

			// And create a new LCAAnnealPickBatchLine based on the pickbatchline and the adjusted ordQty for this line
			// Each line is assigned a unique id, so it can be put in sets without duplicate key problems
			LCAAnnealPickBatchLine* pAnnealLine = new LCAAnnealPickBatchLine( pPickbatchLine, ordQtyForLine, uniqueId, mrWarehouse );
			
			// Then we lookup the index of the LCAAnnealLoadCarrier that represents the LC this line must be assigned to
			std::map< QString, int > ::iterator iCarId = cCarIdToIdx.find(  pLine->getPBCarIdVirtual()  );
			if ( iCarId == cCarIdToIdx.end() ) 
			{
				long count = cCarIdToIdx.size();
				cCarIdToIdx[pLine->getPBCarIdVirtual()] = count;
			}

			// And add the line
			mcLoadCarriers[ cCarIdToIdx[pLine->getPBCarIdVirtual()] ].insertLine( pAnnealLine, mrWarehouse, 
				mAdjustedXStrek, mAdjustedYStrek );

			uniqueId++;
			splitCount--;
		}
	}
}

// This method is the work-horse of the annealing. It performs a full run of simulated annealing using the 
// supplied criterium for evaluation
LCAResult* LCAAnneal::runWithCriterium( LCAAnnealCriterium& rCriterium, double maxRunTime )
{
	QDateTime startTime = QDateTime::currentDateTime();

	// We start by evaluating the start solution, so we know the inital values for the annealing
	long iteration = 0;
	rCriterium.evaluate( mcLoadCarriers, true /* Update start values */ );
	
	
	// We store the current values of all indicators involved in the annealing process
	double currentValue = rCriterium.value();					// The current anneal value, used to accept or reject a change
	int currentLC = rCriterium.loadCarriers();					// The current number of load carriers
	int currentBeauties = rCriterium.beauties();				// The current number of beauties
	double currentDistance = rCriterium.distancePercentage();	// The current distance
	int currentResultLines = rCriterium.resultLines();			// The current number of pickbatchlines

	// The current solution is also the best solution, so we initialize the indicators for the best
	// solution with the same values
	int bestLC = currentLC;
	int bestBeauties = currentBeauties;
	double bestDistance = currentDistance;
	double bestValue = currentValue;
	int bestResultLines = currentResultLines;

	// We set the initial cool parameter, that is used to accept or reject a change. This value depends on the criterium
	double cool = rCriterium.startCool();

	// Before we start the annealing, we first check if the current solution is not already the optimal solution with respect
	// to the current criterium
	bool finished = rCriterium.finished();

	// We store the current configuration of the load carrier vector as the best solution. If we dont find a better one in the
	// annealing proces, we can return this one.
	LCAResult* pSolution = createSolution();
	
	
	if ( !finished )
	{
		/*
		fprintf( mpAnnealFile, "========================================================================================================\n" );
		fprintf( mpAnnealFile, rCriterium.name().toAscii().data() );
		fprintf( mpAnnealFile, "    Starting anneal for %s at value %10.5lf\n", mpPickbatch->getNewGroupId().toAscii().data(), currentValue );
		fprintf( mpAnnealFile, rCriterium.name().toAscii().data() );
		fprintf( mpAnnealFile, "    Starting at    %12.5lf     LC: %2ld    Beauty: %2ld    Dist: %6.2lf    Fill: %6.2lf    Lines: %5ld\n", 
					currentValue, rCriterium.loadCarriers(), rCriterium.beauties(), rCriterium.distancePercentage(),
					rCriterium.fillRateSmallestLC(), rCriterium.resultLines() );
		fprintf( mpAnnealFile, "========================================================================================================\n" );
		fflush( mpAnnealFile );
		*/
	}
	else
	{
		/*
		fprintf( mpAnnealFile, "========================================================================================================\n" );
		fprintf( mpAnnealFile, rCriterium.name().toAscii().data() );
		fprintf( mpAnnealFile, "    Already optimal for %s at value %10.5lf\n", mpPickbatch->getNewGroupId().toAscii().data(), currentValue );
		fprintf( mpAnnealFile, "========================================================================================================\n" );
		fflush( mpAnnealFile );
		*/
	}

	// As long as we have not found the optimal solution and we have not reached the maximum of iterations that we are allowed to do
	// we do another iteration
	while ( iteration < rCriterium.maxIterations() && !finished )
	{
		iteration++;

		// We first make a change
		ChangeVector changes;
		doChange( changes, rCriterium );

		// And then evaluate the solution after the change
		rCriterium.evaluate( mcLoadCarriers, false /* Dont update start values */ );
		
		// We get the value, so we can compare it with the current result and we check if the new solution is valid
		double valueAfterChange = rCriterium.value();
		bool validSolution = rCriterium.valid();


		// If the new solution is not valid, then we always reject it and undo the change. 
		if ( !validSolution )
		{
			// If it is not valid, we always undo it
			undoChange( changes );
		}
		// If the solution is valid and it is either better, or not too much worse, we will accept it
		else if ( ( valueAfterChange < currentValue ) // It's an improvement in value
			    ||
			      ( exp( (currentValue - valueAfterChange ) / cool ) > random.draw() ) // It's an acceptable deterioration
			    )
		{			

			// Since we have a new current solution, we update the indicators of the current solution
			currentValue = valueAfterChange;
			currentBeauties = rCriterium.beauties();
			currentLC = rCriterium.loadCarriers();
			currentDistance = rCriterium.distancePercentage();
			currentResultLines = rCriterium.resultLines();

			if ( currentValue + gcLCAEpsilon < bestValue )
			{
				bestValue = currentValue;
			}
		}
		// If the solution is valid, but worse and non acceptable
		else // It is not better and not acceptable
		{
			undoChange( changes );
		}
		

		// We check if the current solution is an improvement over the best solution we have found so far. For this, we dont use the value, since this contains elements
		// that are only used to steer the annealing. We check this agains the real-world objective
		// 1. Minimize LCS
		// 2. Maximize Beauties ( skipped if beautyphase is skipped )
		// 3. Minimize Resultlines
		// 4. Minimize Distance ( we add epsilon to avoid rounding problems when comparing doubles )
		if  (    ( currentLC < bestLC )
			|| ( ( currentLC == bestLC ) && ( currentBeauties > bestBeauties ) && mpParameters->getDoBeautyPhase() )
			|| ( ( currentLC == bestLC ) && ( ( currentBeauties == bestBeauties ) || !mpParameters->getDoBeautyPhase() ) && ( currentResultLines < bestResultLines ) )
			|| ( ( currentLC == bestLC ) && ( ( currentBeauties == bestBeauties ) || !mpParameters->getDoBeautyPhase() ) && ( currentResultLines == bestResultLines ) && ( currentDistance + gcLCAEpsilon < bestDistance ) ) )
		{
			//fprintf( mpAnnealFile, rCriterium.name().toAscii().data() );
			//fprintf( mpAnnealFile, "*** Found solution %12.5lf     LC: %2ld    Beauty: %2ld    Dist: %6.2lf    Fill: %6.2lf    Lines: %5ld => STORED\n", 
			//	currentValue, currentLC, currentBeauties, currentDistance,
			//	rCriterium.fillRateSmallestLC(), currentResultLines );

			//fflush( mpAnnealFile );

			// The current solution is the best one so far, so we update the indicators for the best solution
			bestLC = currentLC;
			bestBeauties = currentBeauties;
			bestDistance = currentDistance;
			bestResultLines = currentResultLines;

			// We delete the previous best solution
			if ( pSolution ) delete pSolution;
			// And save the current solution
			pSolution = createSolution();
		}

		// We check if the current solution is optimal and stop the annealing if this is the case
		if ( rCriterium.finished() ) 
		{
			//fprintf( mpAnnealFile, rCriterium.name().toAscii().data() );
			//fprintf( mpAnnealFile, "*** Found OPTIMUM\n" );
			finished = true;
			break;
		}
		

		// The chances of accepting a deterioration of the solution must get smaller when we get closure to 
		// the end of the annealing loop. The cool parameter steers the chance of accepting a solution that 
		// is worse than the current one. The smaller this cool parameter gets, the smaller the chance a 
		// deterioration will be accepted. We lower this parameter in 500 steps
		if ( iteration % ( rCriterium.maxIterations() / 500 ) == 0 ) 
		{
			cool *= 0.99;

			// We check if we still have time left. If not, we stop
			if ( startTime.msecsTo( QDateTime::currentDateTime() ) >= maxRunTime )
			{
				msTimeOuts.append( QString( "Pickbatch %1 timed out after \"%2\" percent in phase \"%3\"" ) //THRO (omitted first parameter)
					.arg( mpPickbatch->getNewGroupId() )
					.arg( (100.0*iteration)/rCriterium.maxIterations() )
					.arg( rCriterium.name() ) );
				//fprintf( mpAnnealFile, rCriterium.name().toAscii().data() );
				//fprintf( mpAnnealFile, "*** Timed out\n" );
				finished = true;
				break;
			}
		}
	}


	//fflush( mpAnnealFile );

	// We return the best solution found
	return pSolution;
}

// Make a modification to the current solution and return all changes made to create this
// modification in a changevector. The changevector can be used to undo the modifications
// if they do not lead to an acceptable solution
void LCAAnneal::doChange( ChangeVector& rChanges, const LCAAnnealCriterium& rCriterium )
{
	// We randomly determine the type of modification we will make
	int doType = random.draw( 0, 6 );


	if ( doType == 0 ) 
	{
		// Move a random line from a random load carrier to another load carrier
		doChangeMove( rChanges, rCriterium, false );
	}
	else if ( doType == 1 )
	{
		// Move a line from a load carrier to another load carrier, trying to do a smart selection 
		// of line and load carriers
		doChangeMove( rChanges, rCriterium, true );
	}
	else if ( doType <= 3 )
	{
		// Exchange two lines between load carriers, trying to select the first line and load carrier smart
		doChangeExchange( rChanges, rCriterium, true );
	}
	else if ( doType <= 5 ) 
	{
		// Exchange two lines between load carriers, randomly
		doChangeExchange( rChanges, rCriterium, false );
	}
	else
	{
		// Try to move multiple lines from one load carrier to one or more other load carriers
		// The lines that will be moved are selected such that the total volume to be moved is
		// not too big and the expected gain is good.
		doChangeMoveMany( rChanges, rCriterium );
	}
}
	
// This method will revert the changes given in the changevector. So we can undo the last change
void LCAAnneal::undoChange( ChangeVector &rChanges )
{
	// We go over all changes and undo them in reverse order of the order in which they were done.
	// This automatically updates the optimization values in the load carrier
	while ( !rChanges.empty() )
	{
		LCAAnnealChangeItem& rChange = rChanges.back();

		if ( rChange.isAdd() )
		{
			// If we added a line to do the change, than we have to remove the line to undo it
			mcLoadCarriers[rChange.getLoadCarrierIndex()].removeLine( rChange.getLine(), mrWarehouse, 
				mAdjustedXStrek, mAdjustedYStrek );
		}
		else
		{
			// If we removed a line to do the change, than we have to add the line to undo it
			mcLoadCarriers[rChange.getLoadCarrierIndex()].insertLine( rChange.getLine(), mrWarehouse, 
				mAdjustedXStrek, mAdjustedYStrek );
		}

		rChanges.pop_back();
	}
}

// Exchange two lines between load carriers
// If the argument doBestOption is true and the criterium is for distance, then try to select the line smart
void LCAAnneal::doChangeExchange( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium, bool doBestOption )
{
	// If we have just one load carrier, then we cannot exchange between load carriers
	if ( mcLoadCarriers.size() <= 1 ) return;

	// Select a random load carrier
	int loadCarrier1 = random.draw( 0, mcLoadCarriers.size()-1 );
	
	// Except when the criterium is for distance and we are trying to do a smart selection,
	// Then with 25% chance, we select the load carrier that is closest to having a removable aisle
	// We loop over all load carriers and select the load carrier that has the smallest volume ailse of all
	if ( doBestOption && rCriterium.name() == "Dist" && random.draw() < 0.25 )
	{
		double smallestAisleVolume = mpParameters->getMaxLDVol();

		for ( int i=0; i<mcLoadCarriers.size(); i++ )
		{
			if ( mcLoadCarriers[i].getSmallestAisleVolume() < smallestAisleVolume && mcLoadCarriers[i].getSmallestAisleVolume() > gcLCAEpsilon  )
			{
				smallestAisleVolume = mcLoadCarriers[i].getSmallestAisleVolume();
				loadCarrier1 = i;
			}
		}
	}

	// Select a line from this load carrier. When we optimize for distance and go for the best option, we select
	// a line that is part of the smallest volume aisle of the load carrier.
	// In all other cases we select a random line.
	const LCAAnnealPickBatchLine *pLine1 = 0;
	if ( doBestOption && rCriterium.name() == "Dist" ) 
	{
		pLine1 = mcLoadCarriers[loadCarrier1].selectRandomMinorityAisleLine( random );
	}
	else
	{
		pLine1 = mcLoadCarriers[loadCarrier1].selectRandomLine( random ); 
	}

	// There is a possibility that we have not selected a line, for example when the load carrier has no lines in case we use 
	// less load carriers than the original algorithm did.
	if ( !pLine1 ) return;


	// Select a random second load carrier
	int loadCarrier2 = random.draw( 0, mcLoadCarriers.size()-1 );
	
	// Unless we want to make a smart move for distance, then we move it to a load carrier that already has to visit the aisle
	// of the lane. To allow for some randomness, we start searching for this load carrier at position loadCarrier2 instead of 0, 
	// so we dont always select the same one.
	bool found = false;
	if ( doBestOption && rCriterium.name() == "Dist" )
	{
		for ( int i=0; i<mcLoadCarriers.size(); i++ )
		{
			int loadCarrierTo = (loadCarrier2 + i)%mcLoadCarriers.size();
		
			// It makes no sense inserting in the same load carrier it came from
			if ( loadCarrierTo == loadCarrier1 ) continue;

			// It makes no sense inserting in an empty load carrier, since that increases the number of load carriers and will always be rejected
			if ( mcLoadCarriers[loadCarrierTo].getTotalVolume() < gcLCAEpsilon ) continue;
		
			// If we minimize distance and search for the best fit loadCarrier, we first try to fit it in a load carrier
			// that already has to visit the aisle we need to visit.
			// Only if no such load carrier exists, we try another one
			if (!mcLoadCarriers[loadCarrierTo].containsAisle( pLine1->getAisle()) ) continue;

			loadCarrier2 = loadCarrierTo;
			break;
		}
	}

	// It makes no sense to assign the line to the load carrier it already is in. So, in that
	// case we select the next one
	if ( loadCarrier2 == loadCarrier1 ) 
	{
		loadCarrier2 = ( loadCarrier2 + 1 )%mcLoadCarriers.size();
	}

	// Select a random line from the second load carrier
	const LCAAnnealPickBatchLine *pLine2 = mcLoadCarriers[loadCarrier2].selectRandomLine( random );

	// If no such line exists, we cannot exchange, so we stop
	if ( !pLine2 ) return;

	// Quickly check for volume and weight, since if these limits are not ok, the exchange will fail.
	// This is done to make a speed up, since doing the actual exchange takes a lot more time than this check
	if ( mcLoadCarriers[loadCarrier1].getTotalVolume() + pLine2->getTotalVolume() - pLine1->getTotalVolume() > mpParameters->getMaxLDVol() ) return;
	if ( mcLoadCarriers[loadCarrier2].getTotalVolume() - pLine2->getTotalVolume() + pLine1->getTotalVolume() > mpParameters->getMaxLDVol() ) return;
	if ( mcLoadCarriers[loadCarrier1].getTotalWeight() + pLine2->getTotalWeight() - pLine1->getTotalWeight() > mpParameters->getMaxLDWgt() ) return;
	if ( mcLoadCarriers[loadCarrier2].getTotalWeight() - pLine2->getTotalWeight() + pLine1->getTotalWeight() > mpParameters->getMaxLDWgt() ) return;


	// Finally, do the actual exchange
	mcLoadCarriers[loadCarrier1].removeLine( pLine1, mrWarehouse, 
		mAdjustedXStrek, mAdjustedYStrek );
	mcLoadCarriers[loadCarrier2].insertLine( pLine1, mrWarehouse, 
		mAdjustedXStrek, mAdjustedYStrek );

	mcLoadCarriers[loadCarrier2].removeLine( pLine2, mrWarehouse, 
		mAdjustedXStrek, mAdjustedYStrek );
	mcLoadCarriers[loadCarrier1].insertLine( pLine2, mrWarehouse, 
		mAdjustedXStrek, mAdjustedYStrek );

	// And prepare the undo vector
	rChanges.push_back( LCAAnnealChangeItem( loadCarrier1, pLine1, false /* Removed */) );
	rChanges.push_back( LCAAnnealChangeItem( loadCarrier2, pLine2, false /* Removed */ ) );
	rChanges.push_back( LCAAnnealChangeItem( loadCarrier1, pLine2, true /* Added */) );
	rChanges.push_back( LCAAnnealChangeItem( loadCarrier2, pLine1, true /* Added */ ) );
}

// Move a line from one load carrier to another
// If forceBest is true, we try a line that is good for reaching the criterium
void LCAAnneal::doChangeMove( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium, bool forceBest )
{

	int loadCarrierFrom = 0;
	double smallestVolume = mpParameters->getMaxLDVol();
	double smallestUglyVolume = mpParameters->getMaxLDVol();
	double smallestAisleVolume = mpParameters->getMaxLDVol();
	
	// If the criterium is minimizing the number of load carriers, then we select the load carrier that has
	// the smallest volume left. (For best option moves only)
	if ( forceBest && rCriterium.name() == "#LCs" )
	{
		for ( int i=0; i<mcLoadCarriers.size(); i++ )
		{
			if ( mcLoadCarriers[i].getTotalVolume() < smallestVolume )
			{
				smallestVolume = mcLoadCarriers[i].getTotalVolume();
				loadCarrierFrom = i;
			}
		}
	}
	// If the criterium is storefriendliness, then we select the load carrier that has the lowest amount
	// of ugly volume, i.e., the lowest amount of volume that keeps it from being a beauty with only lines
	// for one productcategorygroup (For best option moves only)
	else if ( forceBest && rCriterium.name() == "SFD " )
	{
		for ( int i=0; i<mcLoadCarriers.size(); i++ )
		{
			if ( mcLoadCarriers[i].getUglyVolume() < smallestUglyVolume && mcLoadCarriers[i].getUglyVolume() > gcLCAEpsilon  )
			{
				smallestUglyVolume = mcLoadCarriers[i].getUglyVolume();
				loadCarrierFrom = i;
			}
		}
	}
	// If the criterium is distance, then we select with chance 25% a the load carrier that has the aisle with the smallest volume
	// (For best option moves only)
	else if ( forceBest && (rCriterium.name() == "Dist") && (random.draw() < 0.25) )
	{
		for ( int i=0; i<mcLoadCarriers.size(); i++ )
		{
			if ( mcLoadCarriers[i].getSmallestAisleVolume() < smallestAisleVolume && mcLoadCarriers[i].getSmallestAisleVolume() > gcLCAEpsilon  )
			{
				smallestAisleVolume = mcLoadCarriers[i].getSmallestAisleVolume();
				loadCarrierFrom = i;
			}
		}
	}
	// If we are not doing a smart move, we select a random load carrier
	else
	{
		loadCarrierFrom = random.draw( 0, mcLoadCarriers.size()-1 );
	}

	
	// We select a random line from the load carrier
	const LCAAnnealPickBatchLine*  pLine = mcLoadCarriers[loadCarrierFrom].selectRandomLine( random );

	// Unless we are optimizing store friendly delivery and are looking for a smart move. In that case
	// we select one of the lines from the "ugly" volume, i.e., the volume that does not belong to the 
	// largest productcategory in the load container
	if ( forceBest && rCriterium.name() == "SFD " )
	{
		pLine = mcLoadCarriers[loadCarrierFrom].selectRandomMinorityVolumeLine( random );
	}
	// When we look for a smart move for distance, we select a line from the least volumuous aisle of
	// the load carrier
	else if ( forceBest && rCriterium.name() == "Dist" )
	{
		pLine = mcLoadCarriers[loadCarrierFrom].selectRandomMinorityAisleLine( random );
	}

	// If no line is found, for example because the load carrier is empty, we quit.
	if ( !pLine ) return;
	
	// We remove the line from the load carrier
	mcLoadCarriers[loadCarrierFrom].removeLine( pLine, mrWarehouse, 
		mAdjustedXStrek, mAdjustedYStrek );

	// We will put it in a random other load carrier, provided that it probably will fit. For this
	// we select the first load carrier we are going to try and starting from that load carrier, we
	// keep trying the next one, until we find one that might be okay
	int loadCarrierToBase = random.draw( 0, mcLoadCarriers.size() - 1 );
	int loadCarrierTo = loadCarrierToBase;
	bool inserted = false;

	// We try the load carriers one by one, starting by loadCarrierToBase. We look at each load carrier at most once,
	// unless we are optimizing for distance. In that case we first try to fit the line in a load carrier that already
	// has to visit het aisle of the line, and only if no such load carrier exists, we consider the other options
	int max = mcLoadCarriers.size();
	if ( forceBest &&  (rCriterium.name() == "Dist") ) max = 2 * mcLoadCarriers.size();
	for ( int i=0; i<max; i++ )
	{
		loadCarrierTo = (loadCarrierToBase + i)%mcLoadCarriers.size();
		
		// It makes no sense inserting in the same load carrier it came from
		if ( loadCarrierTo == loadCarrierFrom ) continue;

		// It makes no sense inserting in an empty load carrier, since that increases the number of load carriers and will always be rejected
		if ( mcLoadCarriers[loadCarrierTo].getTotalVolume() < gcLCAEpsilon ) continue;
		
		// If we minimize distance and search for the best fit loadCarrier, we first try to fit it in a load carrier
		// that already has to visit the aisle we need to visit.
		// Only if no such load carrier exists, we try another one
		if (    forceBest 
			&& (rCriterium.name() == "Dist") 
			&& (!mcLoadCarriers[loadCarrierTo].containsAisle( pLine->getAisle()) 
			&& (i<mcLoadCarriers.size() ) ) ) continue;

		// It makes no sense inserting, if we already know we will exceed some limits by doing that
		if ( mcLoadCarriers[loadCarrierTo].getTotalVolume() + pLine->getTotalVolume() > mpParameters->getMaxLDVol() ) continue;
		if ( mcLoadCarriers[loadCarrierTo].getTotalWeight() + pLine->getTotalWeight() > mpParameters->getMaxLDWgt() ) continue;

		// We can try it now
		mcLoadCarriers[loadCarrierTo].insertLine( pLine, mrWarehouse, mAdjustedXStrek, mAdjustedYStrek );

		// It can still fail on the number of pbrows. All other checks will be done by the evaluate method
		if ( mcLoadCarriers[loadCarrierTo].getPBRowCount() > mpParameters->getMaxPBRowCar() )
		{
			mcLoadCarriers[loadCarrierTo].removeLine( pLine, mrWarehouse, 
				mAdjustedXStrek, mAdjustedYStrek );
		}
		else
		{
			inserted = true;
			break;
		}
	}

	// If no suitable load carrier is found, we put it back in the load carrier it came from
	if ( !inserted )
	{
		loadCarrierTo = loadCarrierFrom;
		mcLoadCarriers[loadCarrierTo].insertLine( pLine, mrWarehouse, 
			mAdjustedXStrek, mAdjustedYStrek );
	}

	// And we register the change
	rChanges.push_back( LCAAnnealChangeItem( loadCarrierFrom, pLine, false /* Removed */) );
	rChanges.push_back( LCAAnnealChangeItem( loadCarrierTo, pLine, true /* Added */ ) );
}

// We try to move multiple lines at a time
void LCAAnneal::doChangeMoveMany( ChangeVector &rChanges, const LCAAnnealCriterium& rCriterium )
{
	// We select a random load carrier
	int loadCarrierFrom = random.draw( 0, mcLoadCarriers.size()-1 );
	

	// With a 20% change we completely remove all lines from the smallest volume aisle in this load carrier
	// With a 80% change we remove a batch of lines that are not more than 10% of the total allowed volume and that 
	// will result in a reduction of distance for the load carrier
	int type = random.draw( 0, 4 );

	std::vector< const LCAAnnealPickBatchLine* > cLines;

	if ( type == 0 )
	{
		cLines = mcLoadCarriers[loadCarrierFrom].selectSmallAisle( random );
	}
	else
	{
		cLines = mcLoadCarriers[loadCarrierFrom].selectLongDistanceLines( random, mrWarehouse, mAdjustedXStrek, mAdjustedYStrek, mpParameters->getMaxLDVol());
	}

	// We try to insert the lines one-by-one into other load carriers. 
	int loadCarrierToBase = random.draw( 0, mcLoadCarriers.size() - 1 );
	for ( long line=0; line < cLines.size(); line++ )
	{
		// We first remove the line
		const LCAAnnealPickBatchLine* pLine = cLines[line];
		mcLoadCarriers[loadCarrierFrom].removeLine( pLine, mrWarehouse, mAdjustedXStrek, mAdjustedYStrek );

		// And then try to find a load carrier to put it in
		int loadCarrierTo = loadCarrierToBase;
		bool inserted = false;

		for ( int i=0; i< 2*mcLoadCarriers.size(); i++ )
		{
			loadCarrierTo = (loadCarrierToBase + i)%mcLoadCarriers.size();
		
			// It makes no sense inserting in the same load carrier it came from
			if ( loadCarrierTo == loadCarrierFrom ) continue;

			// It makes no sense inserting in an empty load carrier, since that increases the number of load carriers and will always be rejected
			if ( mcLoadCarriers[loadCarrierTo].getTotalVolume() < gcLCAEpsilon ) continue;
		
			// If we minimize distance and search for the best fit loadCarrier, we first try to fit it in a load carrier
			// that already has to visit the aisle we need to visit. Only if no such load carrier exists, we try another one
			if ( (!mcLoadCarriers[loadCarrierTo].containsAisle( pLine->getAisle() )) && ( i<mcLoadCarriers.size() ) ) continue;

			// It makes no sense inserting, if we already know we will exceed some limits by doing that
			if ( mcLoadCarriers[loadCarrierTo].getTotalVolume() + pLine->getTotalVolume() > mpParameters->getMaxLDVol() ) continue;
			if ( mcLoadCarriers[loadCarrierTo].getTotalWeight() + pLine->getTotalWeight() > mpParameters->getMaxLDWgt() ) continue;

			// We can try it now
			mcLoadCarriers[loadCarrierTo].insertLine( pLine, mrWarehouse, mAdjustedXStrek, mAdjustedYStrek );

			// It can still fail on the number of pbrows. All other checks will be done by the evaluate method
			if ( mcLoadCarriers[loadCarrierTo].getPBRowCount() > mpParameters->getMaxPBRowCar() )
			{
				mcLoadCarriers[loadCarrierTo].removeLine( pLine, mrWarehouse, mAdjustedXStrek, mAdjustedYStrek );
			}
			else
			{
				inserted = true;
				break;
			}
		}

		// If it is not inserted in another load carrier, put it back where it came from
		if ( !inserted )
		{
			loadCarrierTo = loadCarrierFrom;
			mcLoadCarriers[loadCarrierTo].insertLine( pLine, mrWarehouse, mAdjustedXStrek, mAdjustedYStrek );
		}

		// Register the change
		rChanges.push_back( LCAAnnealChangeItem( loadCarrierFrom, pLine, false /* Removed */) );
		rChanges.push_back( LCAAnnealChangeItem( loadCarrierTo, pLine, true /* Added */ ) );

		// If it is not inserted in another load carrier, skip the next lines, since it will not be possible
		// to move the whole set of lines anyway.
		if ( !inserted )
		{
			break;
		}
	}
}

// This method creates an LCAResult based on the current configuration of the load carrier vector
LCAResult* LCAAnneal::createSolution()
{
	// If a pbrow is split over multiple load carriers, it must get an additional split id
	// If a pbrow is not split, it may not have an additional split id
	// Therefor we first loop over all load carriers to see how many splits exist of each pbrow
	std::map< QString, int > cPBRowCount;
	for ( long i=0; i<mcLoadCarriers.size(); i++ )
	{
		QString sLastPBRow = "";

		for ( AnnealLineSet::iterator iLine = mcLoadCarriers[i].beginLines(); 
			iLine != mcLoadCarriers[i].endLines(); 
			++iLine )
		{
			const LCAAnnealPickBatchLine* pLine = *iLine;

			// Within a load carrier, we count a pbrow only once, since we will merge it anyway
			if ( sLastPBRow == pLine->getPBRowId() ) continue;

			sLastPBRow = pLine->getPBRowId();
			cPBRowCount[pLine->getPBRowId()]++;
		}
	}


	// We create the solution 
	LCAResult* pSolution = new LCAResult( mpPickbatch->getNewGroupId() );
	long loadCarrierId = 0;

	// We use this mapping to see which is the next split id that we should use for a splitted pbrow
	std::map< QString, int > cPBRowCurrent;
	
	// We process the load carriers one by one
	for ( long i=0; i<mcLoadCarriers.size(); i++ )
	{
		if ( mcLoadCarriers[i].beginLines() == mcLoadCarriers[i].endLines() )
		{
			// If the load carrier is empty, we skip it, so the numbering remains consecutive
			continue;
		}

		const LCAAnnealPickBatchLine* pPrevLine = 0;
		double ordQty = 0;

		// During the annealing process, the ordQty of the original PBRow can be split in parts of size 1
		// We want to merge the parts that ended up in the same load carrier, so we can return them as
		// one result line. We keep adding the ordQty of lines with the same PBROWId until we find that
		// the next line has anonther id. Then we store the full resultline
		// Since we have to store the last line as well, we continue till we are one step past the last line
		for ( AnnealLineSet::iterator iLine = mcLoadCarriers[i].beginLines(); true;  ++iLine )
		{

			// It the current line is for another pbrow id, or we have reached the end of the list
			if ( pPrevLine && ( (iLine == mcLoadCarriers[i].endLines())  || (pPrevLine->getPBRowId() != (*iLine)->getPBRowId() ) ) )
			{
				// Check if this pbrow was splitted, if so, assign the next free split id, if not, use an empty id
				QString sSplit = "";
				if ( cPBRowCount[pPrevLine->getPBRowId()] > 1 )
				{
					sSplit = QString::number( cPBRowCurrent[pPrevLine->getPBRowId()] );
					cPBRowCurrent[pPrevLine->getPBRowId()]++;
				}

				pSolution->addResultLine( pPrevLine->getPBRowId(), sSplit, ordQty, 
					QString::number(loadCarrierId), pPrevLine->getPickSeq(), pPrevLine->getCatGroup(), 
					pPrevLine->getWsId(), pPrevLine->getAisle(), pPrevLine->getXCord(), pPrevLine->getYCord(), pPrevLine->getWPAdr() );
				ordQty = 0;
			}
			
			// If we have reached the end of the line, stop
			if ( iLine == mcLoadCarriers[i].endLines() ) break;
			ordQty += (*iLine)->getOrdQty();
			pPrevLine = (*iLine);
		}

		loadCarrierId++;
	}

	return pSolution;
}
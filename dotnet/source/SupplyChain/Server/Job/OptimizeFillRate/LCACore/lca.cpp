#include "lca.h"
#include "lcaresult.h"
#include "lcapickbatch.h"
#include "lcapickbatchline.h"
#include "lcaparameters.h"
#include "lcaerrors.h"
#include "lcaanneal.h"

// This function sets the warehouse for which the load carrier algorithm should run.
// It also performs some preprocessing on the warehouse to speed up calculations
// The method returns gcLCAOk if the warehouse is set and can be used.
// It returns another code (see lcaerrors.h) if the warehouse has problems
// The load carrier algorithm will still function with an incorrect warehouse, but
// the results for distance cannot always be determined
int LCA::setWarehouse( LCAWarehouse warehouse, std::map<int, QStringList>& rsErrorMessages ) 
{ 
	mLCAWarehouse = warehouse; 
	int result = mLCAWarehouse.preprocess( rsErrorMessages ); 
	return result;
}


// This function runs the load carrier algorithm on one batch of pick batch lines. The result is an assignment
// of each pick batch line to one or more load carriers. If a load pick batch line is split over multiple load
// carriers, it is indicated what amount is assigned to which load carrier
//
// The algorithm performs a couple of steps
//
// 1. Check if the pickbatch or the parameters contain any errors that prevent processing the pickbatch
//    If any errors are encountered, return a result that contains the error messages. Else proceed with 2.
// 2. Run the load carrier algorithm currently used by Aptean on the pick batch. This gives a valid assignment
//    of pick batch lines to load carriers and furthermore gives a measure for the total distance traveled in
//    the warehouse.
// 3. Calculate a rough lower bound on the number of load carriers required to proces the pick batch. This is
//    the maximum of a) the minimum number of load carriers based on volume and b) based on weight.
// 4. If the number of load carriers calculated in step 3 is lower than the number of load carriers resulting 
//    from step 2, we perform a number of alternative constructive algorithms to create a load carrier
//    assignment. If at least one of these algorithms constructs a valid and better solution than the solution
//    found in step 2, we continue with this new solution.
// 5. We start a local search based on simulated annealing with the best constructed solution

LCAResult* LCA::processPickBatch( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters )
{
	// Step 1: Run the checks on pickbatch and parameters
	LCAResult* pResultCheck = checkPickBatchAndParameters( pPickbatch, pParameters );
	
	// If the result is not ok, we cannot process the pickbatch and we return results of the checks
	if ( !pResultCheck || (pResultCheck->getResultCode() > gcLCAMaxAcceptableError )  ) return pResultCheck;

	// Step 2: Run the Aptean algorithm
	LCAResult* pResultAptean = processPickBatchAptean( pPickbatch, pParameters );
	
	// If the check was not ok, we cannot calculate the distance
	double apteanDistance = 0;
	
	if ( pResultCheck->getResultCode() <= gcLCAMaxAcceptableWarning )
	{
		apteanDistance = pResultAptean->calculatePickDistance( mLCAWarehouse, false, 
			pPickbatch->getStrekWsId(), pPickbatch->getStrekXCoord(),  pPickbatch->getStrekYCoord(), true  );
	}

	// Step 3: Calculate a guaranteed lower bound 
	int lowerBound = calculateLowerBound( pPickbatch, pParameters );


	// If the resultcode was not ok, we cannot do any other construction than the aptean construction
	if ( pParameters->getOnlyAptean() || pResultCheck->getResultCode() > gcLCAMaxAcceptableLCAWarning  )
	{
		pResultAptean->copyErrors( pResultCheck );
		pResultAptean->setLowerBound( lowerBound );
		delete pResultCheck;
		return pResultAptean;
	}


	// In order to make it work for 03, we must be able to handle zero distance
	// If we have zero distance, we assume it is 1 meter
	if ( apteanDistance < gcLCAEpsilon ) apteanDistance = 1;


	// Step 4a: Run constructive algorithms: Alternating algorithm on location density
	LCAResult* pResultAlternating = processPickBatchAlternating( pPickbatch, pParameters );

	// This situation cannot occur, but we check it anyway, so even if it does occur, the algorithm doesn't crash
	if ( !pResultAlternating ) return pResultAptean;

	double alternatingDistance = pResultAlternating->calculatePickDistance( mLCAWarehouse, false, 
		pPickbatch->getStrekWsId(), pPickbatch->getStrekXCoord(),  pPickbatch->getStrekYCoord(), true );

	LCAResult* pBestResult = pResultAptean;

	// We check if the solution found by the alternating algorithm is better than the solution found
	// by the aptean algorithm. This is the case if it performs better on the main parameters AND
	// the distance of the solution is not too far
	if ( pResultAlternating->getTotalDistance() < apteanDistance * pParameters->getDistanceFactor() )
	{
		pBestResult = determineBest( pBestResult, pResultAlternating, pParameters );
	}
	
	
	// Step 4b: Run constructive algorithms: Alternating algorithm on aisle density
	LCAResult* pResultAisleDensity = processPickBatchAisleDensity( pPickbatch, pParameters );
	
	// This situation cannot occur, but we check it anyway, so even if it does occur, the algorithm doesn't crash
	if ( !pResultAisleDensity ) return pBestResult;

	double aisleDensityDistance = pResultAisleDensity->calculatePickDistance( mLCAWarehouse, false, 
		pPickbatch->getStrekWsId(), pPickbatch->getStrekXCoord(),  pPickbatch->getStrekYCoord(), true );
	
	// We check if the solution found by the aisle density algorithm is better than the best solution found
	// so far. This is the case if it performs better on the main parameters AND
	// the distance of the solution is not too far
	if ( pResultAisleDensity->getTotalDistance() < apteanDistance * pParameters->getDistanceFactor() )
	{
		pBestResult = determineBest( pBestResult, pResultAisleDensity, pParameters );
	}

	// Step 5: Determine the best result and use it as a starting point for the anneal
	LCAAnneal anneal( pPickbatch, pParameters, mLCAWarehouse );
	//anneal.setAnnealFile( mpAnnealFile );
	LCAResult* pAnnealResult = anneal.run( lowerBound, apteanDistance, pBestResult );
	
	// If all steps of the annealing must be skipped according to the parameter settings, then we end
	// up with no new annealresult and cannot delete the bestresult. In all other cases, the 
	// annealresult will be at least as good as the best result.
	if ( pAnnealResult != pBestResult )
	{
		delete pBestResult;
		pBestResult = pAnnealResult;
	}
		
	// The result found by annealling can never be worse than the result it starts with, so we dont have
	// to check which one is better
	// We do have to calculate the pick distance, since this initializes some members of the LCAResult
	pBestResult->calculatePickDistance( mLCAWarehouse, false, pPickbatch->getStrekWsId(), pPickbatch->getStrekXCoord(),  pPickbatch->getStrekYCoord(), true );
	
	pBestResult->copyErrors( pResultCheck );
	pBestResult->setLowerBound( lowerBound );
	delete pResultCheck;

	return pBestResult;
}

// Checks if the parameters are correct and if the pickbatch is correct
LCAResult* LCA::checkPickBatchAndParameters( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters )
{
	LCAResult* pResult = new LCAResult( pPickbatch->getNewGroupId() );
	
	// We check if the parameters are within bounds
	if ( !pParameters ) pResult->addError( gcLCAErrorNoParameters, "No parameters are supplied" );
	if ( pParameters && pParameters->getDistanceFactor() < 1 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "DistanceFactor must be at least 1" );
	if ( pParameters && pParameters->getMaxLDVol() < gcLCAEpsilon ) pResult->addError( gcLCAErrorParameterOutOfBounds, "MaxLDVol must be positive" );
	if ( pParameters && pParameters->getMaxLDWgt() < gcLCAEpsilon ) pResult->addError( gcLCAErrorParameterOutOfBounds, "MaxLDWeight must be positive" );
	if ( pParameters && pParameters->getMaxPBRowCar() < 1 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "MaxPBRowCar must be at least 1" );
	if ( pParameters && pParameters->getIterationsLC() < 500 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Iterations (LC) must be at least 500" );
	if ( pParameters && pParameters->getIterationsLC() > 500000 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Iterations (LC) must be at most 500000" );
	if ( pParameters && pParameters->getIterationsBeauty() < 500 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Iterations (Beauty) must be at least 500" );
	if ( pParameters && pParameters->getIterationsBeauty() > 500000 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Iterations (Beauty) must be at most 500000" );
	if ( pParameters && pParameters->getIterationsDistance() < 500 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Iterations (Distance) must be at least 500" );
	if ( pParameters && pParameters->getIterationsDistance() > 500000 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Iterations (Distance) must be at most 500000" );
	if ( pParameters && pParameters->getMaxmSecsLC() < 100 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Maximum time for LC Phase must be at least 100" );
	if ( pParameters && pParameters->getMaxmSecsBeauty() < 100 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Maximum time for Beauty Phase must be at least 100" );
	if ( pParameters && pParameters->getMaxmSecsDistance() < 100 ) pResult->addError( gcLCAErrorParameterOutOfBounds, "Maximum time for Distance Phase must be at least 100" );

	// Then we check if the pickbatch exists
	if ( !pPickbatch ) 
	{
		pResult->addError( gcLCAErrorNoPickbatch, "No pickbatch is given" );
		return pResult;
	}

	// If a pickbatch is given, we continue checking if the pickbatch is correct. First check if it is for the correct warehouse
	if ( pPickbatch->getWHId() != mLCAWarehouse.getWHId() ) 
	{
		pResult->addError( gcLCAErrorWarehouseMismatch, "The warehouse for the pickbatch and the lca do not match" );
	}

	// Can the strek be found in the warehouse?
	if ( !mLCAWarehouse.checkAreaExists( pPickbatch->getStrekWsId() ) )
	{
		pResult->addError( gcLCAErrorStrekAreaUnknown, QString( "The strek for pickbatch %1 cannot be found in the warehouse" ) .arg( pPickbatch->getNewGroupId() ) ); //THRO (omitted first parameter)
	}

	// Finally we check if we can match each pickbatchline to a location in the warehouse
	// Note that other checks on pickbatchline have already been done when the pickbatchlines were added to the pickbatch
	long totalOrdQty = 0;

	for ( int i=0; i<pPickbatch->getPickLineCount(); i++ )
	{
		const LCAPickBatchLine* pLine = pPickbatch->getPickLine(i);

		// Check if the area exists in the warehouse
		if ( !mLCAWarehouse.checkAreaExists( pLine->getWsId() ) ) pResult->addError( gcLCAErrorReferenceToNonExistingArea, QString( "Area \"%1\" does not exist" ).arg( pLine->getWsId()) ); //THRO
		
		// Check if the aisle exists in the area
		if ( !mLCAWarehouse.checkAisleExists( pLine->getWsId(), pLine->getAisle() ) ) 
			pResult->addError( gcLCAErrorReferenceToNonExistingAisle, QString( "Aisle \"%1\" \"%2\" does not exist" ).arg( pLine->getWsId(), pLine->getAisle()) ); //THRO
		
		// Find the splitted aisle in which this line is located
		// If no such splitted ailse exists, find the splitted aisle that is closest
		const LCASplittedAisle* pSplittedAisle = mLCAWarehouse.getSplittedAisle( pLine->getWsId(), pLine->getAisle(), pLine->getXCord(), pLine->getYCord() );

		if ( !pSplittedAisle )
		{
			pResult->addError( gcLCAErrorReferenceToNonExistingAisle, QString( "Location refers to non existing aisle \"%1\" \"%2\"" ).arg( pLine->getWsId(), pLine->getAisle() ) ); //THRO
		}
		
		std::pair<double,double> offset = mLCAWarehouse.getAreaOffset( pLine->getWsId() );

		// If the location is not in the splitted aisle, but only close to it, give a low level warning
		if ( pSplittedAisle && !pSplittedAisle->containsLocation( offset.first + pLine->getXCord(), offset.second + pLine->getYCord() ) )
		{
			pResult->addError( gcLCAErrorLocationOutsideAisle, QString( "Location (\"%3\",\"%4\") is outside aisle \"%1\" \"%2\"" ) //THRO
				.arg( pLine->getWsId(), pLine->getAisle())
				.arg(pLine->getXCord() )
				.arg(pLine->getYCord() ) );
		}

		// Check if the volume and weight of the smallest unit does not exceed the size of a load carrier
		if ( ( pLine->getVolume() > pParameters->getMaxLDVol() ) || ( pLine->getWeight() > pParameters->getMaxLDWgt() ) )
		{
			pResult->addError( gcLCAErrorTooLargePickBatchLine, QString( "Pickbatchline \"%1\" has too large volume or weight" ).arg( pLine->getPBRowId()) ); //THRO
		}
		else if (   ( !pParameters->getAllowPBRowSplit() ) 
			     && ( ( pLine->getVolume() * pLine->getOrdQty() > pParameters->getMaxLDVol() ) || ( pLine->getWeight() * pLine->getOrdQty()  > pParameters->getMaxLDWgt() ) ) )
		{
			pResult->addError( gcLCAErrorTooLargePickBatchLine, QString( "Pickbatchline \"%1\" has too large volume or weight" ).arg( pLine->getPBRowId()) ); //THRO
		}
		totalOrdQty += pLine->getOrdQty();

		// ordQty, volume and weight must be positive, otherwise the only aptean can be run
		if ( pLine->getOrdQty() <= gcLCAEpsilon ) pResult->addError( gcLCAErrorNonPositiveQuantity, QString("Pickbatchline \"%1\" has zero quantity").arg(pLine->getPBRowId()) ); //THRO
		if ( pLine->getVolume() <= gcLCAEpsilon ) pResult->addError( gcLCAErrorNonPositiveVolume, QString("Pickbatchline \"%1\" has zero volume").arg(pLine->getPBRowId()) ); //THRO
		if ( pLine->getWeight() <= gcLCAEpsilon ) pResult->addError( gcLCAErrorNonPositiveWeight, QString("Pickbatchline \"%1\" has zero weight").arg(pLine->getPBRowId()) ); //THRO
	}
	
	// If the total number of items exceeds 100000, the algoritm would get too slow, so we only do the aptean 
	// algorithm
	if ( totalOrdQty > 100000 )
	{
		pResult->addError( gcLCAErrorTooLargePickBatch, "The pickbatch has too many colli for full algorithm" );
	}

	return pResult;
}


// This method runs the Aptean load carrier algorithm on one batch of pick batch lines. The algorithm performs
// the following steps.
//
// 1. Sort the pick batch lines in ascending order of sequence number.
// 2. Start filling a load carrier from the beginning of the ordered set, until adding another line would
//    result in exceeding either
//    - The maximum volume of the load carrier
//    - The maximum weight of the load carrier
//    - The maximum number of lines assigned to one load carrier
// 3. Check if it is possible to assign part of the pick batch line to the load carrier. If not start a new 
//    load carrier and continue from 2. No pick batch lines are skipped.
LCAResult* LCA::processPickBatchAptean( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters )
{
	// Step 1: Sort the pick batch lines in ascending order of sequence number
	LCAPickBatchLineVector cAscendingPickLines;
	
	cAscendingPickLines.reserve( pPickbatch->getPickLineCount() );
	for ( int i=0; i<pPickbatch->getPickLineCount(); i++ )
	{
		cAscendingPickLines.push_back( pPickbatch->getPickLine(i) );
	}

	std::sort( cAscendingPickLines.begin(), cAscendingPickLines.end(), LCAPickBatchLineAscendingCompare() );

	// Steps 2 and 3: Create load carriers
	LCAResult *pResult = processSortedVector( pPickbatch, cAscendingPickLines, pParameters );	

	return pResult;
}

// This method runs the alternating load carrier algorithm on one batch of pick batch lines. The algorithm performs
// the following steps.
//
// 1. Sort the pick batch lines in descending order of weight to volume ratio
// 2. Create a new sequence by picking alternatingly from the front and from the back of the sorted list
// 3. Start filling a load carrier from the beginning of the ordered set, until adding another line would
//    result in exceeding either
//    - The maximum volume of the load carrier
//    - The maximum weight of the load carrier
//    - The maximum number of lines assigned to one load carrier
// 4. Check if it is possible to assign part of the pick batch line to the load carrier. If not start a new 
//    load carrier and continue from 3. No pick batch lines are skipped.
LCAResult* LCA::processPickBatchAlternating( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters )
{
	// Step 1: Sort the pick batch lines in ascending order of sequence number
	LCAPickBatchLineVector cRatioPickLines;
	
	cRatioPickLines.reserve( pPickbatch->getPickLineCount() );
	for ( int i=0; i<pPickbatch->getPickLineCount(); i++ )
	{
		cRatioPickLines.push_back( pPickbatch->getPickLine(i) );
	}

	std::sort( cRatioPickLines.begin(), cRatioPickLines.end(), LCAPickBatchLineAlternatingCompare() );

	// Step 2: Create a new vector by selecting pick batch lines alternatingly from the front and
	// from the back. We start by picking from the front, until the average density gets above 
	// densitySwitch. Then we start picking from the back until we are below the densitySwitch and
	// start again from the front, etc.
	// The densitySwitch is selected halfway between the average density of the store order and
	// the average density of a load carrier that hits the upper limit on volume and weight
	LCAPickBatchLineVector cAlternatingPickLines;
	
	double densitySwitchLow = pPickbatch->getTotalWeight() / pPickbatch->getTotalVolume();
	double densitySwitchUp = pParameters->getMaxLDWgt() / pParameters->getMaxLDVol();
	double densitySwitch = (densitySwitchLow + densitySwitchUp) / 2.0;
	createAlternatingVector( cRatioPickLines, cAlternatingPickLines, densitySwitch );

	// Steps 3 and 4: Create load carriers
	LCAResult *pResult = processSortedVector( pPickbatch, cAlternatingPickLines, pParameters );

	return pResult;
}

// This method runs the aisle density load carrier algorithm on one batch of pick batch lines. 
// The algorithm performs the following steps.
//
// 1. Calculate the average density of the pick batch lines in each aisle
// 2. Sort the pick batch lines in descending order density of the aisle in which they are situated
//    Pick batch lines in the same aisle are sorted by their own density
// 3. Create a new sequence by picking alternatingly from the front and from the back of the sorted list
// 4. Start filling a load carrier from the beginning of the ordered set, until adding another line would
//    result in exceeding either
//    - The maximum volume of the load carrier
//    - The maximum weight of the load carrier
//    - The maximum number of lines assigned to one load carrier
// 5. Check if it is possible to assign part of the pick batch line to the load carrier. If not start a new 
//    load carrier and continue from 3. No pick batch lines are skipped.
LCAResult* LCA::processPickBatchAisleDensity( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters )
{

	// Step 1. Calculate the average density of the pick batch lines in each aisle
	std::map< std::pair< QString, QString >, double > volumePerAisle;
	std::map< std::pair< QString, QString >, double > weightPerAisle;
	std::map< std::pair< QString, QString >, double > densityPerAisle;

	// We count the total volume and total weight in each aisle
	for ( int i=0; i<pPickbatch->getPickLineCount(); i++ )
	{
		volumePerAisle[ std::make_pair(pPickbatch->getPickLine(i)->getWsId(),  pPickbatch->getPickLine(i)->getAisle() )] += pPickbatch->getPickLine(i)->getOrdQty() *  pPickbatch->getPickLine(i)->getVolume();
		weightPerAisle[ std::make_pair(pPickbatch->getPickLine(i)->getWsId(),  pPickbatch->getPickLine(i)->getAisle() )] += pPickbatch->getPickLine(i)->getOrdQty() *  pPickbatch->getPickLine(i)->getWeight();
	}

	// From those numbers we can calculate the average density in each aisle
	for ( std::map< std::pair< QString, QString >, double >::iterator iAisle = volumePerAisle.begin(); iAisle != volumePerAisle.end(); ++iAisle )
	{
		densityPerAisle[iAisle->first] = weightPerAisle[iAisle->first] / iAisle->second;
	}

	// Step 2. Sort the pick batch lines in descending order density of the aisle
	LCAPickBatchLineVector cAisleDensityPickLines;
	cAisleDensityPickLines.reserve( pPickbatch->getPickLineCount() );
	for ( int i=0; i<pPickbatch->getPickLineCount(); i++ )
	{
		cAisleDensityPickLines.push_back( pPickbatch->getPickLine(i) );
	}
	std::sort( cAisleDensityPickLines.begin(), cAisleDensityPickLines.end(), LCAPickBatchLineAisleDensityCompare( densityPerAisle ) );


	
	// Step 3: Create a new vector by selecting pick batch lines alternatingly from the front and
	// from the back. We start by picking from the front, until the average density gets above 
	// densitySwitch. Then we start picking from the back until we are below the densitySwitch and
	// start again from the front, etc.
	// The densitySwitch is selected halfway between the average density of the store order and
	// the average density of a load carrier that hits the upper limit on volume and weight
	LCAPickBatchLineVector cAlternatingPickLines;
	double densitySwitchLow = pPickbatch->getTotalWeight() / pPickbatch->getTotalVolume();
	double densitySwitchUp = pParameters->getMaxLDWgt() / pParameters->getMaxLDVol();
	double densitySwitch = (densitySwitchLow + densitySwitchUp) / 2.0;
	createAlternatingVector( cAisleDensityPickLines, cAlternatingPickLines, densitySwitch );

	// Steps 4 and 5
	LCAResult *pResult = processSortedVector( pPickbatch, cAlternatingPickLines, pParameters );
	return pResult; 
	
}

// This method assigns the pick lines in the vector to load carriers, in the order in which they occur,
// without skipping any lines
// If a line does not fit in the current load carrier, the current load carrier is closed and a new load
// carrier is started, unless it is allowed to split lines into colli. In that case the same rule applies
// but not on line level, but on collo level.
LCAResult* LCA::processSortedVector( const LCAPickBatch *pPickbatch, const LCAPickBatchLineVector& rVector, 
	const LCAParameters *pParameters )
{
	LCAResult *pResult = new LCAResult( pPickbatch->getNewGroupId() );

	double totalVolume = 0.0;	// The volume assigned to the active load carrier so far
	double totalWeight = 0.0;	// The weight assigned to the active load carrier so far
	int    currentLC = 1;		// We start numbering the load carriers from 1
	int    lineCount = 0;		// The number of lines already assigned to the current load carrier

	for ( long i=0; i<rVector.size(); i++ )
	{
		const LCAPickBatchLine* const pPickLine = rVector[i];
		double ordQty = pPickLine->getOrdQty();
		double weight = pPickLine->getWeight();
		double volume = pPickLine->getVolume();
		int    splitId = 1;							// Counts the number of parts, 1 means not splitted yet
		
		// If adding an extra line would exceed either the number of lines, the volume or the weight
		// we start a new load carrier
		if (    ( lineCount + 1 > pParameters->getMaxPBRowCar() )					 // Number of lines exceeded
			 || ( totalVolume + (ordQty * volume) >  pParameters->getMaxLDVol() )    // Volume limit exceeded
			 || ( totalWeight + (ordQty * weight) >  pParameters->getMaxLDWgt() ) )  // Weight limit exceeded
		{
			// We are now in the situation that the current pick line does not fit in the active load carrier.
			// If we are allowed to split pick lines, we try can try to add part of the line to the active load
			// carrier. Note that we only split off full colli.
			if (   ( pParameters->getAllowPBRowSplit() )				// We are allowed to split
				&& ( lineCount < pParameters->getMaxPBRowCar() ) )		// We have not reached the max lines
			{
				int splitQty = 0;
				// This loop must end, since eiter the volume or the weight didnt fit
				while (( totalVolume + ( (splitQty+1) * volume) <=  pParameters->getMaxLDVol() )  	// The volume must fit
					&& ( totalWeight + ( (splitQty+1) * weight) <=  pParameters->getMaxLDWgt() ) )	// The weight must fit
				{
					splitQty++;
				}

				// If at least one part fits in the active load carrier, add it and reduce the remaining ordQty bij 
				// the number of parts added
				if ( splitQty > 0 )
				{

					pResult->addResultLine( pPickLine->getPBRowId(), QString::number(splitId), splitQty, QString::number(currentLC), pPickLine->getPickSeq(), 
						pPickLine->getCatGroup(), pPickLine->getWsId(), pPickLine->getAisle(), pPickLine->getXCord(), pPickLine->getYCord(), pPickLine->getWPAdr() );
					splitId++;
					ordQty -= splitQty;
				}
			}
		
			// The current load carrier is filled to the max
			// We still must consider the possibility that the (remainder of) the pick batch line is bigger than a full
			// load carrier. In which case we must split it further;
			// First we determine how many units can be put on one load carrier
			double unitsPerLCWeight = pParameters->getMaxLDWgt()/weight;   // Maximum units per LC based on weight
			double unitsPerLCVolume = pParameters->getMaxLDVol()/volume;   // Maximum units per LC based on volume
			double unitsPerLC = (unitsPerLCWeight < unitsPerLCVolume)?unitsPerLCWeight:unitsPerLCVolume;
			
			// We only take of full units, so we must rond this number down
			int splitSize = (int)unitsPerLC;	

			// We always put at least one unit on a load carrier, even if it does not fit
			if ( splitSize < 1 ) splitSize = 1; 

			// As long as the ordQty is bigger than the amount of units that fit on one load carrier, we create
			// a new load carrier with maximum fill
			while ( splitSize < ordQty )
			{
				currentLC++;
				splitId++;
				pResult->addResultLine( pPickLine->getPBRowId(), QString::number(splitId), splitSize, QString::number(currentLC), pPickLine->getPickSeq(), 
					pPickLine->getCatGroup(), pPickLine->getWsId(), pPickLine->getAisle(), pPickLine->getXCord(), pPickLine->getYCord(), pPickLine->getWPAdr() );
				ordQty -= splitSize;
			}

			// We now have a quantity left that will fit on one load carrier.
			// Proceed with the next load carrier
			currentLC++;

			// And set all values to 0
			totalVolume = 0.0;
			totalWeight = 0.0;
			lineCount = 0;
		}
		
		// Add the line to the active load carrier
		// We only save the splitId if the line has been splitted
		pResult->addResultLine( pPickLine->getPBRowId(), (splitId>1)?QString::number(splitId):"", ordQty, QString::number(currentLC), pPickLine->getPickSeq(), 
			pPickLine->getCatGroup(), pPickLine->getWsId(), pPickLine->getAisle(), pPickLine->getXCord(), pPickLine->getYCord(), pPickLine->getWPAdr() );
		// And update the totals
		totalVolume += ordQty * volume;
		totalWeight += ordQty * weight;
		lineCount++;
	}

	// We zorgen ervoor dat het resultaat gesorteerd wordt op een manier dat per load carrier de regels op volgorde van pick sequence staan
	pResult->sortResultLines();

	return pResult;
}

// This method creates a new vector of pick batch lines, based on the source vector. The source vector is assumed to be sorted in descending
// order of density, or a sorting that is not too far off from that. The trend should be descending
// The newly created vector will start by taking elements from the front of the original vector, until the average density of all items
// in the new vector is above the densitySwitch value. Then it will start taking items from the end of the source vector until
// the avarage density is below the densitySwitch value again. This process goes on until all elements are assigned to the new vector
void LCA::createAlternatingVector( const LCAPickBatchLineVector& rSourcePickLines, LCAPickBatchLineVector& rcAlternatingPickLines, const double densitySwitch  )
{
	// Start with an empty vector and already claim the right amount of memory, so insertions will be done quickly
	rcAlternatingPickLines.clear();
	rcAlternatingPickLines.reserve( rSourcePickLines.size() );
	
	// Initialize the startpositions: at the front of the vector and at the end of the vector
	int lowIdx = 0;
	int highIdx = rSourcePickLines.size() - 1;

	// Initialize the current density with a large value, so we will start picking at the end of the vector
	double currentDensity = 100000.0;
	// And initialize variables to keep track of the volume and weight processed so far
	double totalVolume = 0;
	double totalWeight = 0;

	// We either process an element from the front of the vector or from the end of the vector
	// As soon as they meet, we are done
	while ( lowIdx <= highIdx )
	{
		// If the current density is lower than the value we aim for, add a high-density element
		if ( currentDensity < densitySwitch ) 
		{
			rcAlternatingPickLines.push_back( rSourcePickLines[lowIdx] );
			lowIdx++;
		}
		// If not, add a low density element
		else
		{
			rcAlternatingPickLines.push_back( rSourcePickLines[highIdx] );
			highIdx--;
		}

		// Update the current density
		totalVolume += rcAlternatingPickLines.back()->getOrdQty() * rcAlternatingPickLines.back()->getVolume();
		totalWeight += rcAlternatingPickLines.back()->getOrdQty() * rcAlternatingPickLines.back()->getWeight();
		currentDensity = totalWeight / totalVolume;
	}
}

// This methode calculates a guaranteed lower bound on the number of load carriers required to process the pick batch
// It is the maximum of the required number based on volume and the required number based on weight
int LCA::calculateLowerBound( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters )
{
	// Minimum number of load carriers required based on volume
	double minLCsOnVolume = pPickbatch->getTotalVolume() / pParameters->getMaxLDVol();

	// Minimum number of load carriers required based on weight
	double minLCsOnWeight = pPickbatch->getTotalWeight() / pParameters->getMaxLDWgt();

	// Take the maximum of both minima
	double maximum = ( minLCsOnVolume > minLCsOnWeight ) ? minLCsOnVolume : minLCsOnWeight;

	// Return this number of load carriers, rounded up to the nearest integer value;
	int lowerBound = ceil(maximum);

	return lowerBound;
}

// The method returns the best solution out of the two options and deletes the other solution
LCAResult* LCA::determineBest( LCAResult* pOption1, LCAResult* pOption2, const LCAParameters* pParameters )
{
	// If one of the options does not exist, the other one is better by default
	if ( !pOption1 ) return pOption2;
	if ( !pOption2 ) return pOption1;

	// First criterium: the solution with the smallest amount of load carriers is always better
	if ( pOption1->getLoadCarrierCount() < pOption2->getLoadCarrierCount() )
	{
		delete pOption2; 
		return pOption1;
	}
	if ( pOption2->getLoadCarrierCount() < pOption1->getLoadCarrierCount() )
	{
		delete pOption1; 
		return pOption2;
	}

	// Second criterium: If the amount of load carriers is equal in both solutions, then the one with the most beauties is better
	if ( ( pOption1->getBeautyCount() > pOption2->getBeautyCount() ) && pParameters->getDoBeautyPhase() )
	{
		delete pOption2; 
		return pOption1;
	}
	if ( ( pOption2->getBeautyCount() > pOption1->getBeautyCount() )  && pParameters->getDoBeautyPhase() )
	{
		delete pOption1; 
		return pOption2;
	}

	// Third criterium: If the amount of beauties is equal in both solutions, then the one with the least splitted lines is better
	if ( pOption1->getResultLineCount() < pOption2->getResultLineCount() )
	{
		delete pOption2; 
		return pOption1;
	}
	if ( pOption1->getResultLineCount() > pOption2->getResultLineCount() )
	{
		delete pOption1; 
		return pOption2;
	}

	// If all other criteria are equal, the smallest distance wins
	if ( pOption1->getTotalDistance() < pOption2->getTotalDistance() )
	{
		delete pOption2; 
		return pOption1;
	}

	delete pOption1; 
	return pOption2;
}
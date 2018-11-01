#include "lcawarehouse.h"
#include "lcaerrors.h"
#include "lcaaisle.h"
#include <QtCore/qstringlist.h>

// Create a new empty warehouse
LCAWarehouse::LCAWarehouse()
{
	msWHId = "";
}

// Create a new warehouse with warehouse id sWHId
LCAWarehouse::LCAWarehouse( QString sWHId )
{
	msWHId = sWHId;
}

// Destruct a warehouse and free all claimed memory
LCAWarehouse::~LCAWarehouse()
{
	// Delete all splitted aisles
	for ( std::map< std::pair< QString, QString >, std::vector< LCASplittedAisle* > >::iterator iAisle = mcSplittedAisle.begin();
		iAisle != mcSplittedAisle.end(); ++iAisle )
	{
		for ( long aisleIdx = 0; aisleIdx < iAisle->second.size(); ++aisleIdx )
		{
			LCASplittedAisle* pSplittedAisle = iAisle->second[aisleIdx];
			delete pSplittedAisle;
		}
	}

	mcSplittedAisle.clear();
	mcAisleWayPoint.clear();
	mcAisle.clear();
	mcArea.clear();
}

// Add a new area to the warehouse. If an area with the same name already exists, it is replaced
int LCAWarehouse::addArea(	const QString sWSId, const double xCord, const double yCord )
{
	mcArea[sWSId] = LCAArea( sWSId, xCord, yCord );
	return gcLCAOk;
}

// Add a new aisle to the warehouse. If an aisle with the same id already exists, it is replaced
int LCAWarehouse::addAisle(	const QString sWSId, const QString sAisle, const double frmXCord, const double frmYCord,
	const double toXCord, const double toYCord, const QString sDirectionPick )
{
	mcAisle[ std::make_pair(sWSId, sAisle) ] = LCAAisle( sWSId, sAisle, frmXCord, frmYCord, toXCord, toYCord, sDirectionPick );
	
	mcAisleWayPoint[ std::make_pair(sWSId, sAisle) ].push_back( LCAAisleWayPoint( sWSId, sAisle, "AisleEntry", frmXCord, frmYCord ) );
	mcAisleWayPoint[ std::make_pair(sWSId, sAisle) ].push_back( LCAAisleWayPoint( sWSId, sAisle, "AisleExit", toXCord, toYCord ) );

	return gcLCAOk;
}

// Add a new aisle waypoint to the warehouse. If an aisle waypoint with the same id already exists, it is replaced
int LCAWarehouse::addAisleWayPoint(	const QString sWSId,
								const QString sAisle,
								const QString sWaypointId,
								const double xCord,
								const double yCord )
{
	mcAisleWayPoint[ std::make_pair(sWSId, sAisle) ].push_back( LCAAisleWayPoint( sWSId, sAisle, sWaypointId, xCord, yCord ) );
	return gcLCAOk;
}

// When a warehouse is assigned to the load carrier algorithm, a number of preprocessing steps is performed to make future calculations
// for distance faster.
// The steps splits aisles in splitted aisles, based on the location of the waypoints. Each section of an aisle between two aisle waypoints
// becomes a splitted aisle. All calculations will be done on splitted aisles
// If the result is not gcLCAOk, the list rsErrorMessages will contain the encountered errors
int LCAWarehouse::preprocess( std::map<int, QStringList>& rsErrorMessages )
{
	rsErrorMessages.clear();

	// The set of splitted aisles is created in de preprocess step. Hence, if we already have a set of splitted aisle,
	// the proprocess step has already been performed. Since this only may be done once, we return an error.
	if ( mcSplittedAisle.size() != 0 )
	{
		rsErrorMessages[gcLCAErrorWarehousePreprocessCalledTwice].append("Preprocessing of the warehouse has already been done before");
		return gcLCAErrorWarehousePreprocessCalledTwice;
	}

	// Split the input aisles in splitted aisles, i.e., sections of aisles between two consecutive waypoints
	int result = createSplittedAisles( rsErrorMessages ); 
	
	return result;
}


// The method creates splitted aisles, based on the set of aisles and the set of aislewaypoints
// It returns gcLCAOk if everything is ok. It returns the last encountered error otherwise;
// If the return code is not gcLCAOk, then the list rsErrorMessage will contain a description of all encountered problems
int LCAWarehouse::createSplittedAisles( std::map<int, QStringList>& rsErrorMessages )
{
	int result = gcLCAOk;

	std::map< std::pair< QString, QString >, LCAAisle >::const_iterator iAisle;

	// The aisle are processed one by one
	for ( iAisle = mcAisle.begin(); iAisle != mcAisle.end(); ++iAisle )
	{
		const LCAAisle& rAisle = iAisle->second;

		// We first check if the aisle has a mono-directional pickdirection, if not, we don't support is
		if ( !rAisle.hasMonoDirectionalPick() ) 
		{
			rsErrorMessages[gcLCAErrorBidirectionalPick].append( QString( "Aisle \"%1\" \"%2\" has non supported pick direction %3. Aisle skipped." ).arg( rAisle.getWsId(), rAisle.getAisle(), rAisle.getDirectionPick() ) ); //THRO (omitted third parameter)
			result = gcLCAErrorBidirectionalPick;
			continue;
		}
		
		if ( rAisle.getMovingDirectionIsX() && rAisle.getMovingDirectionIsY() )
		{
			rsErrorMessages[gcLCAErrorBidirectionalMove].append( QString( "Aisle \"%1\" \"%2\" differs in both X and Y direction. Aisle skipped." ).arg( rAisle.getWsId(), rAisle.getAisle(), rAisle.getDirectionPick() ) ); //THRO
			result = gcLCAErrorBidirectionalMove;
			continue;
		}

		// We look up the area in which the aisle is located
		std::map< QString, LCAArea >::const_iterator iArea = mcArea.find( rAisle.getWsId() );
		
		// If the area cannot be found, we do not know the location of the aisle relative to other aisles in 
		// other areas. We therefore cannot use this aisle.
		if ( iArea == mcArea.end() ) 
		{
			rsErrorMessages[gcLCAErrorReferenceToNonExistingArea].append( QString( "Aisle \"%1\" \"%2\" refers to non-existing area \"%1\". Aisle skipped." ).arg( rAisle.getWsId(), rAisle.getAisle() ) ); //THRO
			result = gcLCAErrorReferenceToNonExistingArea;
			continue;
		}
		
		// For each aisle, we collect the set of waypoints associated with that aisle
		std::vector< LCAAisleWayPoint > cWayPoints = mcAisleWayPoint[ iAisle->first ];

		// We sort the waypoints in ascending order of pick sequence. This is done to make sure that the splitted aisle that we create,
		// can always be traversed from start to end en never from end to start.
		std::sort( cWayPoints.begin(), cWayPoints.end(), LCAAisleWayPointCompare( rAisle.hasPositivePickDirection(), rAisle.getMovingDirectionIsX() ) );


		double fullAisleLength = 0;
		
		if ( iAisle->second.getMovingDirectionIsX() ) 
		{
			fullAisleLength = abs(cWayPoints[0].getXCord() - cWayPoints[cWayPoints.size()-1].getXCord());
		}
		else
		{
			fullAisleLength = abs(cWayPoints[0].getYCord() - cWayPoints[cWayPoints.size()-1].getYCord());
		}

		// We each consecutive pair of waypoints, we create a splitted aisle.
		for ( long i = 0; i + 1 <cWayPoints.size(); i++ )
		{
			
			// Depending on the moving direction, the length of the aisle is the difference between the x-coords or the y-coords
			double aisleLenght = 0;
			
			if ( iAisle->second.getMovingDirectionIsX() ) 
			{
				aisleLenght = abs( cWayPoints[i+1].getXCord() - cWayPoints[i].getXCord() );
			}
			else // iAisle->second.getMovingDirectionIsY()
			{
				aisleLenght = abs(cWayPoints[i+1].getYCord() - cWayPoints[i].getYCord());
			}

			// Doordat punten zowel als waypoint als als entry/exit kunnen voorkomen
			if ( aisleLenght < gcLCAEpsilon && ( fullAisleLength > gcLCAEpsilon || i != 0 ) ) continue;			

			double minCoord = iAisle->second.getMinCoordinateMovingDirection();
			double maxCoord = iAisle->second.getMaxCoordinateMovingDirection();
			
			if ( iAisle->second.getMovingDirectionIsX() )
			{
				minCoord  += iArea->second.getXCord();
				maxCoord  += iArea->second.getXCord();
			}
			else
			{
				minCoord  += iArea->second.getYCord();
				maxCoord  += iArea->second.getYCord();
			}

			// Create a new splitted aisle.
			mcSplittedAisle[iAisle->first].push_back( new LCASplittedAisle(	iAisle->second.getWsId(), 
																		iAisle->second.getAisle(), 
																		cWayPoints[i].getXCord() + iArea->second.getXCord(), 
																		cWayPoints[i].getYCord() + iArea->second.getYCord(), 
																		cWayPoints[i+1].getXCord() + iArea->second.getXCord(), 
																		cWayPoints[i+1].getYCord() + iArea->second.getYCord(), 
																		i, 
																		aisleLenght,
																		fullAisleLength,
																		minCoord,
																		maxCoord ) );

		}
	}

	return result;
}

// Returns the splitted aisle in which the given position is located. X en Y coords must 
const LCASplittedAisle* LCAWarehouse::getSplittedAisle( const QString sWsId, const QString sAisle, const double xCoord, const double yCoord ) const
{
	std::map< QString, LCAArea >::const_iterator iArea = mcArea.find( sWsId );
	if ( iArea == mcArea.end() ) return 0;

	std::map< std::pair< QString, QString >, std::vector< LCASplittedAisle* > >::const_iterator iSplittedAisle = mcSplittedAisle.find( std::make_pair(sWsId,sAisle) );
	if ( iSplittedAisle == mcSplittedAisle.end() ) return 0;

	double adjustedXCoord = xCoord + iArea->second.getXCord();
	double adjustedYCoord = yCoord + iArea->second.getYCord();

	// We first check if we can find a splitted aisle that contains the location
	for ( int splitIdx = 0; splitIdx < iSplittedAisle->second.size(); splitIdx++ )
	{
		const LCASplittedAisle* pSplittedAisle =  iSplittedAisle->second[splitIdx];
		if ( pSplittedAisle->containsLocation( adjustedXCoord, adjustedYCoord ) )
		{
			return pSplittedAisle;
		}
	}

	// If non of the splitted aisles contains the location, then we return the aisle that is closest
	const LCASplittedAisle* pBestAisle = 0;
	double bestDistance = -1;

	for ( int splitIdx = 0; splitIdx < iSplittedAisle->second.size(); splitIdx++ )
	{
		const LCASplittedAisle* pSplittedAisle =  iSplittedAisle->second[splitIdx];
		double distance = pSplittedAisle->getDistanceToLocation( adjustedXCoord, adjustedYCoord );

		if ( bestDistance < 0 || distance < bestDistance )
		{
			bestDistance = distance;
			pBestAisle = pSplittedAisle;
		}
	}

	return pBestAisle;
}
		
// Returns the offset of the area with the given ID
std::pair< double, double > LCAWarehouse::getAreaOffset( const QString sWsId ) const
{
	std::map< QString, LCAArea >::const_iterator iArea = mcArea.find( sWsId );

	if ( iArea != mcArea.end() ) return std::make_pair( iArea->second.getXCord(), iArea->second.getYCord() );
	return std::make_pair( 0, 0 );
}


// This method calculates the distance between two location addresses.
// The x and y coordinates must already be corrected for the offset of the area
double LCAWarehouse::getDistance( const LCASplittedAisle* pAisleFrom, const double xCoordFrom, const double yCoordFrom,  
		const LCASplittedAisle* pAisleTo, const double xCoordTo, const double yCoordTo ) const
{
	// If we dont have the splitted aisles, we can only do crowDistance
	if ( !pAisleFrom || !pAisleTo ) return getDistanceCrow( xCoordFrom, yCoordFrom, xCoordTo, yCoordTo );
		
	// If the locations are in the same splitted aisle, then we take the differce between the locations in the 
	// moving direction, i.e., we ignore if the locations are on the same side of the aisle or not
	if ( pAisleFrom == pAisleTo )
	{
		double xCoordFromAdjusted = xCoordFrom;
		double yCoordFromAdjusted = yCoordFrom;

		double xCoordToAdjusted = xCoordTo;
		double yCoordToAdjusted = yCoordTo;

		// Make sure the coordinates are within the aisle
		pAisleFrom->adjustCoords( xCoordFromAdjusted, yCoordFromAdjusted );
		pAisleFrom->adjustCoords( xCoordToAdjusted, yCoordToAdjusted );

		if ( pAisleFrom->getMovingDirectionIsX() )
		{
			double dist = xCoordToAdjusted - xCoordFromAdjusted;
			if ( pAisleFrom->getFrmXCord() > pAisleFrom->getToXCord() )
			{
				dist *= -1;
			}

			//dist = abs(dist);
			return dist;
		}
		else
		{
			double dist =  yCoordToAdjusted - yCoordFromAdjusted;
			if ( pAisleFrom->getFrmYCord() > pAisleFrom->getToYCord() )
			{
				dist *= -1;
			}

			//dist = abs(dist);
			return dist;
		}
	}

	// If the address locations are in different splitted aisles, the distance can be calculated as the sum of
	// 1. The distance from the first address location to the exit point of its splitted aisle
	// 2. The distance from the start of the splitted aisle of the second address location and the second address location
	// 3. The manhattan distance between the exit waypoint of the first splitted aisle and the entry waypoint of the second splitted aisle
	else
	{
		double distFromToAisleExit = pAisleFrom->getDistanceToExit( xCoordFrom, yCoordFrom );
		double distAisleEntryToTo =  pAisleTo->getDistanceFromEntry( xCoordTo, yCoordTo );
		double distAisleFromAisleTo = abs( pAisleFrom->getToXCord() - pAisleTo->getFrmXCord() ) + 
			                          abs( pAisleFrom->getToYCord() - pAisleTo->getFrmYCord() );

		return distFromToAisleExit + distAisleEntryToTo + distAisleFromAisleTo;
	}
}

// This method returns the distance to the given location from the start of the load carrier
// The x and y coordinates must already be corrected for the offset of the area
// The start of the load carrier is defined as the exit or entry point of the splitted aisle that is closes to
// the strek
double LCAWarehouse::getEntryDistance( const LCASplittedAisle* pAisle, const double xCoord, const double yCoord, const double strekX, const double strekY, const bool extraAisle ) const
{
	// If we dont have an aisle, we cannot calculate its entry distance
	if ( !pAisle ) return 0;

	/* Twn: Old implementation 
	double distanceStrekToFrom = abs( strekX - pAisle->getFrmXCord() ) + 
								 abs( strekY - pAisle->getFrmYCord() ) ;

	double distanceStrekToTo =  abs( strekX - pAisle->getToXCord() ) + 
								abs( strekY - pAisle->getToYCord() ) ;

	double startUpDistance = 0;

	// Only add extra aisle, when running the LCA
	if ( distanceStrekToFrom > distanceStrekToTo && extraAisle )
	{
		startUpDistance = pAisle->getLength();	
	}
	//*/

	///* Twn: New implementation
	double startUpDistance = 0;

	if ( extraAisle )
	{
		if ( pAisle->getMovingDirectionIsX() )
		{
			double strekCoord = strekX;
			if ( strekCoord < pAisle->getMinCoordOfFullAisle() ) strekCoord = pAisle->getMinCoordOfFullAisle();
			if ( strekCoord > pAisle->getMaxCoordOfFullAisle() ) strekCoord = pAisle->getMaxCoordOfFullAisle();
			startUpDistance = abs( strekCoord - pAisle->getFrmXCord() );
		}
		else //  pAisle->getMovingDirectionIsY()
		{
			double strekCoord = strekY;
			if ( strekCoord < pAisle->getMinCoordOfFullAisle() ) strekCoord = pAisle->getMinCoordOfFullAisle();
			if ( strekCoord > pAisle->getMaxCoordOfFullAisle() ) strekCoord = pAisle->getMaxCoordOfFullAisle();
			startUpDistance = abs( strekCoord - pAisle->getFrmYCord() );
		}
	}
	//*/

	//Twn
	return startUpDistance + pAisle->getDistanceFromEntry( xCoord, yCoord );
	//return ( 0.4 * startUpDistance ) + pAisle->getDistanceFromEntry( xCoord, yCoord );
}

// This method returns the distance from the given location to the endpoint of the load carrier
// The x and y coordinates must already be corrected for the offset of the area
// The endpoint of the load carrier is defined as the entry or exit point of the splitted aisle that
// is closest to the strek
double LCAWarehouse::getExitDistance( const LCASplittedAisle* pAisle, const double xCoord, const double yCoord, const double strekX, const double strekY, const bool extraAisle ) const
{	
	// If we dont have an aisle, we cannot calculate its exit distance
	if ( !pAisle ) return 0;

	/* Twn: Old implementation 
	double distanceStrekToFrom = abs( strekX - pAisle->getFrmXCord() ) + 
						         abs( strekY - pAisle->getFrmYCord() ) ;

	double distanceStrekToTo = abs( strekX - pAisle->getToXCord() ) + 
						       abs( strekY - pAisle->getToYCord() ) ;

	double endDistance = 0;

	// Only add extra aisle, when running the LCA
	if ( distanceStrekToTo > distanceStrekToFrom && extraAisle )
	{
		endDistance = pAisle->getLength();
	}
	//*/
	

	///* Twn: New implementation
	double endDistance = 0;

	if ( extraAisle )
	{
		if ( pAisle->getMovingDirectionIsX() )
		{
			double strekCoord = strekX;
			if ( strekCoord < pAisle->getMinCoordOfFullAisle() ) strekCoord = pAisle->getMinCoordOfFullAisle();
			if ( strekCoord > pAisle->getMaxCoordOfFullAisle() ) strekCoord = pAisle->getMaxCoordOfFullAisle();
			endDistance = abs( strekCoord - pAisle->getToXCord() );
		}
		else //  pAisle->getMovingDirectionIsY()
		{
			double strekCoord = strekY;
			if ( strekCoord < pAisle->getMinCoordOfFullAisle() ) strekCoord = pAisle->getMinCoordOfFullAisle();
			if ( strekCoord > pAisle->getMaxCoordOfFullAisle() ) strekCoord = pAisle->getMaxCoordOfFullAisle();
			endDistance = abs( strekCoord - pAisle->getToYCord() );
		}
	}
	//*/

	//Twn
	return pAisle->getDistanceToExit( xCoord, yCoord ) + endDistance;
	//return pAisle->getDistanceToExit( xCoord, yCoord ) + (0.4 * endDistance);
}

// This method returns the crow distance between the two locations
// The x and y coordinates must already be corrected for the offset of the area
double LCAWarehouse::getDistanceCrow( const double xCoordFrom, const double yCoordFrom, const double xCoordTo, const double yCoordTo ) const
{

	return sqrt( ( (xCoordFrom - xCoordTo) * (xCoordFrom - xCoordTo) ) + 
		         ( (yCoordFrom - yCoordTo) * (yCoordFrom - yCoordTo) ) );
}
		
// The crowdistance measure does not take into account the distance to the first pick location
double LCAWarehouse::getEntryDistanceCrow() const
{
	return 0.0;
}
		
// The crowdistance measure does not take into account the distance from the last pick location
double LCAWarehouse::getExitDistanceCrow() const
{
	return 0.0;
}

// This method returns true iff the area exists in the warehouse
bool LCAWarehouse::checkAreaExists( const QString sWsId ) const
{
	return mcArea.find( sWsId ) != mcArea.end();
}
		
// This method returns true iff the aisle exists in the warehouse
bool LCAWarehouse::checkAisleExists( const QString sWsId, const QString sAisle ) const
{
	return mcAisle.find( std::make_pair( sWsId, sAisle ) ) != mcAisle.end();
}
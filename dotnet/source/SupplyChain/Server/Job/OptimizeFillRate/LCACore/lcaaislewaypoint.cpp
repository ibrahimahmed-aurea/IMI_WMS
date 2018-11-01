#include "lcaaislewaypoint.h"

// Creates a waypoint for the given aisle
LCAAisleWayPoint::LCAAisleWayPoint(	const QString sWsId, const QString sAisle, const QString sWayPointId, const double xCord, const double yCord )
{
	msWsId = sWsId;
	msAisle = sAisle;
	msWayPointId = sWayPointId;
	mXCord = xCord;
	mYCord = yCord;
}

// Create a sort order for waypoints, depending on if they should be sorted in positve or negative direction and
// if the moving direction is X or Y
LCAAisleWayPointCompare::LCAAisleWayPointCompare( const bool hasPositiveDirectionPick, const bool movingDirectionIsX )
{
	mHasPositiveDirectionPick = hasPositiveDirectionPick;
	mMovingDirectionIsX = movingDirectionIsX;
}

// Sorts the waypoint in the order in which they are visited if the aisle is travelled in the 
// moving direction
// If multiple waypoints are situated at the exact same locations, we sort them by waypoint id
bool LCAAisleWayPointCompare::operator()(const LCAAisleWayPoint wp1, const LCAAisleWayPoint wp2 ) const
{
	if ( mHasPositiveDirectionPick && !mMovingDirectionIsX )
	{
		if ( wp1.getYCord() !=  wp2.getYCord() ) return wp1.getYCord() <  wp2.getYCord();
	}
	else if ( (!mHasPositiveDirectionPick) && (!mMovingDirectionIsX) )
	{
		if ( wp1.getYCord() !=  wp2.getYCord() ) return wp1.getYCord() >  wp2.getYCord();
	}
	else if ( mHasPositiveDirectionPick && mMovingDirectionIsX )
	{
		if ( wp1.getXCord() !=  wp2.getXCord() ) return wp1.getXCord() <  wp2.getXCord();
	}
	else // if ( msDirectionPick == "-" && mMovingDirectionIsX )
	{
		if ( wp1.getXCord() !=  wp2.getXCord() ) return wp1.getXCord() >  wp2.getXCord();
	}

	return wp1.getWayPointId() < wp2.getWayPointId();
}
#include "lcaaisle.h"

// Constructs an empty aisle
LCAAisle::LCAAisle()
{
	msWsId.clear();
	msAisle.clear();
	mFrmXCord = 0.0;
	mFrmYCord = 0.0;
	mToXCord = 0.0;
	mToYCord = 0.0;
	msDirectionPick.clear();

	mMinXCoord = 0;
	mMinYCoord = 0;
	mMaxXCoord = 0;
	mMaxYCoord = 0;
}

// Constructs an aisle
LCAAisle::LCAAisle(	const QString sWsId, const QString sAisle, const double frmXCord, const double frmYCord,
	const double toXCord, const double toYCord, const QString sDirectionPick )
{
	msWsId = sWsId;
	msAisle = sAisle;
	mFrmXCord = frmXCord;
	mFrmYCord = frmYCord;
	mToXCord = toXCord;
	mToYCord = toYCord;
	msDirectionPick = sDirectionPick;
	
	mMinXCoord = mFrmXCord < mToXCord ? mFrmXCord : mToXCord;
	mMinYCoord = mFrmYCord < mToYCord ? mFrmYCord : mToYCord;
	mMaxXCoord = mFrmXCord > mToXCord ? mFrmXCord : mToXCord;
	mMaxYCoord = mFrmYCord > mToYCord ? mFrmYCord : mToYCord;

}

// This method constructs a splitted aisle. The original aisle in the WMS may contain additional waypoints
// where the aisle can be entered or left. For each original aisle a set of one or more splitted aisles is
// generated, where each splitted aisle represents the segment between to consecutive waypoints. The 
// splitted aisle is stored in such a way, that the splitted aisle should always be entered from the fromCoords
// and left at the toCoords.
// The coordinates of the splitted aisle should be corrected for the offset of the area in which the aisle is 
// located, e.g., the coordinates of the original aisle dont include the offset, but the coordinates of the
// splitted aisle do. This reduced the number of lookups in het area table during calculations.
LCASplittedAisle::LCASplittedAisle(	const QString sWsId,
							const QString sAisle, 
							const double frmXCord,
							const double frmYCord,
							const double toXCord,
							const double toYCord,
							const int sequenceNumber, 
							const double aisleLength,
							const double fullAisleLength,
							const double minCoordOfFullAisle,
							const double maxCoordOfFullAisle
							)
:LCAAisle( sWsId, sAisle, frmXCord, frmYCord, toXCord, toYCord, "+")
{
	mSequenceNumber = sequenceNumber;
	mAisleLength = aisleLength;
	mFullAisleLength = fullAisleLength;


	mMinCoordOfFullAisle = minCoordOfFullAisle;
	mMaxCoordOfFullAisle = maxCoordOfFullAisle;
}

// This method is used to check wether a location that is allocated to an original aisle, is part of this splitted ailse,
// i.e., to this section of the original aisle. It is only checked if the coordinates of the location in the moving direction
// of the splitted aisle are within bound. It is not checked if the location is close enough to the aisle in the other
// direction.
// The x and y coordinate given, must be corrected for the offset of the area
bool LCASplittedAisle::containsLocation( double x, double y ) const
{
	
	if ( getMovingDirectionIsX() )
	{		
		return ( mMinXCoord <= x && mMaxXCoord >= x );
	}
	else if ( getMovingDirectionIsY() )
	{		
		return ( mMinYCoord <= y && mMaxYCoord >= y );
	}
	else
	{
		// Als de coordinaten van de gang in geen van de richtingen verschillen, dan checken we of x óf y ertussen liggen
		return ( mMinXCoord <= x && mMaxXCoord >= x ) || ( mMinYCoord <= y && mMaxYCoord >= y );
	}
}

// This method returns the distance of the entry point of the splitted aisle to the location
// The xCord and yCord given must be corrected for the offset of the ares
double LCASplittedAisle::getDistanceFromEntry( double xCord, double yCord ) const
{
	adjustCoords( xCord, yCord );

	if ( getMovingDirectionIsX() )
	{
		return abs( xCord - mFrmXCord );
	}
	else
	{
		return abs( yCord - mFrmYCord );
	}
}

// This method returns the distance  the location of the exit point of the splitted aisle
// The xCord and yCord given must be corrected for the offset of the ares
// If the location is outside the splitted aisle, 
double LCASplittedAisle::getDistanceToExit( double xCord, double yCord ) const
{
	adjustCoords( xCord, yCord );

	if ( getMovingDirectionIsX() )
	{
		return abs( xCord - mToXCord );
	}
	else
	{
		return abs( yCord - mToYCord );
	}
}

// Check how far a location that is supposed to be located in the aisle lies outside the aisle
// We only look at the distance in the moving direction
// If the location is inside the aisle, the distance is 0, otherwise it is the distance
// to the nearest entry or exit
double LCASplittedAisle::getDistanceToLocation( double xCord, double yCord ) const
{
	if ( getMovingDirectionIsX() )
	{
		if ( xCord < mMinXCoord ) return mMinXCoord - xCord;
		if ( xCord > mMaxXCoord ) return xCord = mMaxXCoord;
		return 0;
	}
	else
	{
		if ( yCord < mMinXCoord ) return mMinXCoord - yCord;
		if ( yCord > mMaxYCoord ) return yCord - mMaxYCoord;
		return 0;
	}
}

// Adjust the coordinates such that they fall within the bounds of the splitted aisle
void  LCASplittedAisle::adjustCoords( double& xCord, double& yCord ) const
{
	if ( xCord < mMinXCoord ) xCord = mMinXCoord;
	if ( yCord < mMinYCoord ) yCord = mMinYCoord;
	if ( xCord > mMaxXCoord ) xCord = mMaxXCoord;
	if ( yCord > mMaxYCoord ) yCord = mMaxYCoord;
}
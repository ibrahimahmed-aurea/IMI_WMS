#pragma once

#ifndef LCAAISLE_H
#define LCAAISLE_H

#include <QtCore/qstring.h>
#include <set>
#include "lcaerrors.h"

class LCAAisle
{
	public:

		LCAAisle();
		LCAAisle(	const QString sWsId, 
					const QString sAisle, 
					const double frmXCord,
					const double frmYCord,
					const double toXCord,
					const double toYCord,
					const QString sDirectionPick );

		const QString getDirectionPick() const		{ return msDirectionPick; }	
		const bool getMovingDirectionIsX() const	{ if( abs( mFrmXCord - mToXCord ) < gcLCAEpsilon ) return false; else return true; }
		const bool getMovingDirectionIsY() const	{ if( abs( mFrmYCord - mToYCord ) < gcLCAEpsilon ) return false; else return true; }
		const QString getWsId() const				{ return msWsId; } 
		const QString getAisle() const				{ return msAisle; } 

		const bool hasPositivePickDirection() const	{ return msDirectionPick == "+"; }
		const bool hasNegativePickDirection() const	{ return msDirectionPick == "-"; }
		const bool hasMonoDirectionalPick()	const	{ return hasPositivePickDirection() || hasNegativePickDirection(); } 

		const double getMinCoordinateMovingDirection() const { return getMovingDirectionIsX()?mMinXCoord:mMinYCoord; }
		const double getMaxCoordinateMovingDirection() const { return getMovingDirectionIsX()?mMaxXCoord:mMaxYCoord; }

	protected:

		QString msWsId;
		QString msAisle;
		double mFrmXCord;
		double mFrmYCord;
		double mToXCord;
		double mToYCord;
		QString msDirectionPick;

		double mMinXCoord;
		double mMinYCoord;
		double mMaxXCoord;
		double mMaxYCoord;


};

class LCASplittedAisle: public LCAAisle
{
	public:

		LCASplittedAisle(	const QString sWsId,
							const QString sAisle, 
							const double frmXCord,
							const double frmYCord,
							const double toXCord,
							const double toYCord,
							const int sequenceNumber,
							const double aisleLength,
							const double fullAisleLength,
							const double minCoordOfFullAisle,
							const double maxCoordOfFullAisle);

		// get
		const int getSequenceNumber() const			{ return mSequenceNumber; }
		const double getFrmXCord() const			{ return mFrmXCord; }
		const double getFrmYCord() const			{ return mFrmYCord; }
		const double getToXCord() const				{ return mToXCord; }
		const double getToYCord() const				{ return mToYCord; }
		const double getLength() const				{ return mAisleLength; }
		const double getFullLength() const			{ return mFullAisleLength; }
		const double getMinCoordOfFullAisle() const	{ return mMinCoordOfFullAisle; }
		const double getMaxCoordOfFullAisle() const	{ return mMaxCoordOfFullAisle; }


		// methods
		bool containsLocation( double x, double y ) const;
		double getDistanceFromEntry( double xCord, double yCord ) const;
		double getDistanceToExit( double xCord, double yCord ) const;
		double getDistanceToLocation( double xCord, double yCord ) const;
		void  adjustCoords( double& xCord, double& yCord ) const;

	protected:

		int mSequenceNumber;
		double mAisleLength;
		double mFullAisleLength;

		double mMinCoordOfFullAisle; // The smallest coordinate in the moving direction of the full aisle
		double mMaxCoordOfFullAisle; // The largest coordinate in the moving direction of the full aisle
};

#endif
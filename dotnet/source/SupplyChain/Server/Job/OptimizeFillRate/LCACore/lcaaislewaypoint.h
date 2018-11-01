#pragma once

#ifndef LCAAISLEWAYPOINT_H
#define LCAAISLEWAYPOINT_H

#include <QtCore/qstring.h>

class LCAAisleWayPoint
{
	public:

		LCAAisleWayPoint(	const QString sWsId, 
					const QString sAisle,
					const QString sWayPointId,
					const double xCord,
					const double yCord );

		const QString getWayPointId() const { return msWayPointId; }
		const double getXCord() const { return mXCord; }
		const double getYCord() const { return mYCord; }

	protected:

		QString msWsId;
		QString msAisle;
		QString msWayPointId;
		double mXCord;
		double mYCord;
};


class LCAAisleWayPointCompare
{
	public:

		LCAAisleWayPointCompare( const bool hasPositiveDirectionPick, const bool movingDirectionIsX );

		bool operator()(const LCAAisleWayPoint wp1, const LCAAisleWayPoint wp2 ) const;

	protected:

		bool	mHasPositiveDirectionPick;
		bool    mMovingDirectionIsX;
};

#endif
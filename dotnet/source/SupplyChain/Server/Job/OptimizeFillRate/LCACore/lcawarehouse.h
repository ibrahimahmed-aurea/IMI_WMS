#pragma once

#ifndef LCAWAREHOUSE_H
#define LCAWAREHOUSE_H

#include <QtCore/qstring.h>
#include <map>
#include <vector>
#include <set>
#include "lcaarea.h"
#include "lcaaisle.h"
#include "lcaaislewaypoint.h"

class LCAWarehouse
{
	public:

		LCAWarehouse();
		LCAWarehouse( const QString sWHId );
		~LCAWarehouse();

		const QString getWHId() const { return msWHId; }

		int addArea(	const QString sWSId, 
						const double xCord, 
						const double yCord );

		int addAisle(	const QString sWSId, 
						const QString sAisle, 
						const double frmXCord,
						const double frmYCord,
						const double toXCord,
						const double toYCord,
						const QString sDirectionPick );

		int addAisleWayPoint(	const QString sWSId,
								const QString sAisle,
								const QString sWayPointId,
								const double xCord,
								const double yCord );
		
		const LCASplittedAisle* getSplittedAisle( const QString sWsId, const QString sAisle, const double xCoord, const double yCoord ) const;
		std::pair< double, double > getAreaOffset( const QString sWsId )  const;

		double getDistance( const LCASplittedAisle* pAisleFrom, const double xCoordFrom, const double yCoordFrom,  
							const LCASplittedAisle* pAisleTo, const double xCoordTo, const double yCoordTo ) const;
		double getEntryDistance( const LCASplittedAisle* pAisle, const double xCoord, const double yCoord, const double strekX, const double strekY, const bool extraAisle ) const;
		double getExitDistance( const LCASplittedAisle* pAisle, const double xCoord, const double yCoord, const double strekX, const double strekY, const bool extraAisle ) const;

		double getDistanceCrow( const double xCoordFrom, const double yCoordFrom, const double xCoordTo, const double yCoordTo ) const;
		double getEntryDistanceCrow() const;
		double getExitDistanceCrow() const;
		
		bool checkAreaExists( const QString sWsId ) const;
		bool checkAisleExists( const QString sWsId, const QString sAisle ) const;

		int preprocess( std::map<int, QStringList>& rsErrorMessages );

	protected:
		
		friend class LCA;
		
		int createSplittedAisles( std::map<int, QStringList>& rsErrorMessages );

		QString msWHId;
		std::map< QString, LCAArea > mcArea;
		std::map< std::pair< QString, QString >, LCAAisle > mcAisle;									// Pair: WsId, Aisle
		std::map< std::pair< QString, QString >, std::vector<LCAAisleWayPoint> > mcAisleWayPoint;		// Pair: WsId, Aisle
		std::map< std::pair< QString, QString >, std::vector< LCASplittedAisle* > > mcSplittedAisle;	// Pair: WsId, Aisle
};

#endif
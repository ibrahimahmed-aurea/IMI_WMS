#pragma once

#ifndef LCAPICKBATCHLINE_H
#define LCAPICKBATCHLINE_H

#include <QtCore/qstring.h>
#include <vector>
#include <map>

class LCASplittedAisle;

/* This class defines a pick batch line, i.e., a line from a pick batch that defines
 * the amount of pick packages that have to picked of a given SKU.
 * Each pick batch line represents one row in the PBROW table in the WMS. This class
 * only stores the information that is unique for each row in the PBROW table. The
 * information that is common for all rows in the PBROW table will be stored in 
 * the LCAPickBatch class. That class will have a collection of LCAPickBatchLines
 */
class LCAPickBatchLine
{
	public:

		LCAPickBatchLine(	const QString sPBRowId, 
							const int pickSeq,
							const QString sArtId,
							const QString sCompanyId,
							const double ordQty,
							const double volume,
							const double weight,
							const QString sWSId,
							const QString sAisle,
							const QString sArtGroup,
							const QString sCatGroup,
							const double xCord,
							const double yCord,
							const QString msWPAdr );

		// get
		int	getPickSeq() const			{ return mPickSeq; }
		QString getPBRowId() const		{ return msPBRowId; }
		double getWeight() const		{ return mWeight; }
		double getVolume() const		{ return mVolume; }
		double getOrdQty() const		{ return mOrdQty; }
		QString getWsId() const			{ return msWSId; } 
		QString getAisle() const		{ return msAisle; }
		QString getCatGroup() const		{ return msCatGroup; } 
		double getXCord() const			{ return mXCord; }
		double getYCord() const			{ return mYCord; }
		QString getWPAdr() const		{ return msWPAdr; }

	protected:

		QString	msPBRowId;		// Unique Id for this pick batch line
		int		mPickSeq;		// Pick sequence number
		QString msArtId;		// ID of the SKU
		QString msCompanyId;	// Client Id. The combination or ArtID and CompanyID gives a unique SKU ID
		double	mOrdQty;		// The number of ordered pick packages
		double  mVolume;		// The volume of one pick package, including the air correction factor
		double	mWeight;		// The weight of one pick package
		QString	msWSId;			// Unique Id of the warehouse area within the warehouse
		QString msAisle;		// Unique Id of the aisle within the warehouse area
		QString	msArtGroup;		// Unique Id of the assortment group 
		QString msCatGroup;		// Unique Id of the category group
		double	mXCord;			// XCoord of the location, not corrected for offset
		double	mYCord;			// YCoord of the location, not corrected for offset
		QString msWPAdr;		// WP address, use for sorting
};

// This compare operator sorts the elements in ascending order of pick sequence
class LCAPickBatchLineAscendingCompare
{
	public:

		bool operator()(const LCAPickBatchLine * const pLCA1, const LCAPickBatchLine * const pLCA2 ) const
		{
			if ( pLCA1->getPickSeq() != pLCA2->getPickSeq() ) return pLCA1->getPickSeq() < pLCA2->getPickSeq();
			if ( pLCA1->getWsId() != pLCA2->getWsId() ) return pLCA1->getWsId() < pLCA2->getWsId();
			if ( pLCA1->getWPAdr() != pLCA2->getWPAdr() ) return pLCA1->getWPAdr() < pLCA2->getWPAdr();
			return pLCA1->getPBRowId() < pLCA2->getPBRowId();
		}
};

// This compare operator sorts the elements in decending order of weight to volume ratio
class LCAPickBatchLineAlternatingCompare
{
	public:

		bool operator()(const LCAPickBatchLine * const pLCA1, const LCAPickBatchLine * const pLCA2 ) const
		{
			double ratio1 = pLCA1->getWeight() / pLCA1->getVolume();
			double ratio2 = pLCA2->getWeight() / pLCA2->getVolume();

			if ( ratio1 != ratio2 ) return ratio1 > ratio2;
			
			return pLCA1->getPBRowId() < pLCA2->getPBRowId();
		}
};


// This compare operator sorts the elements in decending order of weight to volume ratio
// Within the aisle, sort by descending ratio of the pickbatchline
class LCAPickBatchLineAisleDensityCompare
{
	public:

		LCAPickBatchLineAisleDensityCompare( std::map< std::pair< QString, QString >, double > &rDensityPerAisle )
		{
			mAisleDensity = rDensityPerAisle;
		}

		bool operator()(const LCAPickBatchLine * const pLCA1, const LCAPickBatchLine * const pLCA2 )
		{
			if ( mAisleDensity[ std::make_pair( pLCA1->getWsId(),pLCA1->getAisle() ) ] !=  mAisleDensity[ std::make_pair(pLCA2->getWsId(),pLCA2->getAisle()) ] ) 
				return mAisleDensity[ std::make_pair(pLCA1->getWsId(),pLCA1->getAisle()) ] > mAisleDensity[ std::make_pair(pLCA2->getWsId(),pLCA2->getAisle()) ];

			double ratio1 = pLCA1->getWeight() / pLCA1->getVolume();
			double ratio2 = pLCA2->getWeight() / pLCA2->getVolume();

			if ( ratio1 != ratio2 ) return ratio1 > ratio2;

			if ( pLCA1->getWsId() !=  pLCA2->getWsId() ) return pLCA1->getWsId() <  pLCA2->getWsId();
			if ( pLCA1->getAisle() !=  pLCA2->getAisle() ) return pLCA1->getAisle() <  pLCA2->getAisle();

			if ( pLCA1->getPickSeq() != pLCA2->getPickSeq() ) return pLCA1->getPickSeq() < pLCA2->getPickSeq();
			if ( pLCA1->getWPAdr() != pLCA2->getWPAdr() ) return pLCA1->getWPAdr() < pLCA2->getWPAdr();

			return pLCA1->getPBRowId() < pLCA2->getPBRowId();
		}

	protected:

		std::map< std::pair< QString, QString >, double > mAisleDensity;
};


typedef std::vector< const LCAPickBatchLine* const > LCAPickBatchLineVector;
#endif
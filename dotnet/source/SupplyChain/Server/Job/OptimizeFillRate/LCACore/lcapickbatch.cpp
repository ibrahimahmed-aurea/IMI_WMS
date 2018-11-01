#include "lcapickbatch.h"
#include "lcapickbatchline.h"
#include "lcaerrors.h"

// Create a new LCAPickBatch
LCAPickBatch::LCAPickBatch( const QString sNewGroupId, const QString sWHId, const QString sPZId, const QString sStrekArea, 
	const double strekXCoord, const double strekYCoord )
{
	msNewGroupId = sNewGroupId;
	msWHId = sWHId;
	msPZId = sPZId;
	msStrekArea = sStrekArea;
	mStrekXCoord = strekXCoord;
	mStrekYCoord = strekYCoord;
	mcPickLines.clear();
	mTotalVolume = 0;
	mTotalWeight = 0;
}

// Destruct the LCAPickBatch
LCAPickBatch::~LCAPickBatch()
{
	clear();
}


// Add a new pick batch line to the pick batch
int LCAPickBatch::addPickBatchLine( const QString sPBRowId, const int pickSeq, const QString sArtId, const QString sCompanyId, 
	const double ordQty, const double volume, const double weight, const QString sWSId, const QString sAisle,
	const QString sArtGroup, const QString sCatGroup, const double xCord, const double yCord, const QString sWPAdr )
{	
	// First create the new pick batch line in memory
	LCAPickBatchLine* pLine = new LCAPickBatchLine( sPBRowId, pickSeq, sArtId, sCompanyId, ordQty,
		volume, weight, sWSId, sAisle, sArtGroup, sCatGroup, xCord, yCord, sWPAdr );

	// If the memory cannot be allocated, return a memory error
	if ( pLine == 0 ) return gcLCAErrorNoMemory;

	// Add the newly created pick batch line to the collection
	mcPickLines.push_back( pLine );

	mTotalVolume += pLine->getOrdQty() * pLine->getVolume();
	mTotalWeight += pLine->getOrdQty() * pLine->getWeight();

	if ( pLine->getVolume() == 0 ) return gcLCAErrorNonPositiveVolume;
	if ( pLine->getWeight() == 0 ) return gcLCAErrorNonPositiveWeight;
	if ( pLine->getOrdQty() == 0 ) return gcLCAErrorNonPositiveQuantity;
	if ( sArtGroup == "" ) return gcLCAErrorNoArtGroup;
	if ( sCatGroup == "" ) return gcLCAErrorNoCatGroup;

	return gcLCAOk;
}


QString LCAPickBatch::getNewGroupId() const
{ 
	return msNewGroupId; 
} 
		
QString LCAPickBatch::getWHId() const
{
	return msWHId;
}

int LCAPickBatch::getPickLineCount() const 
{ 
	return mcPickLines.size(); 
}
		
const LCAPickBatchLine* const LCAPickBatch::getPickLine( int pickLineNr ) const
{
	if ( pickLineNr < 0 ) return 0;
	if ( pickLineNr >= mcPickLines.size() ) return 0;
	return mcPickLines[ pickLineNr ];
}

const double LCAPickBatch::getTotalVolume() const
{
	return mTotalVolume;
}
		
const double LCAPickBatch::getTotalWeight() const
{
	return mTotalWeight;
}

// Clear the contents of the LCAPickBatch
void LCAPickBatch::clear()
{
	// Delete all referenced pick batch lines and clear the collection
	for ( int i=0; i<mcPickLines.size(); i++ )
	{
		if ( mcPickLines[i] ) 
		{
			delete mcPickLines[i];
			mcPickLines[i] = 0;
		}
	}
	mcPickLines.clear();

	// Reset all fields to default values
	msNewGroupId.clear();
	msWHId.clear();
	msPZId.clear();
	mStrekXCoord = 0;
	mStrekYCoord = 0;
	mTotalVolume = 0;
	mTotalWeight = 0;
}

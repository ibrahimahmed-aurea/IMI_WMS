#include "lcaannealpickbatchline.h"
#include "lcapickbatchline.h"
#include "lcawarehouse.h"

// This creates a pickbatch line to be used in the annealing. It has a reference to the original pickbatch line and a 
// quantity. In this way, we can have multiple pickbatchlines in the annealling that together represent one original
// pickbatch line. The summed quantity of all annealpickbatchlines is equal to the volume of the original pick batch
// line. The anneal pickbatchline is the smallest unit that can be moved between loadcarriers.
// In order to speed up computation, the total volume and total weight are calculated and a unique id is assigned
LCAAnnealPickBatchLine::LCAAnnealPickBatchLine( const LCAPickBatchLine* pPickBatchLine, double ordQty, int uniqueId, const LCAWarehouse& rWarehouse )
{
	mpPickBatchLine = pPickBatchLine;
	mOrdQty = ordQty;
	mTotalVolume = mOrdQty * pPickBatchLine->getVolume();
	mTotalWeight = mOrdQty * pPickBatchLine->getWeight();
	mUniqueId = uniqueId;
	mpSplittedAisle = rWarehouse.getSplittedAisle( mpPickBatchLine->getWsId(), mpPickBatchLine->getAisle(), mpPickBatchLine->getXCord(), mpPickBatchLine->getYCord() );
	std::pair<double,double> offSet = rWarehouse.getAreaOffset( mpPickBatchLine->getWsId() );
	mAdjustedXCoord = mpPickBatchLine->getXCord() + offSet.first;
	mAdjustedYCoord = mpPickBatchLine->getYCord() + offSet.second;
}

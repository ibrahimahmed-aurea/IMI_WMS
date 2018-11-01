#include "lcaarea.h"

// This represents an area of the warehouse in the algorithm.
// It is only used to calculate the offset of the x and y coordinates
LCAArea::LCAArea()
{
	msWSId.clear();
	mXCord = 0.0;
	mYCord = 0.0;
}

LCAArea::LCAArea( const QString sWSId, const double xCord, const double yCord )
{
	msWSId = sWSId;
	mXCord = xCord;
	mYCord = yCord;
}
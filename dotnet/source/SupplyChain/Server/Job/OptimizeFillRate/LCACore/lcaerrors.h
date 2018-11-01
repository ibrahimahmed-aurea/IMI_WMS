#pragma once

#ifndef LCAERRORS_H
#define LCAERRORS_H

static const double gcLCAEpsilon = 0.00000001;						// Anything below epsilon, we regard as non-positive

// Succesfull completion codes
static const int gcLCAOk = 0;										// Success

// Warning codes: 
static const int gcLCATimeout = 1;									// Success, but with a timeout. Solution might be low quality

// proces can continue, but distance cannot be calculated

// Layout warnings
static const int gcLCAErrorReferenceToNonExistingArea = 801;		// A location, waypoint of aisle refers to an area that is not part of the dataset
static const int gcLCAErrorWarehousePreprocessCalledTwice = 202;	// The preprocessing function of the LCA Warehouse has been called too many times
static const int gcLCAErrorReferenceToNonExistingAisle = 804;		// A location refers to an aisle that is not part of the dataset
static const int gcLCAErrorBidirectionalPick = 205;					// An aisle with bidirectional pickdirection was added, this is not supported
static const int gcLCAErrorBidirectionalMove = 206;					// An aisle with bidirectional moving direction was added, this is not supported

// Pickbatch warnings
static const int gcLCAErrorWarehouseMismatch = 800;					// The warehouse for the pickbatch and the LCA do not match
static const int gcLCAErrorTooLargePickBatchLine = 301;				// The pickbatch line is too big to fit on a load carrier
static const int gcLCAErrorTooLargePickBatch = 302;					// The pickbatch has too many orderlines to run the full algorithm
static const int gcLCAErrorStrekAreaUnknown = 803;					// The pickbatch refers to an area for the strek that is not in the warehouse
static const int gcLCAErrorLocationOutsideAisle = 104;				// The pickbatch has x,y coordinates outside the supplied aisle
static const int gcLCAErrorNonPositiveWeight = 105;					// Pickbatch lines must have positive weight
static const int gcLCAErrorNonPositiveVolume = 306;					// Pickbatch lines must have positive volume
static const int gcLCAErrorNonPositiveQuantity = 307;				// Pickbatch lines must have positive quantity
static const int gcLCAErrorNoArtGroup = 108;						// No assortmentgroup is filled in
static const int gcLCAErrorNoCatGroup = 109;						// No categorygroup is filled in

static const int gcLCAMaxAcceptableLCAWarning = 199;				// For any warning code above this the new LCA will not be able to run
static const int gcLCAMaxAcceptableWarning = 499;					// For any warning code above this the LCA will not be able to calculate any distances
static const int gcLCAMaxAcceptableError = 999;						// Any error code above this will make the LCA return no result

// Error codes: 
// proces cannot continue

// General errors
static const int gcLCAErrorNoMemory = 1000;							// Failed to allocate memory

// Pickbatch errors
static const int gcLCAErrorNoPickbatch = 1303;						// No pickbatch is supplied ( pointer is null )

// Parameter errors
static const int gcLCAErrorNoParameters = 1400;						// No parameters are supplied ( pointer is null )
static const int gcLCAErrorParameterOutOfBounds = 1401;				// A parameter is out of bounds

#endif
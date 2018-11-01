#include "lcaparameters.h"

// Construct a LCAParameters object
LCAParameters::LCAParameters(	const double maxLDVol, const double maxLdWgt, const int maxPBRowCar, const bool allowPBRowSplit,
	const double distanceFactor, const int numberOfIterationsLC, const int numberOfIterationsBeauty, const int numberOfIterationsDistance, 
	const bool onlyAptean, const bool doLCPhase, const bool doBeautyPhase, const bool doDistPhase, const int maxmSecLC, const int maxmSecBeauty,
	const int maxmSecDistance )
{
	mMaxLDVol = maxLDVol;
	mMaxLDWgt = maxLdWgt;
	mMaxPBRowCar = maxPBRowCar;
	mAllowPBRowSplit = allowPBRowSplit;
	mDistanceFactor = distanceFactor;
	mNumberOfIterationsLC = numberOfIterationsLC;
	mNumberOfIterationsBeauty = numberOfIterationsBeauty;
	mNumberOfIterationsDistance = numberOfIterationsDistance;
	mOnlyAptean = onlyAptean;
	mDoLCPhase = doLCPhase;
	mDoBeautyPhase = doBeautyPhase;
	mDoDistPhase = doDistPhase;
	mMaxmSecsLC = maxmSecLC;
	mMaxmSecsBeauty = maxmSecBeauty;
	mMaxmSecsDistance = maxmSecDistance;
}
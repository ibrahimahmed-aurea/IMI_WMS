/* This class defines the parameters the LCA should take into account when compiling
 * load carriers for a LCAPickBatch.
 */

#pragma once

#ifndef LCAPARAMETERS_H
#define LCAPARAMETERS_H

class LCAParameters
{
	public:

		LCAParameters(	const double maxLDVol, 
						const double maxLdWgt, 
						const int maxPBRowCar, 
						const bool allowPBRowSplit,
						const double distanceFactor,
						const int numberOfIterationsLC,
						const int numberOfIterationsBeauty,
						const int numberOfIterationsDistance,
						const bool onlyAptean,
						const bool doLCPhase,
						const bool doBeautyPhase,
						const bool doDistPhase,
						const int maxmSecLC, 
						const int maxmSecBeauty,
						const int maxmSecDistance );
		
		double getMaxLDVol() const				{ return mMaxLDVol; }
		double getMaxLDWgt() const				{ return mMaxLDWgt; }
		int    getMaxPBRowCar() const			{ return mMaxPBRowCar; }
		bool   getAllowPBRowSplit() const		{ return mAllowPBRowSplit; }
		double getDistanceFactor() const		{ return mDistanceFactor; }
		int	   getIterationsLC() const			{ return mNumberOfIterationsLC; }
		int	   getIterationsBeauty() const		{ return mNumberOfIterationsBeauty; }
		int	   getIterationsDistance() const	{ return mNumberOfIterationsDistance; }
		bool   getOnlyAptean() const			{ return mOnlyAptean; }
		bool   getDoLCPhase() const				{ return mDoLCPhase; }
		bool   getDoBeautyPhase() const			{ return mDoBeautyPhase; }
		bool   getDoDistPhase() const			{ return mDoDistPhase; }
		int    getMaxmSecsLC() const			{ return mMaxmSecsLC; }
		int    getMaxmSecsBeauty() const			{ return mMaxmSecsBeauty; }
		int    getMaxmSecsDistance() const			{ return mMaxmSecsDistance; }

	protected:

		double	mMaxLDVol;						// The maximum volume of goods that can be placed on one load carrier
		double	mMaxLDWgt;						// The maximum weight of goods that can be placed on one load carrier
		int		mMaxPBRowCar;					// The maximum number of pick batch lines that can be placed on one load carrier
		bool	mAllowPBRowSplit;				// true if one pick batch line may be split over multiple load carriers, false if not
		double	mDistanceFactor;				// Maximum allowed factor of increase on the distance travelled, compared to the current algorithm
		bool    mOnlyAptean;					// Only perform the aptean phase.
		bool	mDoLCPhase;						// If annealing is performed, include the minimize LC's step
		bool	mDoBeautyPhase;					// If annealing is performed, include the maximization of beauties
		bool	mDoDistPhase;					// If annealing is performed, include the minimization of distance
		int		mNumberOfIterationsLC;			// The number of iterations that the annealing phase is allowed to do per load carriers for the LC phase
		int		mNumberOfIterationsBeauty;		// The number of iterations that the annealing phase is allowed to do per load carriers for the Beauty phase
		int		mNumberOfIterationsDistance;	// The number of iterations that the annealing phase is allowed to do per load carriers for the Distance phase
		int		mMaxmSecsLC;					// The maximumum time spend on optimizing load carriers in the LC phase of annealing per load carrier
		int		mMaxmSecsBeauty;				// The maximumum time spend on optimizing load carriers in the Beauty phase of annealing per load carrier
		int		mMaxmSecsDistance;				// The maximumum time spend on optimizing load carriers in the Distance phase of annealing per load carrier
};

#endif
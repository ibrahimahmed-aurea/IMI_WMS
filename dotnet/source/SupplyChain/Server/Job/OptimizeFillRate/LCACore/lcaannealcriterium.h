#pragma once

#ifndef LCAANNEALCRITERIUM_H
#define LCAANNEALCRITERIUM_H

#include "lcaanneallc.h"
#include "lcaerrors.h"

#include <vector>

// The LCAAnnealCriterium is used to determine if a solution found during the simulated annealing process is valid and what the value
// of this solution is. The value of a solution is used to determine whether one solution is to be preferred over another solution 
// and how much better or worse it is.
// For the different steps in the simulated annealing process, different criteria can be used. This is done by implementing the 
// evaluate method that must set the following information:
// - mValue: the value of the given load carrier configuration
// - mFinished: true if and only if the given load carrier configuration is guaranteed to be optimal with respect to the criterium
// Furthermore if updateStartValues is true, it must set
// - mMaxIterations: the maximum number of iterations that the simulated annealing algorithm may perform using this criterium
// - mStartCool: the initial cooling temperature of the annealing process
class LCAAnnealCriterium
{
	public:

		LCAAnnealCriterium( double maximumDistance, double maxVolume, double maxWeight, int maxLines, int minBeauties, int iterationsPerLC );

		virtual QString name() const = 0;
		virtual void	evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues ) = 0;
		
		
		
		double	value()	const				{ return mValue; } 
		bool	finished() const			{ return mFinished; }
		bool	valid()	const				{ return mValid; } 
		int		maxIterations() const		{ return mMaxIterations; }
		double	startCool() const			{ return mStartCool; }
		int     loadCarriers() const		{ return mLoadCarrierCount; }
		int     beauties() const			{ return mBeautyCount; }
		double  distancePercentage() const	{ return 100 * mTotalDistance / mMaximumDistance; };
		int     resultLines() const			{ return mResultLines; }
		double  fillRateSmallestLC() const	{ return 100*mVolumeSmallestLC/mMaxVolume; }

	protected:

		void updateValues( std::vector< LCAAnnealLC >& rcLoadCarriers );

		// These values are updated by updateValues
		bool	mValid;
		double  mTotalDistance;
		int		mLoadCarrierCount;
		int     mLoadCarriersAboutVolume;
		int     mLoadCarriersAboutWeight;
		int     mLoadCarriersAboutLineCount;
		int		mBeautyCount;
		int     mResultLines;
		double  mVolumeSmallestLC;

		// These values must be set by the evaluate method, after calling updateValues()
		double	mValue;
		bool	mFinished;

		// Values that are only set when updateStartValues == true
		int		mMaxIterations;
		double	mStartCool;

		// Parameter values
		double	mMaximumDistance;
		double	mMaxVolume;
		double  mMaxWeight;
		int		mMinBeauties;
		int     mMaxLines;
		int		mIterationsPerLC;
};

// This is the criterium used to minimize the number of load carriers
class LCAAnnealCriteriumMinimizeLCs: public LCAAnnealCriterium
{
	public:

		LCAAnnealCriteriumMinimizeLCs( int optimumLoadCarriers, double maximumDistance, double maxVolume, 
			double maxWeight, int maxLines, int iterationsPerLC );

		virtual void   evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues );
		virtual QString name() const { return "#LCs"; }

	protected:

		int		mOptimumLoadCarriers;
};

// This is the criterium used to maximize store friendly delivery
class LCAAnnealCriteriumSFD: public LCAAnnealCriterium
{
	public:

		LCAAnnealCriteriumSFD( double maximumDistance, double maxVolume, double maxWeight, int maxLines, int iterationsPerLC );

		virtual void   evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues );
		virtual QString name() const { return "SFD "; }

	protected:

		void calcOptimumBeauties( std::vector< LCAAnnealLC >& rcLoadCarriers );

		int mOptimumBeauties;

};

// This is the criterium used to minimize distance
class LCAAnnealCriteriumMinimizeDist: public LCAAnnealCriterium
{
	public:

		LCAAnnealCriteriumMinimizeDist( double maximumDistance, double maxVolume, double maxWeight, int maxLines, 
			int minBeauties, int iterationsPerLC );

		virtual void   evaluate( std::vector< LCAAnnealLC >& rcLoadCarriers, bool updateStartValues );
		virtual QString name() const { return "Dist"; }

	protected:

		double mMaxResultLines;

};

#endif
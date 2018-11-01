#pragma once

using namespace System;
using namespace System::Text;

namespace LCAInterop
{
	public ref class LCASettings
	{
		public:
			LCASettings(void);
			virtual String^ ToString(void) override;

			double	MaxLDVol;						// The maximum volume of goods that can be placed on one load carrier
			double	MaxLDWgt;						// The maximum weight of goods that can be placed on one load carrier
			int		MaxPBRowCar;					// The maximum number of pick batch lines that can be placed on one load carrier
			bool	AllowPBRowSplit;				// true if one pick batch line may be split over multiple load carriers, false if not
			double	DistanceFactor;					// Maximum allowed factor of increase on the distance travelled, compared to the current algorithm
			bool    OnlyAptean;						// Only perform the aptean phase.
			bool	DoLCPhase;						// If annealing is performed, include the minimize LC's step
			bool	DoBeautyPhase;					// If annealing is performed, include the maximization of beauties
			bool	DoDistPhase;					// If annealing is performed, include the minimization of distance
			int		NumberOfIterationsLC;			// The number of iterations that the annealing phase is allowed to do per load carriers for the LC phase
			int		NumberOfIterationsBeauty;		// The number of iterations that the annealing phase is allowed to do per load carriers for the Beauty phase
			int		NumberOfIterationsDistance;		// The number of iterations that the annealing phase is allowed to do per load carriers for the Distance phase
			int		MaxmSecLC;						// The maximumum time spend on optimizing load carriers in the LC phase of annealing per load carrier
			int		MaxmSecBeauty;					// The maximumum time spend on optimizing load carriers in the Beauty phase of annealing per load carrier
			int		MaxmSecDistance;				// The maximumum time spend on optimizing load carriers in the Distance phase of annealing per load carrier
			double  StrekXCoord;
			double  StrekYCoord;
			String^ WHId;
			String^ PZId;
			String^ GroupId;
			String^ StrekArea;
	};
}
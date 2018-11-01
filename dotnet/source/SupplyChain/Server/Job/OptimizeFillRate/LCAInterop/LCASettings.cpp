#include "LCASettings.h"

namespace LCAInterop
{
	LCASettings::LCASettings(void)
	{
		
	}
	
	String^ LCASettings::ToString(void)
	{
		StringBuilder^ sb = gcnew StringBuilder();

		sb->AppendLine(String::Format("MaxLDVol={0}", MaxLDVol));
		sb->AppendLine(String::Format("MaxLDWgt={0}", MaxLDWgt));
		sb->AppendLine(String::Format("MaxPBRowCar={0}", MaxPBRowCar));
		sb->AppendLine(String::Format("AllowPBRowSplit={0}", AllowPBRowSplit));
		sb->AppendLine(String::Format("DistanceFactor={0}", DistanceFactor));
		sb->AppendLine(String::Format("OnlyAptean={0}", OnlyAptean));
		sb->AppendLine(String::Format("DoLCPhase={0}", DoLCPhase));
		sb->AppendLine(String::Format("DoBeautyPhase={0}", DoBeautyPhase));
		sb->AppendLine(String::Format("DoDistPhase={0}", DoDistPhase));
		sb->AppendLine(String::Format("NumberOfIterationsLC={0}", NumberOfIterationsLC));
		sb->AppendLine(String::Format("NumberOfIterationsBeauty={0}", NumberOfIterationsBeauty));
		sb->AppendLine(String::Format("NumberOfIterationsDistance={0}", NumberOfIterationsDistance));
		sb->AppendLine(String::Format("MaxmSecLC={0}", MaxmSecLC));
		sb->AppendLine(String::Format("MaxmSecBeauty={0}", MaxmSecBeauty));
		sb->AppendLine(String::Format("MaxmSecDistance={0}", MaxmSecDistance));
		sb->AppendLine(String::Format("StrekXCoord={0}", StrekXCoord));
		sb->AppendLine(String::Format("StrekYCoord={0}", StrekYCoord));
		sb->AppendLine(String::Format("WHId={0}", WHId));
		sb->AppendLine(String::Format("PZId={0}", PZId));
		sb->AppendLine(String::Format("GroupId={0}", GroupId));
		sb->AppendLine(String::Format("StrekArea={0}", StrekArea));

		return sb->ToString();
	}
	
}
#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace LCAInterop
{
	public ref class LCAWrapperResultLine
	{
		public:
			String^		PBRowId;			// Unique id of the pick batch line within a pick batch
			String^		PBRowSplitId;		// Unique id for the split of the pick batch line. Empty is not splitted
			double		OrdQty;				// Number of the pick packages in this result
			String^		PBCarIdVirtual;		// Unique id for the virtual load carrier 
			int			PickSeq;			// The original pick sequence
			String^		CatGroup;			// The Categorygroup of the pick line
			String^		WsId;				// The area for the pick location
			String^		Aisle;				// The aisle for the pick location
			double		XCoord;				// The x coordinate of the picklocation, NOT corrected for offset of the area
			double		YCoord;				// The y coordinate of the picklocation, NOT corrected for offset of the area
			String^		WPAdr;				// The WPAdress of the result line
		
			LCAWrapperResultLine(void);
	};

	public ref class LCAWrapperResult
	{
		private:
			List<LCAWrapperResultLine^>^ _hResultLines;
			Dictionary<int, List<String^>^>^ _hErrors;
		public:

			LCAWrapperResult(List<LCAWrapperResultLine^>^ hResultLines, Dictionary<int, List<String^>^>^ hErrors);

			property List<LCAWrapperResultLine^>^ Lines
			{
				List<LCAWrapperResultLine^>^ get();
			}

			property Dictionary<int, List<String^>^>^ Errors
			{
				Dictionary<int, List<String^>^>^ get();
			}
	};
}
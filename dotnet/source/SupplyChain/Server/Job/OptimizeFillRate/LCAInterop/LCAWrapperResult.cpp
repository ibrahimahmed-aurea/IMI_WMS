#include "LCAWrapperResult.h"

namespace LCAInterop
{
	LCAWrapperResultLine::LCAWrapperResultLine()
	{
	}

	List<LCAWrapperResultLine^>^ LCAWrapperResult::Lines::get()
	{
		return _hResultLines;
	}


	Dictionary<int, List<String^>^>^ LCAWrapperResult::Errors::get()
	{
		return _hErrors;
	}


	LCAWrapperResult::LCAWrapperResult(List<LCAWrapperResultLine^>^ hResultLines, Dictionary<int, List<String^>^>^ hErrors)
	{
		_hResultLines = hResultLines;
		_hErrors = hErrors;
	}
}
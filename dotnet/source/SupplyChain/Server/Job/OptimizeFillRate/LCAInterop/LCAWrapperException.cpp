#include "LCAWrapperException.h"

using namespace System;
using namespace System::Collections::Generic;

namespace LCAInterop
{
	LCAWrapperException::LCAWrapperException(String^ message, int errorCode) 
		: Exception(String::Format("{0}. Error Code = {1}.", message, errorCode))
	{
		_errorCode = errorCode;
	}

	int LCAWrapperException::ErrorCode::get()
	{
		return _errorCode;
	}
	
}
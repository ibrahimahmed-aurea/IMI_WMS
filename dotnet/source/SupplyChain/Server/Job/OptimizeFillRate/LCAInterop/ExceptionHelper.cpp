#include "lcaerrors.h"
#include "ExceptionHelper.h"
#include "LCAWrapperException.h"

namespace LCAInterop
{
	void ExceptionHelper::ThrowIfError(int errorCode)
	{
		if (errorCode > gcLCAMaxAcceptableError)
		{
			if (errorCode == gcLCAErrorNoMemory)
			{
				throw gcnew LCAWrapperException("Failed to allocate memory.", errorCode);
			}
			else if (errorCode == gcLCAErrorNoPickbatch)
			{
				throw gcnew LCAWrapperException("No pick order lines were supplied to the algorithm.", errorCode);
			}
			else if (errorCode == gcLCAErrorNoParameters)
			{
				throw gcnew LCAWrapperException("No parameters were supplied to the algorithm.", errorCode);
			}
			else if (errorCode == gcLCAErrorParameterOutOfBounds)
			{
				throw gcnew LCAWrapperException("A parameter to the algorithm is out of bounds.", errorCode);
			}
			else
			{
				throw gcnew LCAWrapperException("Unexpected Error.", errorCode);
			}
		}
	}
}
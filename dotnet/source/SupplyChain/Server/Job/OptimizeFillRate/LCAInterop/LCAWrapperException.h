#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace LCAInterop
{
	public ref class LCAWrapperException : Exception
	{
		private:
			int _errorCode;
		public:

			LCAWrapperException(String^ message, int errorCode);

			property int ErrorCode
			{
				int get();
			}
	};
}
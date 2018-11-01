#pragma once

using namespace System;

namespace LCAInterop
{
	ref class ExceptionHelper
	{
		public:
			static void ThrowIfError(int errorCode);
	};
}

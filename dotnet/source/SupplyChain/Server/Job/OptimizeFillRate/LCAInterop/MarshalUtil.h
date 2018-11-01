#pragma once

#include <msclr\marshal_cppstd.h>
#include <map>
#include <QtCore\qstring.h>

using namespace System;
using namespace msclr::interop;
using namespace System::Collections::Generic;

namespace LCAInterop
{
	ref class MarshalUtil
	{
		public:
	
			static const QString ToQString(String^ str);

			static String^ ToString(const QString str);
		
		private:

			MarshalUtil(void);
	};
}
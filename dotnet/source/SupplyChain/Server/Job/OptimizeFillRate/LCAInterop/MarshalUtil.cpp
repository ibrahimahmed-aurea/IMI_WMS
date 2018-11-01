#include "MarshalUtil.h"

namespace LCAInterop
{
	const QString MarshalUtil::ToQString(String^ str)
	{
		if (str != nullptr)
		{
			std::wstring temp = marshal_as<std::wstring>(str);
			QString ret = QString::fromWCharArray( temp.c_str() );

			return ret;
		}
		else
		{
			return QString();
		}
	}

	String^ MarshalUtil::ToString(const QString str)
	{
		if (str.length() > 0)
		{
			std::wstring temp = L""; 
			temp.resize(str.length());temp.resize(str.toWCharArray(&(*temp.begin())));

			return marshal_as<String^>(temp); 
		}
		else
		{
			return gcnew String("");
		}
	}
	
	MarshalUtil::MarshalUtil(void)
	{
	}
}


#include "MarshalUtil.h"
#include "LCAWrapper.h"
#include "ExceptionHelper.h"
#include "LCAWrapperResult.h"
#include "lcaresult.h"
#include "lcaresultline.h"
#include "lcaparameters.h"
#include <QtCore/qstringlist.h>

namespace LCAInterop
{
	LCAWrapper::LCAWrapper(LCASettings^ hLCASettings)
	{
		_hLCASettings = hLCASettings;

		_hErrors = gcnew Dictionary<int, List<String^>^>();

		_pLCAWarehouse = new LCAWarehouse(MarshalUtil::ToQString(_hLCASettings->WHId)); 

		_pLCAPickBatch = new LCAPickBatch(MarshalUtil::ToQString(_hLCASettings->GroupId),
										  MarshalUtil::ToQString(_hLCASettings->WHId), 
										  MarshalUtil::ToQString(_hLCASettings->PZId), 
										  MarshalUtil::ToQString(_hLCASettings->StrekArea), 
										  _hLCASettings->StrekXCoord, 
										  _hLCASettings->StrekYCoord); 
		_pLCA = new LCA();
	}

	LCAWrapper::~LCAWrapper()
	{
		delete _pLCAWarehouse;
		delete _pLCAPickBatch;
		delete _pLCA;
	}

	void LCAWrapper::AddArea(String^ sWSId, const double xCord, const double yCord)
	{
		int ret = _pLCAWarehouse->addArea(MarshalUtil::ToQString(sWSId), xCord, yCord);
			
		ExceptionHelper::ThrowIfError(ret);
	}

	void LCAWrapper::AddAisle(String^ sWSId, String^ sAisle, const double frmXCord, const double frmYCord, const double toXCord, const double toYCord, String^ sDirectionPick)
	{
		int ret = _pLCAWarehouse->addAisle(MarshalUtil::ToQString(sWSId), MarshalUtil::ToQString(sAisle), frmXCord, frmYCord, toXCord, toYCord, MarshalUtil::ToQString(sDirectionPick));

		ExceptionHelper::ThrowIfError(ret);
	}
	
	void LCAWrapper::AddAisleWayPoint(String^ sWSId, String^ sAisle, String^ sWayPointId, const double xCord, const double yCord)
	{
		int ret = _pLCAWarehouse->addAisleWayPoint(MarshalUtil::ToQString(sWSId), MarshalUtil::ToQString(sAisle), MarshalUtil::ToQString(sWayPointId), xCord, yCord);

		ExceptionHelper::ThrowIfError(ret);
	}

	void LCAWrapper::AddPickBatchLine(String^ sPBRowId, const int pickSeq, String^ sArtId, String^ sCompanyId, 
					const double ordQty, const double volume, const double weight, String^ sWSId, String^ sAisle,
					String^ sArtGroup, String^ sCatGroup, const double xCord, const double yCord, String^ sWPAdr)
	{
		int ret = _pLCAPickBatch->addPickBatchLine(MarshalUtil::ToQString(sPBRowId), pickSeq, MarshalUtil::ToQString(sArtId), MarshalUtil::ToQString(sCompanyId), 
													ordQty, volume, weight, MarshalUtil::ToQString(sWSId), MarshalUtil::ToQString(sAisle),
													MarshalUtil::ToQString(sArtGroup), MarshalUtil::ToQString(sCatGroup), xCord, yCord, MarshalUtil::ToQString(sWPAdr));
		ExceptionHelper::ThrowIfError(ret);
				
		if (ret == gcLCAErrorNoArtGroup)
		{
			if (!_hErrors->ContainsKey(gcLCAErrorNoArtGroup))
			{
				_hErrors[gcLCAErrorNoArtGroup] = gcnew List<String^>();
			}
			
			_hErrors[gcLCAErrorNoArtGroup]->Add(String::Format("The Assortment Group is missing for Pick Order Line \"{0}\".", sPBRowId));
		}
		else if (ret == gcLCAErrorNoCatGroup)
		{
			if (!_hErrors->ContainsKey(gcLCAErrorNoCatGroup))
			{
				_hErrors[gcLCAErrorNoCatGroup] = gcnew List<String^>();
			}
			
			_hErrors[gcLCAErrorNoCatGroup]->Add(String::Format("The Category Group is missing for Pick Order Line \"{0}\".", sPBRowId));
		}
	}

	LCAWrapperResult^ LCAWrapper::Process()
	{
		LCAParameters* params = new LCAParameters(_hLCASettings->MaxLDVol, 
													_hLCASettings->MaxLDWgt, 
													_hLCASettings->MaxPBRowCar, 
													_hLCASettings->AllowPBRowSplit,
													_hLCASettings->DistanceFactor,
													_hLCASettings->NumberOfIterationsLC,
													_hLCASettings->NumberOfIterationsBeauty,
													_hLCASettings->NumberOfIterationsDistance,
													_hLCASettings->OnlyAptean,
													_hLCASettings->DoLCPhase,
													_hLCASettings->DoBeautyPhase,
													_hLCASettings->DoDistPhase,
													_hLCASettings->MaxmSecLC, 
													_hLCASettings->MaxmSecBeauty,
													_hLCASettings->MaxmSecDistance);
		try
		{
			std::map<int, QStringList> rsErrorMessages;
		
			//Initialize C++ LCA
			int ret = _pLCA->setWarehouse(*_pLCAWarehouse, rsErrorMessages);
				
			//Throw exception if critical error
			ExceptionHelper::ThrowIfError(ret);
					
			//Map error messages
			for (std::map<int, QStringList>::iterator iError = rsErrorMessages.begin();
					iError != rsErrorMessages.end(); ++iError )
			{
				if (!_hErrors->ContainsKey(iError->first))
				{
					_hErrors[iError->first] = gcnew List<String^>();
				}

				for (int i = 0; i < iError->second.size(); i++)
				{
					_hErrors[iError->first]->Add(MarshalUtil::ToString(iError->second.at(i)));
				}
			}

			//Process batch using C++ LCA
			LCAResult* pResult = _pLCA->processPickBatch(_pLCAPickBatch, params);

			try
			{
				ret = pResult->getResultCode();
			
				//Throw exception if critical error
				ExceptionHelper::ThrowIfError(ret);
			
				//Map error messages
				std::map<int, std::set<QString>> cErrors = pResult->getErrors();
										
				for (std::map<int, std::set<QString>>::iterator iError = cErrors.begin();
					iError != cErrors.end(); ++iError )
				{
					if (!_hErrors->ContainsKey(iError->first))
					{
						_hErrors[iError->first] = gcnew List<String^>();
					}

					for (std::set<QString>::iterator iText = iError->second.begin();
					iText != iError->second.end(); ++iText )
					{
						_hErrors[iError->first]->Add(MarshalUtil::ToString(*iText));
					}
				}
			
				//Map result
				List<LCAWrapperResultLine^>^ hResultLines = gcnew List<LCAWrapperResultLine^>();
				
				int mLines = pResult->getResultLineCount();

				for (int i = 0; i < mLines; i++)
				{
					const LCAResultLine* pResultLine = pResult->getResultLine(i);
					LCAWrapperResultLine^ hResultLine = gcnew LCAWrapperResultLine();
					
					hResultLine->PBRowId = MarshalUtil::ToString(pResultLine->getPBRowId());
					hResultLine->PBCarIdVirtual = MarshalUtil::ToString(pResultLine->getPBCarIdVirtual());
					hResultLine->PBRowSplitId = MarshalUtil::ToString(pResultLine->getPBRowSplitId());
					hResultLine->CatGroup = MarshalUtil::ToString(pResultLine->getCatGroup());
					hResultLine->WsId = MarshalUtil::ToString(pResultLine->getWsId());
					hResultLine->Aisle = MarshalUtil::ToString(pResultLine->getAisle());
					hResultLine->WPAdr = MarshalUtil::ToString(pResultLine->getWPAdr());
					hResultLine->PickSeq = pResultLine->getPickSeq();
					hResultLine->OrdQty = pResultLine->getOrdQty();
					hResultLine->XCoord = pResultLine->getXCoord();
					hResultLine->YCoord = pResultLine->getYCoord();

					hResultLines->Add(hResultLine);
				}
				
				LCAWrapperResult^ hResult = gcnew LCAWrapperResult(hResultLines, _hErrors);

				return hResult;
			}
			finally
			{
				delete pResult;
			}
		}
		finally
		{
			delete params;
		}
	}

	String^ LCAWrapper::StringMarshallingTest(String^ str)
	{
		QString qStr = MarshalUtil::ToQString(str);
		return MarshalUtil::ToString(qStr);
	}
}
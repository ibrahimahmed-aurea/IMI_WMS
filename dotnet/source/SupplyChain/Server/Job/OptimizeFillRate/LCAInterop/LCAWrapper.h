#pragma once

#include "lca.h"
#include "lcawarehouse.h"
#include "lcapickbatch.h"
#include "LCAWrapperResult.h"
#include "LCASettings.h"

using namespace System;

namespace LCAInterop
{
	public ref class LCAWrapper : IDisposable
	{

		private:

			LCAWarehouse* _pLCAWarehouse;
			LCAPickBatch* _pLCAPickBatch;
			LCA* _pLCA;
			LCASettings^ _hLCASettings;
			Dictionary<int, List<String^>^>^ _hErrors;

		public:
			
			/***************************/
			/* Copied from lcaerrors.h */
			/***************************/
			literal int gcLCAErrorReferenceToNonExistingArea = 801;		// A location, waypoint of aisle refers to an area that is not part of the dataset
			literal int gcLCAErrorWarehousePreprocessCalledTwice = 202;	// The preprocessing function of the LCA Warehouse has been called too many times
			literal int gcLCAErrorReferenceToNonExistingAisle = 804;		// A location refers to an aisle that is not part of the dataset
			literal int gcLCAErrorBidirectionalPick = 205;					// An aisle with bidirectional pickdirection was added, this is not supported
			literal int gcLCAErrorBidirectionalMove = 206;					// An aisle with bidirectional moving direction was added, this is not supported

			// Pickbatch warnings
			literal int gcLCAErrorWarehouseMismatch = 800;					// The warehouse for the pickbatch and the LCA do not match
			literal int gcLCAErrorTooLargePickBatchLine = 301;				// The pickbatch line is too big to fit on a load carrier
			literal int gcLCAErrorTooLargePickBatch = 302;					// The pickbatch has too many orderlines to run the full algorithm
			literal int gcLCAErrorStrekAreaUnknown = 803;					// The pickbatch refers to an area for the strek that is not in the warehouse
			literal int gcLCAErrorLocationOutsideAisle = 104;				// The pickbatch has x,y coordinates outside the supplied aisle
			literal int gcLCAErrorNonPositiveWeight = 105;					// Pickbatch lines must have positive weight
			literal int gcLCAErrorNonPositiveVolume = 306;					// Pickbatch lines must have positive volume
			literal int gcLCAErrorNonPositiveQuantity = 307;				// Pickbatch lines must have positive quantity
			literal int gcLCAErrorNoArtGroup = 108;						// No assortmentgroup is filled in
			literal int gcLCAErrorNoCatGroup = 109;						// No categorygroup is filled in

			literal int gcLCAMaxAcceptableLCAWarning = 199;				// For any warning code above this the new LCA will not be able to run
			literal int gcLCAMaxAcceptableWarning = 499;					// For any warning code above this the LCA will not be able to calculate any distances
			literal int gcLCAMaxAcceptableError = 999;		

			LCAWrapper(LCASettings^ hLCASettings);

			~LCAWrapper();

			void AddArea(String^ sWSId, const double xCord, const double yCord);
			void AddAisle(String^ sWSId, String^ sAisle, const double frmXCord, const double frmYCord, const double toXCord, const double toYCord, String^ sDirectionPick);
			void AddAisleWayPoint(String^ sWSId, String^ sAisle, String^ sWayPointId, const double xCord, const double yCord);
			void AddPickBatchLine(String^ sPBRowId, const int pickSeq, String^ sArtId, String^ sCompanyId, 
							const double ordQty, const double volume, const double weight, String^ sWSId, String^ sAisle,
							String^ sArtGroup, String^ sCatGroup, const double xCord, const double yCord, String^ sWPAdr);

			LCAWrapperResult^ Process();
			
			String^ StringMarshallingTest(String^ str);
	};
};
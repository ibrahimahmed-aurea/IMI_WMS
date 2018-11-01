using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Imi.SupplyChain.Warehouse.Alarm.BusinessLogic;
using BusinessEntities = Imi.SupplyChain.Warehouse.Alarm.BusinessEntities;
using Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts;
using Imi.SupplyChain.Warehouse.Services.Alarm.ServiceImplementation.Translators;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace Imi.SupplyChain.Warehouse.Services.Alarm.ServiceImplementation
{
    public class AlarmServiceAdapter : MarshalByRefObject, IAlarmService
    {
                
        public FindAlarmTextResponse FindAlarmText(FindAlarmTextRequest request)
        {
            BusinessEntities.FindAlarmTextParams p = FindAlarmTextTranslator.TranslateFromServiceToBusiness(request.FindAlarmTextParams);

            FindAlarmTextAction action = PolicyInjection.Create<FindAlarmTextAction>();

            IList<BusinessEntities.FindAlarmTextResult> businessResult = action.Execute(p);
                        
            FindAlarmTextResponse response = new FindAlarmTextResponse();

            if (businessResult.Count > 0)
                response.FindAlarmTextResult = FindAlarmTextTranslator.TranslateFromBusinessToService(businessResult[0]);
            else
            {
                response.FindAlarmTextResult = new DataContracts.FindAlarmTextResult();
                response.FindAlarmTextResult.AlarmText = request.FindAlarmTextParams.AlarmId;
            }

            return response;
        }
                

    }
}

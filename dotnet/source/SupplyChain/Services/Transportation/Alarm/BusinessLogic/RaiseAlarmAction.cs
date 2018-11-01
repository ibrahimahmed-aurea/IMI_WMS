using System;
using System.Collections.Generic;
using System.Text;
using Imi.SupplyChain.Transportation.Alarm.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Transportation.Alarm.BusinessLogic
{
    public class RaiseAlarmAction : MarshalByRefObject
    {
        public static void Execute(string errorCode)
        {
            Execute(errorCode, null, null);
        }
                
        public static void Execute(string errorCode, int? currentPosition, string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                if (ApplicationContext.Current != null)
                    languageCode = ApplicationContext.Current.LanguageCode;
                else
                    languageCode = ApplicationContext.DefaultLanguageCode;
            }

            if (!String.IsNullOrEmpty(errorCode))
            {
                FindAlarmTextAction findAlarmTextAction = new FindAlarmTextAction();
                FindAlarmTextParams findAlarmTextParams = new FindAlarmTextParams();
                
                findAlarmTextParams.AlarmId = errorCode;
                findAlarmTextParams.LanguageCode = languageCode;

                IList<FindAlarmTextResult> alarmTextList = findAlarmTextAction.Execute(findAlarmTextParams);

                if (alarmTextList.Count != 1)
                {
                    AlarmException alarmException = new AlarmException(errorCode, "", currentPosition);
                    
                    throw alarmException;
                }
                else
                {
                    AlarmException alarmException = new AlarmException(errorCode, alarmTextList[0].AlarmText, currentPosition);
                    
                    throw alarmException;
                }
            }
        }
    }
}

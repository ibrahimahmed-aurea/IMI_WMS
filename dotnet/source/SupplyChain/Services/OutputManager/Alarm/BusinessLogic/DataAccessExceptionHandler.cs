using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

using System.Collections.Specialized;

using Imi.SupplyChain.OutputManager.Alarm.BusinessEntities;

using Imi.Framework.DataAccess;
using Imi.Framework.Services;

namespace Imi.SupplyChain.OutputManager.Alarm.BusinessLogic
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    class DataAccessExceptionHandler : IExceptionHandler
    {     

        public DataAccessExceptionHandler(NameValueCollection ignore)
        { 

        }
        Exception IExceptionHandler.HandleException(Exception exception, Guid handlingInstanceId)
        {
            DataAccessException dbExcep = exception as DataAccessException;
            if (dbExcep != null)
            {
                AlarmException alarmException;
                string errorCode = dbExcep.ErrorCode;
                string msg = dbExcep.Message;
                string addMsg = string.Empty;

                string languageCode;
                if (ApplicationContext.Current != null)
                    languageCode = ApplicationContext.Current.LanguageCode;
                else
                    languageCode = ApplicationContext.DefaultLanguageCode;

                FindAlarmTextAction findAlarmTextAction = new FindAlarmTextAction();
                FindAlarmTextParams findAlarmTextParams = new FindAlarmTextParams();

                findAlarmTextParams.AlarmId = errorCode;
                findAlarmTextParams.LanguageCode = languageCode;

                IList<FindAlarmTextResult> alarmTextList = findAlarmTextAction.Execute(findAlarmTextParams);

                if (alarmTextList.Count == 1)
                {
                    msg = alarmTextList[0].AlarmText;
                    if (exception.InnerException != null && exception.InnerException.Message.Length > 0)
                    {
                        addMsg = exception.InnerException.Message;
                    }
                    alarmException = new AlarmException(errorCode, msg, null, addMsg, null);

                    return alarmException;
                }

                return exception;
            }
            return exception;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.ServiceModel;

namespace Imi.Framework.Services.ExceptionHandling
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public ValidationExceptionHandler(NameValueCollection ignore)
        { 

        }

        #region IExceptionHandler Members

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            ValidationException ex = exception as ValidationException;

            if (ex != null)
            {
                ValidationFault fault = new ValidationFault();
                fault.ParameterName = ex.PropertyName;
                fault.Message = ex.Message;
                fault.Id = new Guid();
                
                if (ex is NullValidationException)
                    fault.ValidationError = ValidationError.NullValue;
                else
                    fault.ValidationError = ValidationError.InvalidValue;

                return new FaultException<ValidationFault>(fault, string.Format("Validation error. {0}", ex.Message));
            }

            return exception;
        }

        #endregion
    }
}

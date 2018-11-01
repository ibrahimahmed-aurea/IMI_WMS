using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Oracle.DataAccess.Client;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Imi.Framework.DataAccess;
using Imi.Framework.Services;

namespace Imi.Framework.Services.ExceptionHandling
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class CheckConstraintExceptionHandler : IExceptionHandler
    {
        public CheckConstraintExceptionHandler(NameValueCollection ignore)
        { 

        }

        #region IExceptionHandler Members

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            CheckConstraintException ex = exception as CheckConstraintException;

            if (ex != null)
            {
                if (ex.IsNullConstraint)
                    return new NullValidationException(ex.Message, ex.PropertyName, ex);
                else
                    return new ValidationException(ex.Message, ex.PropertyName, ex);
            }

            return exception;
        }

        #endregion
    }
}

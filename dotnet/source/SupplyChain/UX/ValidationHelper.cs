using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Imi.SupplyChain.UX
{
    public class ValidationHelper
    {
        private ValidationHelper()
        { 
        }

        public static ValidationResult ConvertToResult(ValidationException ex)
        {
            if (ex is NullValidationException)
                return new ValidationResult(ex.Message, null, ex.PropertyName, null, new NotNullValidator());
            else
                return new ValidationResult(ex.Message, null, ex.PropertyName, null, null);
        }
    }
}

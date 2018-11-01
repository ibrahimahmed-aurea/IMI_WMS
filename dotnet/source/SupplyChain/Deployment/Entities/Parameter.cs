using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Deployment.Entities
{
    [Serializable]
    public class Parameter
    {
        public string Name;
        public string Description;
        public bool IsMandatory;

        [OptionalField]
        public string Default;

        [OptionalField]
        public string ValidationRegEx;

        [OptionalField]
        public string ValidationErrorText;

        [OptionalField]
        public string ProductVersionIntroduced;

        [OptionalField]
        public string ProductVersionLastUsed;

        // Constructor
        public Parameter()
        {
            // Set all default values for all fields
            Default = string.Empty;
            ValidationRegEx = string.Empty;
            ProductVersionIntroduced = string.Empty;
            ProductVersionLastUsed = string.Empty;
        }

    }
}

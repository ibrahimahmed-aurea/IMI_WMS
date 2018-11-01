using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Imi.SupplyChain.UX.Rules
{
    public class ActionRuleContext<TActionParameters, TUserSessionType>
    {
        private TActionParameters _actionParameters;
        private TUserSessionType _session;

        public ActionRuleContext(TActionParameters actionParameters, TUserSessionType session)
        {
            _actionParameters = actionParameters;
            _session = session;
        }
                        
        public TActionParameters Parameters
        {
            get
            {
                return _actionParameters;
            }
        }

        public TUserSessionType Session
        {
            get
            {
                return _session;
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public bool IsItemSelected
        {
            get
            {
                Type type = Parameters.GetType();
                PropertyInfo info = type.GetProperty("IsItemSelected");

                if (info != null)
                    return (bool)info.GetValue(Parameters, null);
                else
                    return false;
            }
        }

        public bool IsMultipleItemsSelected
        {
            get
            {
                Type type = Parameters.GetType();
                PropertyInfo info = type.GetProperty("IsMultipleItemsSelected");

                if (info != null)
                    return (bool)info.GetValue(Parameters, null);
                else
                    return false;
            }
        }
    }
}

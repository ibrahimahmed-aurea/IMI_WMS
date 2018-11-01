using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Rules
{
    public class ViewRuleContext<TViewResult, TUserSessionType>
    {
        private TViewResult _viewResult;
        private TUserSessionType _session;

        public ViewRuleContext(TViewResult viewParameters, TUserSessionType session)
        {
            _viewResult = viewParameters;
            _isEnabled = true;
            _isVisible = true;
            _session = session;
        }


        public TUserSessionType Session
        {
            get
            {
                return _session;
            }
        }


        public TViewResult Parent
        {
            get
            {
                return _viewResult;
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
        
        private bool _isVisible;

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }
    }

}

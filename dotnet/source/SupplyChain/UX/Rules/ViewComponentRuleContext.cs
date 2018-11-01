using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Rules
{
    public class ViewComponentRuleContext<TViewResult, TValue, TUserSessionType>
        : ComponentRuleContext<TViewResult, TUserSessionType>
    {
        public ViewComponentRuleContext(TViewResult viewResult, TUserSessionType session, Func<string, MessageBoxButton, MessageBoxImage, MessageBoxResult> messageBoxDelegate)
            : base(viewResult, session, messageBoxDelegate)
        {
            _attentionLevel = UIAttentionLevel.Normal;
        }
                
        private UIAttentionLevel _attentionLevel;

        public UIAttentionLevel AttentionLevel
        {
            get
            {
                return _attentionLevel;
            }
            set
            {
                _attentionLevel = value;
            }
        }
                
        public bool IsValueSet { get; set; }

        TValue _value;

        public TValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                IsValueSet = true;
                _value = value;
            }
        }
    }
}

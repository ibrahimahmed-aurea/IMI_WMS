using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Rules
{
    public class ComponentRuleContext<TViewResult, TUserSessionType>
    {
        private TViewResult _viewResult;
        private TUserSessionType _session;
        private Func<string, MessageBoxButton, MessageBoxImage, MessageBoxResult> _messageBoxDelegate;

        public ComponentRuleContext(TViewResult viewResult, TUserSessionType session, Func<string, MessageBoxButton, MessageBoxImage, MessageBoxResult> messageBoxDelegate)
        {
            _viewResult = viewResult;
            _isEnabled = true;
            _visibility = UXVisibility.Visible;
            _session = session;
            _messageBoxDelegate = messageBoxDelegate;
        }
        
        public TUserSessionType Session
        {
            get
            {
                return _session;
            }
        }

        public MessageBoxResult ShowMessageBox(string message, MessageBoxButton button, MessageBoxImage image)
        {
            return _messageBoxDelegate(message, button, image);
        }
                
        public TViewResult View
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

        public bool IsVisibilitySet { get; set; }

        private  UXVisibility _visibility;

        public UXVisibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                IsVisibilitySet = true;
                _visibility = value;
            }
        }

        private bool _isMultipleItemsSelected;

        public bool IsMultipleItemsSelected
        {
            get
            {
                return _isMultipleItemsSelected;
            }
            set
            {
                _isMultipleItemsSelected = value;
            }
        }
       
        public bool IsInitializing { get; set; }
    }
}

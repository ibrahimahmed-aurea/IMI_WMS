using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class ShellAction : INotifyPropertyChanged, IAuthOperation
    {
        private bool isAuthorized;
        private bool isEnabled;
        private WorkItem workItem;
        private string caption;
        private string parameters;
        private bool isDetailAction;

        public ShellAction(WorkItem workItem)
        {
            this.workItem = workItem;
        }

        public WorkItem WorkItem
        {
            get
            {
                return workItem;
            }
        }
                
        public string Id { get; set; }

        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;

                OnPropertyChanged("Caption");
            }
        }

        public string Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;

                OnPropertyChanged("Parameters");
            }
        }

        public bool IsAuthorized
        {
            get
            {
                return isAuthorized;
            }
            set
            {
                isAuthorized = value;

                OnPropertyChanged("IsAuthorized");
            }
        }

        public bool IsEnabled 
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;

                OnPropertyChanged("IsEnabled");
            }
        }

        public bool IsDetailAction
        {
            get { return isDetailAction; }
            set { isDetailAction = value; }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;

            if (temp != null)
                temp(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Operation { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

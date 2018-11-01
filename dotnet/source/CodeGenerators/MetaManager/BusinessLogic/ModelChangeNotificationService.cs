using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ModelChangeNotificationService : IModelChangeNotificationService
    {
        private IConfigurationManagementService _confMgnService;

        private IConfigurationManagementService ConfigurationManagementService
        {
            get { return _confMgnService; }
            set
            {
                _confMgnService = value;
                _confMgnService.ObjectChanged += new DomainObjectChangedDelegate(ObjectChangedEventHandler);
                _confMgnService.ObjectAdded += new DomainObjectAddedDelegate(ObjectAddedEventHandler);
            }
        }

        private IModelService _modelService;

        private IModelService ModelService
        {
            get { return _modelService; }
            set
            {
                _modelService = value;
                _modelService.DomainObjectAdded += new DomainObjectAddedDelegate(DomainObjectAddedEventHandler);
                _modelService.DomainObjectDeleted += new DomainObjectDeletedDelegate(DomainObjectDeletedEventHandler);
                _modelService.DomainObjectChanged += new DomainObjectChangedDelegate(DomainObjectChangedEventHandler);
            }
        }
        
        public ModelChangeNotificationService()
        {
        }

        void DomainObjectChangedEventHandler(Guid Id, Type objectType)
        {
            RaiseDomainObjectChanged(Id, objectType, ObjectChangeTypes.Changed);
        }

        void DomainObjectDeletedEventHandler(Guid Id, Type objectType)
        {
            RaiseDomainObjectChanged(Id, objectType, ObjectChangeTypes.Deleted);
        }

        void DomainObjectAddedEventHandler(Guid Id, Type objectType)
        {
            RaiseDomainObjectChanged(Id, objectType, ObjectChangeTypes.Added);
        }

        void ObjectChangedEventHandler(Guid Id, Type objectType)
        {
            RaiseDomainObjectChanged(Id, objectType, ObjectChangeTypes.Added);
        }

        void ObjectAddedEventHandler(Guid Id, Type objectType)
        {
            RaiseDomainObjectChanged(Id, objectType, ObjectChangeTypes.Changed);
        }

        private void RaiseDomainObjectChanged(Guid Id, Type objectType, ObjectChangeTypes objectChangeType)
        {
            if (DomainObjectChange != null)
            {
                DomainObjectChange(Id, objectType, objectChangeType);
            }
        }

        #region IModelChangeNotificationService Members

        public event DomainObjectChangeDelegate DomainObjectChange;

        #endregion
    }
}

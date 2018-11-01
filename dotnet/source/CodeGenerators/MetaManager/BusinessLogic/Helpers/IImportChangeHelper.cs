using System;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class DeltaListEntry
    {
        public DeltaListEntry(object currentObject_newVersion, object currentObject_oldVersion, PropertyInfo parentProperty)
        {
            CurrentObject_NewVersion = currentObject_newVersion;
            CurrentObject_OldVersion = currentObject_oldVersion;
            Changes = new List<ObjectChangeDescription>();
            ChildObjects = new List<DeltaListEntry>();
            ParentPropery = parentProperty;
            PreviewMode = false;
        }

        public object CurrentObject_NewVersion;
        public object CurrentObject_OldVersion;
        public object CurrentObject_CurrentVersion
        {
            get
            {
                if (PreviewMode)
                {
                    return CurrentObject_CurrentVersionPreview;
                }
                else
                {
                    return CurrentObject_CurrentVersionSave;
                }
            }

            set
            {
                if (PreviewMode)
                {
                    CurrentObject_CurrentVersionPreview = value;
                }
                else
                {
                    CurrentObject_CurrentVersionSave = value;
                }
            }
        }
        public List<ObjectChangeDescription> Changes;
        public List<DeltaListEntry> ChildObjects;
        public PropertyInfo ParentPropery;

        public bool PreviewMode { get; private set; }

        private object CurrentObject_CurrentVersionPreview;
        private object CurrentObject_CurrentVersionSave;

        public void SwitchCurrentOjects()
        {
            PreviewMode = !PreviewMode;
            foreach (DeltaListEntry child in ChildObjects)
            {
                child.SwitchCurrentOjects();
            }
        }

        public Guid OwnerId;

        public bool HasConflicts
        {
            get
            {
                if (Changes.Where(c => c.conflict).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public enum ChangeTypes
    {
        New,
        Deleted,
        Changed,
        Moved
    }

    public class ObjectChangeDescription
    {
        public ObjectChangeDescription(PropertyInfo changedProperty, ChangeTypes changeType, object newValue, object oldValue, DataAccess.Domain.VisualModel.UXComponent movedComponent = null, string newComponentOrder = "")
        {
            ChangedProperty = changedProperty;
            ChangeType = changeType;
            NewValue = newValue;
            OldValue = oldValue;
            MovedComponent = movedComponent;
            NewComponentOrder = newComponentOrder;
        }

        public PropertyInfo ChangedProperty;
        public ChangeTypes ChangeType;
        public object NewValue;
        public object OldValue;
        public DataAccess.Domain.VisualModel.UXComponent MovedComponent;
        public string NewComponentOrder;

        public bool conflict = false;
        public bool possibleDoubleImplementation = false;
        public object CurrentValue = null;
       
    }

    public interface IImportChangeHelper
    {
        List<DeltaListEntry> GetChange(string issueId, string repositoryPathForImport, out string additionalInfo);
        List<string> GetCurrentVersionOfObjects(List<DeltaListEntry> changeDelta, DataAccess.Domain.Application frontendApplication, DataAccess.Domain.Application backendApplication);
        void GetConflicts(List<DeltaListEntry> changeDelta);
        List<DeltaListEntry> GetSortedDeltaList(List<DeltaListEntry> originalDeltaList, out Dictionary<DeltaListEntry, Dictionary<Guid, Type>> changedObjectsWithMissingReferences);
        void ApplyChange(List<DeltaListEntry> sortedChangeDelta, bool save);
    }
}

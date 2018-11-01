using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{

    public class DataModelChange
    {

        public List<DetectedChange> ListedDetectedChanges { get; set; }

        public bool Apply { get; set; }

        public DataModelChange(DataModelChangeType type) : this(type, string.Empty) { }

        public DataModelChange(DataModelChangeType type, string change)
        {
            AddChange(type, change);
            Apply = true;
        }

        public void AddChange(DataModelChangeType type, string change)
        {
            GetDetectedChangeByType(type).AddText(change);
        }

        public bool ContainDataModelChangeType(DataModelChangeType type)
        {
            bool toReturn = false;
            if (ListedDetectedChanges != null)
            {
                foreach (DetectedChange dc in ListedDetectedChanges)
                {
                    if (dc.ChangeType == type)
                    {
                        toReturn = true;
                    }
                }
            }
            return toReturn;
        }

        private DetectedChange GetDetectedChangeByType(DataModelChangeType type)
        {
            DetectedChange detectedChangeToReturn = null;
            if (ListedDetectedChanges == null)
            {
                ListedDetectedChanges = new List<DetectedChange>();
                detectedChangeToReturn = new DetectedChange(type);
                ListedDetectedChanges.Add(detectedChangeToReturn);
            }
            else
            {
                foreach (DetectedChange dc in ListedDetectedChanges)
                {
                    if (dc.ChangeType == type)
                    {
                        detectedChangeToReturn = dc;
                    }
                }
                if (detectedChangeToReturn == null)
                {
                    detectedChangeToReturn = new DetectedChange(type);
                    ListedDetectedChanges.Add(detectedChangeToReturn);
                }
            }

            return detectedChangeToReturn;

        }

    }
}

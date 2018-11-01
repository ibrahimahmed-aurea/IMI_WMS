using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public enum DataModelChangeType { New, Modified, Deleted, NewHint, ModifiedHint/*, RemoveHint*/, DeletedHint, UnresolvedHint };

    public class DetectedChange
    {
        public DataModelChangeType ChangeType { get; set; }
        public List<string> Changes { get; set; }

        public DetectedChange() { ChangeType = DataModelChangeType.New; }

        public DetectedChange(DataModelChangeType type) : this(type, string.Empty) { ;}

        public DetectedChange(DataModelChangeType type, string txt)
        {
            ChangeType = type;
            if (txt != string.Empty)
            {
                Changes.Add(txt);
            }
        }

        public void AddText(string txt)
        {
            if (txt.Trim() != string.Empty)
            {
                if (Changes == null)
                    Changes = new List<string>();

                Changes.Add(txt.Trim());
            }
        }

        public override string ToString()
        {
            string strToReturn = string.Empty;

            if (Changes != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (String str in Changes)
                {
                    sb.AppendLine(str);
                }
                strToReturn = sb.ToString().Trim();
            }
            return strToReturn;
        }

    }
}

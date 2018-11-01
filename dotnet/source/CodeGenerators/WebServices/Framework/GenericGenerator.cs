using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace Imi.CodeGenerators.WebServices.Framework
{
    public class ProgressEventArgs : EventArgs
    {
        private double percent = 0;
        private int curr = 0;
        private int total = 0;

        public ProgressEventArgs(int curr, int total)
        {
            this.total = total;
            Update(curr);
        }

        public void Update(int curr)
        {
            this.curr = curr;
            double dcurr = Convert.ToDouble(curr);
            double dtotal = Convert.ToDouble(total);
            percent = Math.Ceiling(((dcurr / dtotal) * 100));
        }

        public String Percent()
        {
            return (percent.ToString());
        }
    }

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    public class DataType
    {
        public const int StringType = 0;
        public const int DoubleType = 1;
        public const int IntegerType = 2;
        public const int DateTimeType = 3;
        public const int UnknownType = 4;

        public int dataType;
        public int length;
        public int precision;
        public int scale;
    }

    public abstract class GenericGenerator
    {
        protected MessageDefinition messageDef;
        protected Package packageDef;
        public event ProgressEventHandler Progress;

        public GenericGenerator(MessageDefinition messageDef, Package packageDef)
        {
            this.messageDef = messageDef;
            this.packageDef = packageDef;
        }

        protected virtual void OnProgress(ProgressEventArgs e)
        {
            if (Progress != null)
            {
                //Invokes the delegates.
                Progress(this, e);
            }
        }

        protected DataType GetDataType(ParameterType p)
        {
            string datatype;
            DataType dt = new DataType();
            bool column_datatype_known = false;

            if (p.column != null)
            {
                column_datatype_known = (p.column.dataType != null);
            }

            if (column_datatype_known)
            {
                datatype = p.column.dataType;
            }
            else
            {
                datatype = p.dataType;
            }

            switch (datatype)
            {
                case "VARCHAR":
                case "VARCHAR2":
                    dt.dataType = DataType.StringType;
                    break;
                case "NUMBER":
                    if (!column_datatype_known)
                        dt.dataType = DataType.DoubleType; // we cannot tell if it is integer or not
                    else if ((p.column.scale == "0") &&
                      (Convert.ToDecimal(p.column.precision) <= 9)) // -2,147,483,648 to 2,147,483,647
                        dt.dataType = DataType.IntegerType;
                    else
                        dt.dataType = DataType.DoubleType;
                    break;
                case "DATE":
                    dt.dataType = DataType.DateTimeType;
                    break;
                default:
                    dt.dataType = DataType.UnknownType;
                    break;
            }

            // detailsunknown, then invent something...
            if (!column_datatype_known)
            {
                switch (dt.dataType)
                {
                    case DataType.StringType:
                        dt.length = 255;
                        dt.precision = 0;
                        dt.scale = 0;
                        break;
                    case DataType.DoubleType:
                        dt.length = 25;
                        dt.precision = 15;
                        dt.scale = 9;
                        break;
                    case DataType.IntegerType:
                        dt.length = 9;
                        dt.precision = 9;
                        dt.scale = 0;
                        break;
                    case DataType.DateTimeType:
                        dt.length = 25;
                        dt.precision = 15;
                        dt.scale = 10;
                        break;
                }
            }
            else
            {
                if (p.column.length != "")
                    dt.length = Convert.ToInt32(p.column.length);
                else
                    dt.length = 0;

                if (p.column.precision != "")
                    dt.precision = Convert.ToInt32(p.column.precision);
                else
                    dt.precision = 0;

                if (p.column.scale != "")
                    dt.scale = Convert.ToInt32(p.column.scale);
                else
                    dt.scale = 0;
            }
            return dt;
        }

        public string GetDBDeclaration(DataType dt)
        {
            String decl = "";

            switch (dt.dataType)
            {
                case DataType.StringType:
                    decl = "VARCHAR2(" + Convert.ToString(dt.length) + ")";
                    break;
                case DataType.DoubleType:
                    if (dt.precision > 0)
                    {
                        if (dt.scale > 0)
                            decl = "NUMBER(" + Convert.ToString(dt.precision) + "," + Convert.ToString(dt.scale) + ")";
                        else
                            decl = "NUMBER(" + Convert.ToString(dt.precision) + ")";
                    }
                    else
                        decl = "NUMBER";
                    break;
                case DataType.IntegerType:
                    if (dt.precision > 0)
                        decl = "NUMBER(" + Convert.ToString(dt.precision) + ")";
                    else
                        decl = "NUMBER(9)";
                    break;
                case DataType.DateTimeType:
                    decl = "DATE";
                    break;
                default:
                    decl = "Syntax Error";
                    break;
            }

            return (decl);
        }

        public string GetDBDeclaration(ParameterType p)
        {
            String decl = "";

            if (p.column != null)
                decl = p.column.declaration;

            if ((decl == null) || (decl == ""))
            {
                DataType dt = GetDataType(p);
                decl = GetDBDeclaration(dt);
            }

            return (decl);
        }

        // we do not always want to check for client identity, allow derived class to always respond false
        protected virtual bool IsClientIdentity(ParameterType p)
        {
            return (p.name.ToUpper() == ("ClientIdentity").ToUpper());
        }

        private bool IsCommPartnerIdentity(ParameterType p)
        {
            return (p.name.ToUpper() == ("CommPartnerIdentity").ToUpper());
        }

        public bool IsExternalField(ParameterType p)
        {
            return (((p.fieldType == ParameterTypeFieldType.OpCode) ||
              (p.fieldType == ParameterTypeFieldType.Normal)) &&
              !IsClientIdentity(p) && !IsCommPartnerIdentity(p));
        }

        protected string GetExternalName(ParameterType p, StructureType structure)
        {
            if (structure.modifications != null)
                foreach (ModificationType mt in structure.modifications)
                    if (p.name.ToUpper() == mt.name.ToUpper())
                        if ((mt.overrideName).Length > 0)
                            return mt.overrideName;

            return p.name;
        }

        protected ParameterType[] GetParameters(String name)
        {
            String[] procedureName = name.Split('.');

            foreach (ProcedureType p in packageDef.procedures)
            {
                if (p.name.Equals(procedureName[procedureName.GetUpperBound(0)]))
                    return (p.parameters);
            }

            return (null);
        }

        protected bool IsExcluded(String name, StructureType structure)
        {
            if (structure.modifications != null)
                foreach (ModificationType p in structure.modifications)
                    if (name.ToUpper() == p.name.ToUpper())
                        return (p.exclude);
            return false;
        }

        protected ParameterType[] GetUniqueParameterList(StructureType structure)
        {
            bool IncludeHapiFields = true;
            bool IncludeOpCode = true;
            bool IncludeExcludedFields = false;
            bool IncludeSystemAdmin = true;

            StringCollection plUniq = new StringCollection();
            ArrayList holder = new ArrayList();
            ParameterType[] r = null;

            if (IncludeHapiFields)
            {
                if (messageDef.globalParameters != null)
                {
                    // look for SystemId fields
                    foreach (ParameterType p in messageDef.globalParameters)
                    {
                        if (p.fieldType == ParameterTypeFieldType.SystemId)
                        {
                            plUniq.Add(p.name);
                            holder.Add(p);
                        }
                    }
                }
            }

            if (IncludeOpCode)
            {
                if (messageDef.globalParameters != null)
                {
                    // look for OpCode fields
                    foreach (ParameterType p in messageDef.globalParameters)
                    {
                        if (p.fieldType == ParameterTypeFieldType.OpCode)
                        {
                            plUniq.Add(p.name);
                            holder.Add(p);
                        }
                    }
                }
            }

            if (structure.insertSP != "")
            {
                ParameterType[] pl = GetParameters(structure.insertSP);

                foreach (ParameterType p in pl)
                    if (!plUniq.Contains(p.name))
                    {
                        if (IsExcluded(p.name, structure) && (!IncludeExcludedFields))
                            continue;

                        plUniq.Add(p.name);
                        p.use = new UseType();
                        p.use.insert = true;
                        holder.Add(p);
                    }
                    else
                    {
                        foreach (ParameterType pt in holder)
                        {
                            if (pt.name == p.name)
                            {
                                pt.use.insert = true;
                            }
                        }
                    }
            }

            if (structure.updateSP != "")
            {
                ParameterType[] pl = GetParameters(structure.updateSP);

                foreach (ParameterType p in pl)
                    if (!plUniq.Contains(p.name))
                    {
                        if (IsExcluded(p.name, structure) && (!IncludeExcludedFields))
                            continue;

                        plUniq.Add(p.name);
                        p.use = new UseType();
                        p.use.update = true;
                        holder.Add(p);
                    }
                    else
                    {
                        foreach (ParameterType pt in holder)
                        {
                            if (pt.name == p.name)
                            {
                                pt.use.update = true;
                            }
                        }
                    }
            }

            if (structure.deleteSP != "")
            {
                ParameterType[] pl = GetParameters(structure.deleteSP);

                foreach (ParameterType p in pl)
                    if (!plUniq.Contains(p.name))
                    {
                        if (IsExcluded(p.name, structure) && (!IncludeExcludedFields))
                            continue;

                        plUniq.Add(p.name);
                        p.use = new UseType();
                        p.use.delete = true;
                        holder.Add(p);
                    }
                    else
                    {
                        foreach (ParameterType pt in holder)
                        {
                            if (pt.name == p.name)
                            {
                                pt.use.delete = true;
                            }
                        }
                    }
            }

            if ((structure.baseTable != null) && (structure.baseTable != ""))
            {
                ParameterType[] pl = GetParameters(structure.baseTable);

                foreach (ParameterType p in pl)
                    if (!plUniq.Contains(p.name))
                    {
                        if (IsExcluded(p.name, structure) && (!IncludeExcludedFields))
                        {
                            if (!p.column.primaryKey)
                                continue;
                        }

                        plUniq.Add(p.name);
                        p.use = new UseType();
                        p.use.insert = true;
                        holder.Add(p);
                    }
                    else
                    {
                        foreach (ParameterType pt in holder)
                        {
                            if (pt.name == p.name)
                            {
                                pt.use.insert = true;
                            }
                        }
                    }
            }

            if (IncludeSystemAdmin)
            {
                if (messageDef.globalParameters != null)
                {
                    // look for SystemAdmin fields
                    foreach (ParameterType p in messageDef.globalParameters)
                    {
                        if (p.fieldType == ParameterTypeFieldType.SystemAdmin)
                        {
                            plUniq.Add(p.name);
                            holder.Add(p);
                        }
                    }
                }
            }

            r = holder.ToArray(typeof(ParameterType)) as ParameterType[];

            return (r);
        }

        protected String GetTableName(String shortName)
        {
            return shortName;
        }

        protected StructureType GetStructureFromName(string StructureName)
        {
            foreach (StructureType st in messageDef.structures)
            {
                if (st.name == StructureName)
                    return st;
            }
            return null;
        }

        // belongs to GetUniqueStructureList
        StringCollection slUniq = null;
        ArrayList holder = null;

        void AddStructure(string StructureName)
        {
            if (StructureName != "")
            {
                StructureType st = GetStructureFromName(StructureName);
                if (st != null)
                {
                    if (!slUniq.Contains(StructureName))
                    {
                        slUniq.Add(StructureName);
                        holder.Add(st);
                    }

                    // recurse all children
                    if (st.children != null)
                        foreach (ChildType ct in st.children)
                            AddStructure(ct.name);
                }
            }
        }

        protected StructureType[] GetUniqueStructureList(InterfaceType i)
        {
            StructureType[] r = null;

            slUniq = new StringCollection();
            holder = new ArrayList();

            AddStructure(i.structure);

            r = holder.ToArray(typeof(StructureType)) as StructureType[];

            return (r);
        }

        protected String GetExternalTypeName(String structure)
        {
            return (structure + "Doc");
        }

        protected string GetExternalDataType(ParameterType p)
        {
            DataType dt = GetDataType(p);
            switch (dt.dataType)
            {
                case DataType.StringType:
                    return "string";
                case DataType.DoubleType:
                    return "Nullable<double>";
                case DataType.IntegerType:
                    return "Nullable<int>";
                case DataType.DateTimeType:
                    return "Nullable<DateTime>";
                default:
                    return "Syntax Error";
            }

        }

        protected String CapitalizeWords(String OriginalString, String WordsToUse)
        {
            if (OriginalString == null)
                return null;

            String s = OriginalString.ToUpper();

            if (WordsToUse != null)
            {
                String[] words = WordsToUse.Split((" .,").ToCharArray());
                ArrayList CapitalizedWords = new ArrayList();

                foreach (String word in words)
                {
                    String sw = "";
                    bool First = true;
                    foreach (char ch in word)
                    {
                        if (First)
                        {
                            First = false;
                            sw += char.ToUpper(ch);
                        }
                        else
                            sw += char.ToLower(ch);
                    }
                    if (!First)
                        CapitalizedWords.Add(sw);
                }

                CapitalizedWords.Sort(new CompareStringLength());

                ArrayList ReplacedWords = new ArrayList();

                foreach (String word in CapitalizedWords)
                {
                    bool DontReplace = false;
                    foreach (String ReplacedWord in ReplacedWords)
                        if (ReplacedWord.IndexOf(word) >= 0)
                        {
                            DontReplace = true;
                            break;
                        }

                    if (DontReplace)
                        break;
                    s = s.Replace(word.ToUpper(), word);
                    ReplacedWords.Add(word.ToUpper());
                }
            }

            char[] charr = s.ToCharArray();
            char[] charr_build = s.ToCharArray();

            for (int i = 1; i < charr.Length; i++)
            {
                if (char.IsUpper(charr[i]))
                {
                    if (i < (charr.Length - 1))
                        if (char.IsLower(charr[i + 1]))
                            continue;

                    if (char.IsLower(charr[i - 1]))
                        continue;

                    charr_build[i] = char.ToLower(charr[i]);
                }
            }

            s = "";
            foreach (char ch in charr_build)
                s += ch;

            return s;
        }

        protected string[] GetComment(StructureType st, ParameterType p)
        {
            if (st.modifications != null)
            {
                foreach (ModificationType mt in st.modifications)
                {
                    if (mt.name == p.name)
                    {
                        if (mt.comment != null)
                            return mt.comment;
                        break;
                    }
                }
            }

            if (p.column != null)
                return p.column.comment;

            return null;
        }

        public abstract void GetOutPut(TextWriter tw);

        protected bool GetMandatory(ParameterType p)
        {
            if (p != null)
                if (p.column != null)
                    if (p.column.mandatory)
                        return true;

            return false;
        }

    }

    class CompareStringLength : IComparer
    {
        public int Compare(object x, object y)
        {
            return (((String)y).Length - ((String)x).Length);
        }
    }

}

using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Security.Principal;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Dictionary
{
    public class DictionaryGenerator : GenericGenerator
    {
        private const string NewLine = "\r\n";
        DictionaryDefinition indict;

        public DictionaryGenerator(MessageDefinition messageDef, Package packageDef, DictionaryDefinition dict)
            :
        base(messageDef, packageDef)
        {
            indict = dict;
        }

        private ParameterType[] GetCompleteParameterList()
        {
            StringCollection plUniq = new StringCollection();
            ArrayList holder = new ArrayList();
            ParameterType[] r = null;

            if (messageDef.globalParameters != null)
            {
                // look for SystemId fields
                foreach (ParameterType p in messageDef.globalParameters)
                {
                    plUniq.Add(p.name);
                    holder.Add(p);
                }
            }

            foreach (StructureType structure in messageDef.structures)
            {
                if (structure.insertSP != "")
                {
                    ParameterType[] pl = GetParameters(structure.insertSP);

                    if (pl != null)
                        foreach (ParameterType p in pl)
                            if (!plUniq.Contains(p.name))
                            {
                                plUniq.Add(p.name);
                                p.use = new UseType();
                                p.use.insert = true;
                                holder.Add(p);
                            }
                }

                if (structure.updateSP != "")
                {
                    ParameterType[] pl = GetParameters(structure.updateSP);

                    if (pl != null)
                        foreach (ParameterType p in pl)
                            if (!plUniq.Contains(p.name))
                            {
                                plUniq.Add(p.name);
                                p.use = new UseType();
                                p.use.update = true;
                                holder.Add(p);
                            }
                }

                if (structure.deleteSP != "")
                {
                    ParameterType[] pl = GetParameters(structure.deleteSP);

                    if (pl != null)
                        foreach (ParameterType p in pl)
                            if (!plUniq.Contains(p.name))
                            {
                                plUniq.Add(p.name);
                                p.use = new UseType();
                                p.use.delete = true;
                                holder.Add(p);
                            }
                }

                if ((structure.baseTable != null) && (structure.baseTable != ""))
                {
                    ParameterType[] pl = GetParameters(structure.baseTable);

                    if (pl != null)
                        foreach (ParameterType p in pl)
                            if (!plUniq.Contains(p.name))
                            {
                                plUniq.Add(p.name);
                                p.use = new UseType();
                                p.use.insert = true;
                                holder.Add(p);
                            }
                }
            }

            r = holder.ToArray(typeof(ParameterType)) as ParameterType[];

            return (r);
        }

        private InterfaceFieldType[] GetInterfaceFieldTypeList()
        {
            ArrayList holder = new ArrayList();
            ParameterType[] ptlist = GetCompleteParameterList();

            if (indict != null)
                if (indict.fields != null)
                    foreach (InterfaceFieldType incpt in indict.fields)
                        holder.Add(incpt);

            foreach (ParameterType pt in ptlist)
            {
                bool found = false;
                foreach (InterfaceFieldType cpt in holder)
                {
                    if (pt.name.ToUpper() == cpt.name.ToUpper())
                        found = true;
                }
                if (!found)
                {
                    InterfaceFieldType c = new InterfaceFieldType();
                    c.name = pt.name;
                    holder.Add(c);
                }
            }
            return (holder.ToArray(typeof(InterfaceFieldType)) as InterfaceFieldType[]);
        }

        private InterfaceFieldType[] SortInterfaceFieldTypeList(InterfaceFieldType[] ift)
        {
            ArrayList holder = new ArrayList();

            if (ift != null)
                foreach (InterfaceFieldType incpt in ift)
                    holder.Add(incpt);

            holder.Sort(new CompareInterfaceFieldType());

            return (holder.ToArray(typeof(InterfaceFieldType)) as InterfaceFieldType[]);
        }

        public override void GetOutPut(TextWriter tw)
        {
            DictionaryDefinition dd = new DictionaryDefinition();
            dd.fields = GetInterfaceFieldTypeList();
            dd.fields = SortInterfaceFieldTypeList(dd.fields);
            XmlSerializer serializer = new XmlSerializer(typeof(DictionaryDefinition));
            serializer.Serialize(tw, dd);
        }
    }

    class CompareInterfaceFieldType : IComparer
    {
        public int Compare(object x, object y)
        {
            return String.Compare( ((InterfaceFieldType)x).name,((InterfaceFieldType)y).name );
        }
    }
}

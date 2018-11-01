using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.Spec2XML
{

    class XMLGenerator : GenericGenerator
    {
        private DictionaryDefinition dictDef;

        public XMLGenerator(MessageDefinition messageDef, Package packageDef, DictionaryDefinition dictDef)
            :
        base(messageDef, packageDef)
        {
            this.dictDef = dictDef;
        }

        public void GenerateXML(PLSQLSpecPackage ss, bool dict_is_optional)
        {
            int j;
            bool bIsFunction;

            if (ss == null)
                return;

            packageDef = new Package();

            packageDef.name = ss.PackageName;

            if (ss.Procedures != null)
            {
                {
                    string missingParameters = "";
                    // identify missing parameters from dictionary
                    foreach (PLSQLSpecProc sp in ss.Procedures)
                        foreach (PLSQLSpecParam p in sp.Params)
                            if (InDictionary(p.ParamName) == null)
                                missingParameters += "    <field name=\"" + p.ParamName + "\" />\r\n";

                    if (!string.IsNullOrEmpty(missingParameters))
                        Console.WriteLine("Warning parameters missing in dictionary\r\n" + missingParameters);
                }

                packageDef.procedures = new ProcedureType[ss.Procedures.Count];

                int i = 0;
                foreach (PLSQLSpecProc sp in ss.Procedures)
                {
                    bIsFunction = (sp.UnconnectedReturnType != "");

                    packageDef.procedures[i] = new ProcedureType();

                    packageDef.procedures[i].name = sp.ProcName;
                    packageDef.procedures[i].overloaded = CheckForOverload(ss, sp.ProcName);

                    if (bIsFunction)
                        packageDef.procedures[i].type = ProcedureTypeType.Function;
                    else
                        packageDef.procedures[i].type = ProcedureTypeType.Procedure;

                    int ParamCount = sp.Params.Count;
                    if (bIsFunction)
                        ParamCount++; // return value is handled as an extra parameter

                    packageDef.procedures[i].parameters = new ParameterType[ParamCount];

                    j = 0;
                    foreach (PLSQLSpecParam p in sp.Params)
                    {
                        packageDef.procedures[i].parameters[j] = new ParameterType();

                        String newName = InDictionary(p.ParamName);
                        if (newName != null)
                            packageDef.procedures[i].parameters[j].name = newName;
                        else
                        {
                            if (dict_is_optional)
                                packageDef.procedures[i].parameters[j].name = p.ParamName;
                            else
                            {
                                Exception e = new Exception("Parameter not found in dictionary: " + p.ParamName);
                                throw (e);
                            }
                        }

                        packageDef.procedures[i].parameters[j].originTable = p.TableName;
                        packageDef.procedures[i].parameters[j].originColumn = p.ColumnName;

                        if (p.DirIn && !p.DirOut)
                            packageDef.procedures[i].parameters[j].direction = ParameterTypeDirection.@In;
                        else if (!p.DirIn && p.DirOut)
                            packageDef.procedures[i].parameters[j].direction = ParameterTypeDirection.@Out;
                        else if (p.DirIn && p.DirOut)
                            packageDef.procedures[i].parameters[j].direction = ParameterTypeDirection.InOut;
                        else if (!p.DirIn && !p.DirOut)
                            packageDef.procedures[i].parameters[j].direction = ParameterTypeDirection.@In;

                        packageDef.procedures[i].parameters[j].dataType = p.UnconnectedType;
                        packageDef.procedures[i].parameters[j].fieldType = ParameterTypeFieldType.Normal;

                        if (p.Mandatory)
                        {
                            packageDef.procedures[i].parameters[j].column = new ColumnType();
                            packageDef.procedures[i].parameters[j].column.mandatory = true;
                        }

                        j++;
                    }

                    if (bIsFunction)
                    {
                        // the extra parameter
                        packageDef.procedures[i].parameters[j] = new ParameterType();

                        packageDef.procedures[i].parameters[j].name = "";
                        packageDef.procedures[i].parameters[j].originTable = "";
                        packageDef.procedures[i].parameters[j].originColumn = "";

                        packageDef.procedures[i].parameters[j].direction = ParameterTypeDirection.Result;
                        packageDef.procedures[i].parameters[j].dataType = sp.UnconnectedReturnType;
                        packageDef.procedures[i].parameters[j].fieldType = ParameterTypeFieldType.Normal;
                    }

                    i++;
                }
            }
        }

        public override void GetOutPut(TextWriter tw)
        {
            if (packageDef == null)
                return;

            XmlSerializer serializer = new XmlSerializer(typeof(Package));
            serializer.Serialize(tw, packageDef);
        }

        private bool CheckForOverload(PLSQLSpecPackage ss, string ProcName)
        {
            int count = 0;
            foreach (PLSQLSpecProc sp in ss.Procedures)
            {
                if (sp.ProcName.ToUpper() == ProcName.ToUpper())
                    count++;
            }
            return (count > 1);
        }

        private String InDictionary(String name)
        {
            try
            {
                if (dictDef != null)
                    if (dictDef.fields != null)
                        foreach (InterfaceFieldType ift in dictDef.fields)
                            if (ift.name.ToUpper() == name.ToUpper())
                                return ift.name;
            }
            catch
            {
            }

            return null;
        }

    }

}

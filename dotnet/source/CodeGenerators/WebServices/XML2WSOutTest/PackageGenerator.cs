using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Shared;
using Imi.CodeGenerators.WebServices.ColumnInfo2XML;

namespace Imi.CodeGenerators.WebServices.XML2WSOutTest
{


    /// <summary>
    /// Summary description for PackageGenerator.
    /// </summary>
    public class PackageGenerator : GenericGenerator
    {
        // Caching for Primary Key lookup
        private StringCollection pkList = new StringCollection();
        private String currTable = "";
        private DictionaryDefinition dictDef;
        private Database db = null;

        public PackageGenerator(MessageDefinition messageDef, Package packageDef, DictionaryDefinition dictDef, Database db)
            :
        base(messageDef, packageDef)
        {
            this.db = db;
            this.dictDef = dictDef;
        }

        private bool IsPrimaryKey(String tableName, String columnName)
        {
            IDataReader r = null;

            try
            {
                // Don't do anything if no table or column is given
                if ((tableName == "") || (columnName == ""))
                    return (false);

                if (currTable != tableName)
                {
                    currTable = tableName;
                    pkList.Clear();

                    r = db.ExecuteReader(
                      "select u2.column_name        " +
                      "from user_constraints u1,    " +
                      "     user_cons_columns u2    " +
                      "where u1.table_name = '" + tableName + "'  " +
                      "and constraint_type = 'P'    " +
                      "and u1.owner = u2.owner      " +
                      "and u1.constraint_name = u2.constraint_name " +
                      "and u1.table_name      = u2.table_name      " +
                      "order by position");

                    while (r.Read())
                    {
                        String ss = r.GetString(0);
                        pkList.Add(ss);
                    }
                }
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return (pkList.Contains(columnName));


        }

        public ProcedureType GetProcedure(String baseTableName)
        {
            ArrayList parameterList = new ArrayList();
            IDataReader r = null;
            ProcedureType aProcedure = null;
            ColumnInfoGenerator cig = new ColumnInfoGenerator(null, null, db);
            String tableName = "";

            if ((baseTableName != null) && (baseTableName != ""))
            {
                string[] a = baseTableName.Split('.');
                if (a.Length > 1)
                    tableName = a[1];
                else
                    tableName = a[0];
            }

            try
            {
                // Don't do anything if no table or column is given
                if (tableName == "")
                    return (null);

                r = db.ExecuteReader(
                  "select U2.COLUMN_NAME " +
                  "  from USER_TAB_COLUMNS  U2 " +
                  " where U2.TABLE_NAME  = '" + tableName + "'" +
                  " order by U2.COLUMN_ID");

                while (r.Read())
                {
                    ParameterType param = new ParameterType();
                    param.name = r.GetString(0);

                    switch (param.name)
                    {
                        case "HAPITRANS_ID":
                            break;
                        case "MSG_OUT_ID":
                            break;
                        case "UPDDTM":
                            break;
                        case "PROID":
                            break;
                        default:
                            param.originTable = tableName;
                            param.originColumn = param.name;
                            param.direction = ParameterTypeDirection.In;
                            param.fieldType = ParameterTypeFieldType.Normal;
                            cig.AddColumn(param);

                            String newName = InDictionary(param.name);
                            if (newName != null)
                                param.name = newName;
                            else
                            {
                                if (param.column != null)
                                {
                                    if ((param.column.comment != null) && (param.column.comment.Length > 0))
                                        param.name = CapitalizeWords(param.name, param.column.comment[0]);
                                }
                            }

                            if (param.column != null)
                            {
                                param.column.primaryKey = IsPrimaryKey(param.originTable, param.originColumn);
                            }
                            parameterList.Add(param);
                            break;
                    }
                }

                aProcedure = new ProcedureType();
                aProcedure.name = tableName;
                aProcedure.overloaded = false;
                aProcedure.type = ProcedureTypeType.Procedure;
                aProcedure.parameters = parameterList.ToArray(typeof(ParameterType)) as ParameterType[];

            }
            catch (Exception e)
            {
                Console.WriteLine("Error while processing " + tableName + Environment.NewLine +
                  e.Message + Environment.NewLine +
                  e.StackTrace);
                throw e;
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return (aProcedure);
        }

        public override void GetOutPut(TextWriter tw)
        {
            StringBuilder s = new StringBuilder();

            int curr = 0;
            int total = messageDef.structures.Length + 1;
            ProgressEventArgs progressArgs;
            progressArgs = new ProgressEventArgs(curr, total);
            OnProgress(progressArgs);

            ProcedureType[] procedureList = new ProcedureType[messageDef.structures.Length];
            int i = 0;

            foreach (StructureType structure in messageDef.structures)
            {
                ProcedureType aProcedure = GetProcedure(structure.baseTable);
                procedureList[i++] = aProcedure;
                progressArgs.Update(++curr);
                OnProgress(progressArgs);
            }

            Package p = new Package();
            p.procedures = procedureList;

            XmlSerializer serializer = new XmlSerializer(typeof(Package));
            serializer.Serialize(tw, p);

            progressArgs.Update(total);
            OnProgress(progressArgs);
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


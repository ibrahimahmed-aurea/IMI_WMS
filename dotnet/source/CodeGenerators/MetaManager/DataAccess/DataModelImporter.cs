using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using Cdc.MetaManager.DataAccess.Domain;
using System.Text.RegularExpressions;
using System.Data;

namespace Cdc.MetaManager.DataAccess
{
    public class DataModelTableInfo
    {
        public string TableName { get; set; }
        public DateTime TableCreated { get; set; }
        public string TableComment { get; set; }

        public DataModelTableInfo(string tableName, DateTime tableCreated, string tableComment)
        {
            TableName = tableName;
            TableComment = tableComment;
            TableCreated = tableCreated;
        }
    }

    public class DataModelImporter
    {
        private DataModelImporter()
        {
        }

        public static IList<DataModelTableInfo> GetAllTablesInSchema(Schema schema)
        {
            List<DataModelTableInfo> tableList = new List<DataModelTableInfo>();

            using (OracleConnection connection = new OracleConnection(schema.ConnectionString))
            {
                connection.Open();

                string sql = "select  X1.OBJECT_NAME TABLE_NAME," +
                             "        X1.CREATED TABLE_CREATED," +
                             "        X2.COMMENTS TABLE_COMMENT" +
                             "  from  USER_OBJECTS X1," +
                             "        SYS.ALL_TAB_COMMENTS X2" +
                             "  where X1.OBJECT_TYPE = 'TABLE'" +
                             "  and   X1.OBJECT_NAME not like '%$%'" +
                             "  and   X2.TABLE_NAME = X1.OBJECT_NAME" +
                             "  and   X2.OWNER      = '" + schema.Name + "'" +
                             "  and   X2.TABLE_TYPE = 'TABLE'" +
                             "  order by X1.OBJECT_NAME";

                OracleCommand cmd = new OracleCommand(sql, connection);

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Fetch all tables
                        if (!reader.IsDBNull(0))
                        {
                            tableList.Add(new DataModelTableInfo(reader.GetString(0),
                                                                        reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1),
                                                                        reader.IsDBNull(2) ? string.Empty : reader.GetString(2)));
                        }
                    }
                }
            }

            return tableList;
        }

        public static Type GetClrTypeFromDbType(string dbDataType, int? length, int? precision, int? scale, IList<PropertyCode> propertyCodes)
        {
            if (dbDataType == "VARCHAR2")
            {
                // Check if codes has the code '1' and '0' only, then the Type is a boolean.
                if (propertyCodes != null && propertyCodes.Count == 2 &&
                    ((propertyCodes[0].Code == "0" || propertyCodes[0].Code == "1") &&
                     (propertyCodes[1].Code == "0" || propertyCodes[1].Code == "1")))
                {
                    // Since this is a boolean then the codes should be removed.
                    propertyCodes.Clear();

                    return typeof(bool);
                }
                else
                {
                    return typeof(string);
                }
            }
            if (dbDataType == "CHAR")
            {
                return typeof(string);
            }
            if (dbDataType == "CLOB")
            {
                return typeof(string);
            }
            else if (dbDataType == "DATE" || dbDataType.StartsWith("TIMESTAMP"))
            {
                return typeof(DateTime);
            }
            else if (dbDataType == "ROWID")
            {
                return typeof(string);
            }
            else if (dbDataType == "BOOLEAN")
            {
                return typeof(bool);
            }
            else if (dbDataType == "DECIMAL")
            {
                return typeof(decimal);
            }
            else if (dbDataType == "DOUBLE")
            {
                return typeof(double);
            }
            else if (dbDataType == "NUMBER")
            {
                scale = scale ?? 0;
                precision = precision ?? 0;

                if (scale == 0 && precision <= 9)
                {
                    return typeof(int);
                }
                else if (scale == 0 && precision <= 18)
                {
                    return typeof(long);
                }
                else if (scale == 0)
                {
                    return typeof(decimal);
                }
                else
                {
                    if (precision < 16)
                    {
                        return typeof(double);
                    }
                    else if (precision <= 38)
                    {
                        return typeof(decimal);
                    }
                    else
                        throw new ArgumentException("The specified precision is too high (above 38).", "precision");
                }
            }
            else if (dbDataType == "BLOB")
            {
                return typeof(byte[]);
            }
            else if (dbDataType == null)
            {
                return typeof(string);
            }

            throw new ArgumentException(string.Format("The specified data type (\"{0}\") is not supported.", dbDataType), "dbDataType");
        }

        public static string GetDbType(MappedProperty mappedProperty)
        {
            string propType = string.Empty;

            if (mappedProperty != null && mappedProperty.TargetProperty != null)
            {
                if (mappedProperty.TargetProperty.StorageInfo != null)
                {
                    propType = string.Format("{0}.{1}%type",
                                            mappedProperty.TargetProperty.StorageInfo.TableName,
                                            mappedProperty.TargetProperty.StorageInfo.ColumnName);
                }
                else
                {
                    if (mappedProperty.TargetProperty.Type == typeof(string))
                    {
                        propType = "varchar2";
                    }
                    else if (mappedProperty.TargetProperty.Type == typeof(int) ||
                             mappedProperty.TargetProperty.Type == typeof(long) ||
                             mappedProperty.TargetProperty.Type == typeof(decimal) ||
                             mappedProperty.TargetProperty.Type == typeof(double))
                    {
                        propType = "number";
                    }
                    else if (mappedProperty.TargetProperty.Type == typeof(DateTime))
                    {
                        propType = "datetime";
                    }
                    else if (mappedProperty.TargetProperty.Type == typeof(bool))
                    {
                        propType = "boolean";
                    }
                }
            }

            return propType;
        }

        public static IList<BusinessEntity> AnalyzeTables(Schema schema, IList<string> tableNames, bool allTables)
        {
            int propCount = 0;
            int propCodesCount = 0;
            int propWithCodeCount = 0;
            List<string> beNames = new List<string>();
            List<BusinessEntity> businessEntities = new List<BusinessEntity>();

            string tableText = string.Empty;

            if (!allTables && tableNames != null && tableNames.Count > 0)
            {
                foreach (string tableName in tableNames)
                {
                    tableText += string.Format("'{0}',", tableName);
                }

                tableText = tableText.TrimEnd(new char[] { ',', ' ' });
            }

            using (OracleConnection conn = new OracleConnection(schema.ConnectionString))
            {
                conn.Open();

                string sql = "select X1.TABLE_NAME"
                    + ",X1.COLUMN_NAME"
                    + ",X1.DATA_TYPE"
                    + ",DECODE(X1.CHAR_LENGTH, 0, X1.DATA_LENGTH, X1.CHAR_LENGTH) DATA_LENGTH"
                    + ",X1.DATA_PRECISION"
                    + ",X1.DATA_SCALE"
                    + ",X2.COMMENTS COL_COMMENT"
                    + ",X3.COMMENTS TAB_COMMENT"
                    + ",decode((select COLUMN_NAME from SYS.ALL_CONS_COLUMNS ACC"
                              + " where ACC.OWNER = '" + schema.Name + "'"
                              + " and   ACC.TABLE_NAME = X1.TABLE_NAME"
                              + " and   ACC.COLUMN_NAME = X1.COLUMN_NAME"
                              + " group by COLUMN_NAME"
                              + "), X1.COLUMN_NAME, '1', '0') HAS_CHECK_CONSTRAINT"
                    + " from SYS.ALL_TAB_COLS X1,"
                    + " SYS.ALL_COL_COMMENTS X2,"
                    + " SYS.ALL_TAB_COMMENTS X3"
                    + " where X1.OWNER = '" + schema.Name + "'"
                    + " and X1.TABLE_NAME not like 'BIN$%'"
                    + " and X2.TABLE_NAME = X1.TABLE_NAME"
                    + " and X2.COLUMN_NAME = X1.COLUMN_NAME"
                    + " and X2.OWNER = X1.OWNER"
                    + " and X3.TABLE_NAME = X1.TABLE_NAME"
                    + " and X3.OWNER = X1.OWNER"
                    + " and X3.TABLE_TYPE = 'TABLE'"
                    + (allTables ? string.Empty : string.Format(" and X1.TABLE_NAME in ({0})", tableText))
                    + " order by X1.OWNER, X1.TABLE_NAME, X1.COLUMN_NAME";

                OracleCommand cmd = new OracleCommand(sql, conn);

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    string tableName = "";
                    BusinessEntity entity = null;

                    while (reader.Read())
                    {

                        PropertyStorageInfo info = new PropertyStorageInfo();

                        info.ColumnName = reader["COLUMN_NAME"].ToString();

                        if (reader["TABLE_NAME"].ToString() != tableName)
                        {
                            if (entity != null)
                            {
                                businessEntities.Add(entity);
                            }

                            tableName = reader["TABLE_NAME"].ToString();

                            string entityName = GetCaptionFromComment(reader["TAB_COMMENT"].ToString());

                            if (string.IsNullOrEmpty(entityName))
                                entityName = tableName;

                            entity = new BusinessEntity();

                            int c = beNames.Count(n => n == entityName);

                            beNames.Add(entityName);

                            if (c > 0)
                            {
                                entityName = string.Format("entityName{0}", c + 1);
                            }

                            entity.Name = entityName;
                            entity.TableName = tableName;
                            entity.Properties = new List<Property>();
                            entity.Description = reader["TAB_COMMENT"].ToString();
                            entity.Application = schema.Application;
                        }

                        info.Schema = schema;
                        info.TableName = tableName;
                        info.StorageType = reader["DATA_TYPE"].ToString();
                        info.Length = Convert.ToInt32(reader["DATA_LENGTH"]);

                        if (!(reader["DATA_PRECISION"] is DBNull))
                            info.Precision = Convert.ToInt32(reader["DATA_PRECISION"]);

                        if (!(reader["DATA_SCALE"] is DBNull))
                            info.Scale = Convert.ToInt32(reader["DATA_SCALE"]);

                        Property prop = new Property();

                        string name = GetCaptionFromComment(reader["COL_COMMENT"].ToString());

                        if (string.IsNullOrEmpty(name))
                            name = reader["COLUMN_NAME"].ToString();

                        // Parse codes if it has check constraints
                        if (reader["HAS_CHECK_CONSTRAINT"].ToString() == "1")
                        {
                            prop.Codes = GetPropertyCodes(info);

                            // Set the property for each code in list
                            if (prop.Codes != null && prop.Codes.Count > 0)
                            {
                                foreach (PropertyCode code in prop.Codes)
                                {
                                    code.Property = prop;
                                }
                            }
                        }

                        prop.Name = name;
                        prop.Type = GetClrTypeFromDbType(info.StorageType, info.Length, info.Precision, info.Scale, prop.Codes);

                        // Check if its varchar2 field and it's not set to bool already
                        // then parse the column comment to see if it should be a bool anyway
                        if (info.StorageType == "VARCHAR2" && prop.Type != typeof(bool))
                        {
                            // Try to match something like this:
                            //      (1=Yes,0=No)
                            //      (1=Yes,0=No, default: Yes)
                            //      (1=Yes,0=No, default: No)
                            if (Regex.Match(reader["COL_COMMENT"].ToString(),
                                            @"\(\s*((1\s*=\s*Yes\s*,\s*0\s*=\s*No)|(0\s*=\s*No\s*,\s*1\s*=\s*Yes))" +
                                            @"((\s*\))|(\s*,[^=]*\)))").Success)
                            {
                                prop.Type = typeof(bool);
                                prop.Codes.Clear();
                            }
                        }

                        if (prop.Codes.Count > 0)
                        {
                            propCodesCount += prop.Codes.Count;
                            propWithCodeCount++;
                        }

                        prop.BusinessEntity = entity;
                        prop.StorageInfo = info;


                        Hint hint = new Hint();
                        hint.Text = GetHintTextFromComment(reader["COL_COMMENT"].ToString());

                        prop.Hint = hint;

                        propCount++;

                        entity.Properties.Add(prop);
                    }

                    // Save the last entity
                    if (entity != null)
                    {
                        businessEntities.Add(entity);
                    }
                }
            }

            return businessEntities;
        }

        public static string GetCaptionFromComment(string comment)
        {
            string caption = comment.Split('\r', '\n')[0];

            string[] tokens = caption.Split(new char[] { ' ', '-', '/', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

            caption = "";
            bool ignore = false;

            foreach (string token in tokens)
            {
                string word = token.Trim().ToLower();

                if (word.Contains("("))
                {
                    ignore = true;
                    continue;
                }

                if (word.Contains(")"))
                {
                    ignore = false;
                    continue;
                }

                if (word == "identity")
                    word = "id";

                if (!ignore)
                    caption += word.ToUpper()[0] + word.Substring(1);
            }

            // Now remove all chars that is not alphanumeric
            caption = Regex.Replace(caption, @"[^\w]", "");

            if (caption.Length > 50)
                caption = caption.Substring(0, 50);

            return caption;
        }

        public static string GetHintTextFromComment(string comment)
        {
            string[] splitt = comment.Split('\r', '\n');
            

            if (splitt.Count() > 1)
            {
                int i = 1;
                StringBuilder sb = new StringBuilder();
                while (splitt.Count() > i)
                {
                    if (i != 1)
                    {
                        sb.Append("\n");
                    }
                    sb.Append(splitt[i]);

                    i++;
                }
                return sb.ToString().Trim() ;
            }

            return "";
        }

        private static IList<PropertyCode> GetPropertyCodes(PropertyStorageInfo info)
        {
            List<PropertyCode> codeList = null;

            using (OracleConnection connection = new OracleConnection(info.Schema.ConnectionString))
            {
                connection.Open();

                // Run a SQL for fetching each seperate constraint per column
                string constraint_sql = "select  SEARCH_CONDITION"
                                       + " from  SYS.ALL_CONS_COLUMNS X1"
                                       + "      ,SYS.ALL_CONSTRAINTS  X2"
                                       + " where X1.OWNER = :schemaName"
                                       + " and   X1.TABLE_NAME = :tableName"
                                       + " and   X1.COLUMN_NAME = :columnName"
                                       + " and   X2.OWNER = X1.OWNER"
                                       + " and   X2.CONSTRAINT_NAME = X1.CONSTRAINT_NAME"
                                       + " and   X2.TABLE_NAME = X1.TABLE_NAME"
                                       + " and   X2.CONSTRAINT_TYPE = 'C'";

                OracleCommand command = new OracleCommand(constraint_sql, connection);

                command.Parameters.Add("schemaName", OracleDbType.Varchar2, info.Schema.Name, ParameterDirection.Input);
                command.Parameters.Add("tableName", OracleDbType.Varchar2, info.TableName, ParameterDirection.Input);
                command.Parameters.Add("columnName", OracleDbType.Varchar2, info.ColumnName, ParameterDirection.Input);

                command.InitialLONGFetchSize = 500;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        codeList = new List<PropertyCode>();

                        while (reader.Read())
                        {
                            string constraint = reader["SEARCH_CONDITION"].ToString();

                            if (!string.IsNullOrEmpty(constraint))
                            {
                                IList<string> keyList = ParseCheckConstraint(constraint);

                                if (keyList != null && keyList.Count > 0)
                                {
                                    foreach (string key in keyList)
                                    {
                                        // Create the property
                                        PropertyCode pc = new PropertyCode();

                                        pc.Code = SingleQuoteFromDatabase(key);
                                        pc.Value = GetPropertyCodeText(info.ColumnName, key, "EN", info.Schema);

                                        // Add code to list
                                        codeList.Add(pc);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // If list is empty then return null instead.
            if (codeList != null && codeList.Count == 0)
            {
                codeList = null;
            }

            return codeList;
        }

        private static IList<string> ParseCheckConstraint(string checkConstraint)
        {
            // Regular expression for a checkconstraint:
            // Example: DEP_PICKSTAT in ('C','O','R','H','F')
            Regex regEx = new Regex(@"(?ni:" +                          // Regex options: Explicitcapture and Ignore case
                                     @"(?<COLUMN>\w+)\s+in\s*\(\s*" +   // Match the name of the column
                                      @"(\s*'" +
                                       @"(?<KEY>" +                     // Start of matching a check constraint value (our KEY)
                                         "(" +
                                            @"('{2})|([^']*)" +         // We match any character that isn't a single quote "'". We also match two single quotes "''".
                                          ")+" +
                                       @")'\s*" +                       // The value ends with a single quote and possibly one or more whitespaces..
                                      @"(,|\))" +                       // .. and then there is either a comma "," or an ending parenthesis ")".
                                      ")+" +                            // There can be one or several KEY's found.         
                                     ")");                              // End of Regular expression.

            List<string> keyList = null;

            // Check if there is any matches against the regular expression
            MatchCollection mc = regEx.Matches(checkConstraint);

            if (mc.Count == 1)
            {
                // Check if we got any keys
                if (mc[0].Groups["KEY"].Captures.Count > 0)
                {
                    keyList = new List<string>();

                    for (int i = 0; i < mc[0].Groups["KEY"].Captures.Count; i++)
                    {
                        keyList.Add(mc[0].Groups["KEY"].Captures[i].Value);
                    }
                }
            }

            return keyList;
        }

        private static string GetPropertyCodeText(string columnName, string code, string language, Schema schema)
        {
            string text = string.Empty;

            using (OracleConnection connection = new OracleConnection(schema.ConnectionString))
            {
                connection.Open();

                // Select the text from OLACODNL table
                string olacod_sql = "select   OLACODTXT"
                                    + " from  OLACODNL"
                                    + " where NLANGCOD = :nlangcod"
                                    + " and   OLAID = :olaid"
                                    + " and   OLACOD = :olacod";

                OracleCommand command = new OracleCommand(olacod_sql, connection);

                command.Parameters.Add("nlangcod", OracleDbType.Varchar2, (language == string.Empty ? "EN" : language), ParameterDirection.Input);
                command.Parameters.Add("olaid", OracleDbType.Varchar2, columnName, ParameterDirection.Input);
                command.Parameters.Add("olacod", OracleDbType.Varchar2, code, ParameterDirection.Input);

                command.InitialLONGFetchSize = 255;

                using (OracleDataReader olacod_reader = command.ExecuteReader())
                {
                    if (olacod_reader.HasRows)
                    {
                        while (olacod_reader.Read())
                        {
                            text = SingleQuoteFromDatabase(olacod_reader["OLACODTXT"].ToString());
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                // Couldn't find text for code in OLACODNL table
                text = "Text for code '" + SingleQuoteFromDatabase(code) + "'";
            }

            return text;
        }

        private static string SingleQuoteFromDatabase(string text)
        {
            return text.Replace("''", "'");
        }

        public static DataModelChanges CompareEntities(IList<BusinessEntity> newEntities, IList<BusinessEntity> existingEntities)
        {
            return CompareEntities(newEntities, existingEntities, null);
        }

        public static DataModelChanges CompareEntities(IList<BusinessEntity> newEntities,  IList<BusinessEntity> existingEntities, DataModelChanges changesToApply)
        {
            DataModelChanges dataModelChanges = new DataModelChanges();
            
            bool hintsOnly = false;

            if (changesToApply != null)
            {
                hintsOnly = changesToApply.HintsOnly;
            }

            if (newEntities != null && existingEntities != null)
            {
                foreach (BusinessEntity existingEntity in existingEntities)
                {
                    if (!string.IsNullOrEmpty(existingEntity.TableName))
                    {
                        BusinessEntity newEntity = newEntities.Where(be => be.TableName == existingEntity.TableName).FirstOrDefault<BusinessEntity>();

                        // Check if the entity still exists in what has been fetched
                        if (newEntity != null)
                        {
                            // Check for any changes of the entity itself (except properties)

                            if (existingEntity.Description != newEntity.Description)
                            {
                                if (changesToApply == null)
                                {
                                    dataModelChanges.Add(existingEntity,
                                                    DataModelChangeType.Modified,
                                                    string.Format("BusinessEntity - Changed description from \"{0}\" to \"{1}\".",
                                                                  existingEntity.Description,
                                                                  newEntity.Description));
                                }
                                else if (changesToApply[existingEntity] != null &&
                                         changesToApply[existingEntity].Apply &&
                                         !hintsOnly)
                                {
                                    existingEntity.Description = newEntity.Description;
                                }
                            }

                            if (existingEntity.Name != newEntity.Name)
                            {
                                if (changesToApply == null)
                                {
                                    dataModelChanges.Add(existingEntity,
                                                    DataModelChangeType.Modified,
                                                    string.Format("BusinessEntity - Changed name from \"{0}\" to \"{1}\".",
                                                                  existingEntity.Name,
                                                                  newEntity.Name));
                                }
                                else if (changesToApply[existingEntity] != null &&
                                         changesToApply[existingEntity].Apply &&
                                         !hintsOnly)
                                {
                                    existingEntity.Name = newEntity.Name;
                                }
                            }

                            // Check all properties
                            foreach (Property existingProperty in existingEntity.Properties)
                            {
                                if (existingProperty.StorageInfo != null)
                                {
                                    Property newProp = newEntity.Properties.Where(p => p.StorageInfo.ColumnName == existingProperty.StorageInfo.ColumnName).FirstOrDefault<Property>();

                                    if (newProp != null)
                                    {
                                        IList<string> changes = null;

                                        // Check codes
                                        if (changesToApply == null)
                                        {
                                            changes = CheckCodes(existingProperty, newProp, false);

                                            if (changes != null &&
                                                changes.Count > 0)
                                            {
                                                dataModelChanges.Add(existingProperty, DataModelChangeType.Modified, changes);
                                            }
                                        }
                                        else if (changesToApply[existingProperty] != null &&
                                                 changesToApply[existingProperty].Apply &&
                                                 !hintsOnly)
                                        {
                                            changes = CheckCodes(existingProperty, newProp, true);
                                        }


                                        // Deleted Hint
                                        if (changesToApply == null)
                                        {
                                            changes = CheckDeletedHints(existingProperty, newProp, false);
                                            if (changes != null && changes.Count > 0)
                                            {
                                                //dataModelChanges.Add(existingProperty, DataModelChangeType.Modified, "Deleted Hint on Property");
                                                dataModelChanges.Add(existingProperty, DataModelChangeType.DeletedHint, changes);
                                            }

                                        }
                                        else if (changesToApply[existingProperty] != null &&
                                                 changesToApply[existingProperty].Apply &&
                                                 changesToApply[existingProperty].ContainDataModelChangeType(DataModelChangeType.DeletedHint))
                                        {
                                            //existingProperty.Hint = null;
                                        }

                                        // Check Hint
                                        if (changesToApply == null)
                                        {
                                            changes = CheckHintUpdates(existingProperty, newProp, false);

                                            if (changes != null && changes.Count > 0)
                                            {
                                                // The Hint still in use?
                                                IList<Property> props = GetPropertysUsinHint(existingEntities, existingProperty.Hint.Id);
                                                bool reusedHintOnProperty = (props.Count > 1);

                                                if (reusedHintOnProperty)
                                                {
                                                    existingProperty.Hint = null;
                                                }
                                                else
                                                {
                                                    dataModelChanges.Add(existingProperty, DataModelChangeType.ModifiedHint, changes);
                                                }                                                                                              
                                            }
                                        }
                                        else if (changesToApply[existingProperty] != null &&
                                                 changesToApply[existingProperty].Apply &&
                                                 changesToApply[existingProperty].ContainDataModelChangeType(DataModelChangeType.ModifiedHint))
                                        {
                                            changes = CheckHintUpdates(existingProperty, newProp, true);
                                        }


                                        // Check New Hints
                                        if (changesToApply == null)
                                        {
                                            changes = CheckNewHints(existingProperty, newProp, false);

                                            if (changes != null &&
                                                    changes.Count > 0)
                                            {
                                                //dataModelChanges.Add(existingProperty, DataModelChangeType.Modified, "New Hint on Property");
                                                dataModelChanges.Add(existingProperty, DataModelChangeType.NewHint, changes);
                                            }
                                        }
                                        else if (changesToApply[existingProperty] != null &&
                                                 changesToApply[existingProperty].Apply &&
                                                 changesToApply[existingProperty].ContainDataModelChangeType(DataModelChangeType.NewHint))
                                        {
                                            changes = CheckNewHints(existingProperty, newProp, true);
                                        }


                                        // Check StorageInfo
                                        if (changesToApply == null)
                                        {
                                            changes = CheckStorageInfo(existingProperty, newProp, true);

                                            if (changes != null &&
                                                changes.Count > 0)
                                            {
                                                dataModelChanges.Add(existingProperty, DataModelChangeType.Modified, changes);
                                            }
                                        }
                                        else if (changesToApply[existingProperty] != null &&
                                                 changesToApply[existingProperty].Apply &&
                                                 changesToApply[existingProperty].ContainDataModelChangeType(DataModelChangeType.Modified) &&
                                                 !hintsOnly)
                                        {
                                            changes = CheckStorageInfo(existingProperty, newProp, true);
                                        }


                                        // Check types
                                        if (existingProperty.Type != newProp.Type)
                                        {
                                            // It's valid for the user to have changed type from 
                                            // string to boolean if it's set in the database to be VARCHAR2 with
                                            // the length 1.
                                            if (!(existingProperty.Type == typeof(bool) &&
                                                  newProp.Type == typeof(string) &&
                                                  newProp.StorageInfo.Length == 1))
                                            {
                                                if (changesToApply == null)
                                                {
                                                    dataModelChanges.Add(existingProperty,
                                                                    DataModelChangeType.Modified,
                                                                    string.Format("Property ({0}.{1} - \"{2}\") - Changed type from \"{3}\" to \"{4}\".",
                                                                                  existingProperty.StorageInfo.TableName,
                                                                                  existingProperty.StorageInfo.ColumnName,
                                                                                  existingProperty.Name,
                                                                                  existingProperty.Type,
                                                                                  newProp.Type));
                                                }
                                                else if (changesToApply[existingProperty] != null &&
                                                         changesToApply[existingProperty].Apply &&
                                                         !hintsOnly)
                                                {
                                                    existingProperty.Type = newProp.Type;
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {
                                        // Property doesn't exist anymore
                                        if (changesToApply == null)
                                        {
                                            if (existingProperty.StorageInfo != null)
                                            {
                                                dataModelChanges.Add(existingProperty,
                                                                DataModelChangeType.Deleted,
                                                                string.Format("Property {0} - Deleted",
                                                                              existingProperty.Name));
                                            }
                                        }
                                        else
                                        {
                                            if (changesToApply.ContainsKey(existingProperty))
                                            {
                                                if (hintsOnly)
                                                {
                                                    changesToApply[existingProperty].Apply = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Check all new properties if they are totally new
                            foreach (Property newProp in newEntity.Properties)
                            {
                                if (newProp.StorageInfo != null)
                                {
                                    if (existingEntity.Properties.Where(p => p.StorageInfo != null && p.StorageInfo.ColumnName == newProp.StorageInfo.ColumnName).Count() == 0)
                                    {
                                        if (changesToApply == null)
                                        {
                                            string changeString = string.Format("Add Hint to new Property - Whith value: \"{0}\".",
                                                                                 newProp.Hint.Text);


                                            dataModelChanges.Add(newProp,
                                                            DataModelChangeType.New,
                                                            string.Format("Property \"{0}\" - New",
                                                                          newProp.Name));

                                            if (newProp.Hint.Text.Trim().Length > 0)
                                            {
                                                dataModelChanges.Add(newProp, DataModelChangeType.NewHint, changeString);
                                            }
                                            else
                                            {
                                                newProp.Hint = null;
                                            }

                                            // Set the property to have the new BusinessEntity
                                            newProp.BusinessEntity = existingEntity;
                                        }
                                        else
                                        {

                                            if (changesToApply.ContainsKey(newProp))
                                            {
                                                if (hintsOnly)
                                                {
                                                    changesToApply[newProp].Apply = false;
                                                }

                                                if (changesToApply[newProp].Apply)
                                                {
                                                    // Add the property to the BusinessEntity
                                                    existingEntity.Properties.Add(newProp);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Loop through all new business entities and try to find those that
                // doesn't already exist in database.
                foreach (BusinessEntity newEntity in newEntities)
                {
                    // See if it doesn't exist
                    if (existingEntities.Where(ee => ee.TableName == newEntity.TableName).Count() == 0)
                    {
                        if (changesToApply == null)
                        {
                            string changeTextBe = string.Format("BusinessEntity - \"{0}\" New.", newEntity.Name);

                            dataModelChanges.Add(newEntity, DataModelChangeType.New, changeTextBe);

                            foreach (Property property in newEntity.Properties)
                            {
                                dataModelChanges.Add(property, DataModelChangeType.New, string.Format("Property \"{0}\" - New", property.Name));

                                string changeStringHint = string.Format("Add Hint to new Property - Whith value: \"{0}\".",
                                                                                 property.Hint.Text);

                                dataModelChanges.Add(property, DataModelChangeType.NewHint, changeStringHint);

                                
                            }
                        }
                        else
                        {
                            int index = 0;
                            while (index < newEntity.Properties.Count)
                            {
                                if (changesToApply.ContainsKey(newEntity.Properties[index]))
                                {
                                    if (!changesToApply[newEntity.Properties[index]].Apply)
                                    {
                                        newEntity.Properties.RemoveAt(index);
                                        index--;
                                    }
                                }

                                index++;
                            }

                        }
                    }
                }
            }

            return dataModelChanges;
        }

        private static IList<Property> GetPropertysUsinHint(IList<BusinessEntity> existingEntities, Guid guid)
        {
            IList<Property> propList = new List<Property>();

            foreach (BusinessEntity existingEntity in existingEntities)
            {
                foreach (Property prop in existingEntity.Properties)
                {
                    if (prop.Hint != null && prop.Hint.Id == guid)
                    {
                        propList.Add(prop);
                    }
                }

            }
            return propList;
        }

        private static IList<string> CheckStorageInfo(Property existingProperty, Property newProperty, bool applyChanges)
        {
            List<string> changes = new List<string>();

            if (existingProperty.StorageInfo.ColumnName != newProperty.StorageInfo.ColumnName)
            {
                // Code has changed value
                changes.Add(string.Format("PropertyStorageInfo - ColumnName has changed from \"{0}\" to \"{1}\".",
                                          existingProperty.StorageInfo.ColumnName,
                                          newProperty.StorageInfo.ColumnName));

                if (applyChanges)
                {
                    existingProperty.StorageInfo.ColumnName = newProperty.StorageInfo.ColumnName;
                }
            }

            if (existingProperty.StorageInfo.Length != newProperty.StorageInfo.Length)
            {
                // Code has changed value
                changes.Add(string.Format("PropertyStorageInfo - Length has changed from \"{0}\" to \"{1}\".",
                                          existingProperty.StorageInfo.Length,
                                          newProperty.StorageInfo.Length));

                if (applyChanges)
                {
                    existingProperty.StorageInfo.Length = newProperty.StorageInfo.Length;
                }
            }

            if (existingProperty.StorageInfo.Precision != newProperty.StorageInfo.Precision)
            {
                // Code has changed value
                changes.Add(string.Format("PropertyStorageInfo - Precision has changed from \"{0}\" to \"{1}\".",
                                          existingProperty.StorageInfo.Precision,
                                          newProperty.StorageInfo.Precision));

                if (applyChanges)
                {
                    existingProperty.StorageInfo.Precision = newProperty.StorageInfo.Precision;
                }
            }

            if (existingProperty.StorageInfo.Scale != newProperty.StorageInfo.Scale)
            {
                // Code has changed value
                changes.Add(string.Format("PropertyStorageInfo - Scale has changed from \"{0}\" to \"{1}\".",
                                          existingProperty.StorageInfo.Scale,
                                          newProperty.StorageInfo.Scale));

                if (applyChanges)
                {
                    existingProperty.StorageInfo.Scale = newProperty.StorageInfo.Scale;
                }
            }

            if (existingProperty.StorageInfo.StorageType != newProperty.StorageInfo.StorageType)
            {
                // Code has changed value
                changes.Add(string.Format("PropertyStorageInfo - StorageType has changed from \"{0}\" to \"{1}\".",
                                          existingProperty.StorageInfo.StorageType,
                                          newProperty.StorageInfo.StorageType));

                if (applyChanges)
                {
                    existingProperty.StorageInfo.StorageType = newProperty.StorageInfo.StorageType;
                }
            }

            if (existingProperty.StorageInfo.TableName != newProperty.StorageInfo.TableName)
            {
                // Code has changed value
                changes.Add(string.Format("PropertyStorageInfo - TableName has changed from \"{0}\" to \"{1}\".",
                                          existingProperty.StorageInfo.TableName,
                                          newProperty.StorageInfo.TableName));

                if (applyChanges)
                {
                    existingProperty.StorageInfo.TableName = newProperty.StorageInfo.TableName;
                }
            }

            return changes;
        }

        private static IList<string> CheckCodes(Property existingProperty, Property newProperty, bool applyChanges)
        {
            List<string> changes = new List<string>();

            if (existingProperty.Codes.Count > 0)
            {
                List<PropertyCode> removeCodes = new List<PropertyCode>();

                foreach (PropertyCode existingCode in existingProperty.Codes)
                {
                    PropertyCode newCode = newProperty.Codes.Where(c => c.Code == existingCode.Code).FirstOrDefault<PropertyCode>();

                    if (newCode != null)
                    {
                        // Check if it has the same value
                        if (existingCode.Value != newCode.Value)
                        {
                            // Code has changed value
                            changes.Add(string.Format("PropertyCode (\"{0}\") - Changed value from \"{1}\" to \"{2}\".",
                                                      newCode.Code,
                                                      existingCode.Value,
                                                      newCode.Value));

                            if (applyChanges)
                            {
                                existingCode.Value = newCode.Value;
                            }
                        }
                    }
                    else
                    {
                        // Code can't be found, must have been deleted
                        changes.Add(string.Format("PropertyCode (\"{0}\") - Deleted", existingCode.Code));

                        if (applyChanges)
                        {
                            removeCodes.Add(existingCode);
                        }
                    }
                }

                if (applyChanges && removeCodes.Count > 0)
                {
                    foreach (PropertyCode code in removeCodes)
                    {
                        existingProperty.Codes.Remove(code);
                    }
                }
            }

            if (newProperty.Codes.Count > 0)
            {
                foreach (PropertyCode newCode in newProperty.Codes)
                {
                    // Check if we have a completely new code
                    if (existingProperty.Codes.Where(c => c.Code == newCode.Code).Count() == 0)
                    {
                        if (applyChanges)
                        {
                            PropertyCode newPropCode = new PropertyCode();

                            newPropCode.Property = existingProperty;
                            newPropCode.Code = newCode.Code;
                            newPropCode.Value = newCode.Value;

                            existingProperty.Codes.Add(newPropCode);
                        }

                        // Code can't be found, must have been deleted
                        changes.Add(string.Format("PropertyCode \"{0}\" - New with value \"{1}\".", newCode.Code, newCode.Value));
                    }
                }
            }

            return changes;
        }

        private static IList<string> CheckDeletedHints(Property existingProperty, Property newProperty, bool applyChanges)
        {
            List<string> changes = new List<string>();

            if (existingProperty.Hint != null && newProperty.Hint != null)
            {
                if ( newProperty.Hint.Text.Trim().Length == 0 )
                {
                    // Empty Hint
                    changes.Add(string.Format("Delete Hint - With value \r\n\"{0}\".",
                                              existingProperty.Hint.Text));

                    if (applyChanges)
                    {
                        existingProperty.Hint = null;
                    }
                }

            }

            return changes;
        }

        private static IList<string> CheckHintUpdates(Property existingProperty, Property newProperty, bool applyChanges)
        {
            List<string> changes = new List<string>();

            if (existingProperty.Hint != null && newProperty.Hint != null)
            {
                // Check if the Hint has the same Text
                string newText = newProperty.Hint.Text.Trim();
                string oldText = existingProperty.Hint.Text.Trim();
                bool sameText = oldText == newText;
                bool hasText = (newText.Length > 0);
                if ( !sameText && hasText )
                {
                    // The Hint has changed Text
                    changes.Add(string.Format("Hint - Changed value from \r\n\"{0}\" \r\nto\r\n\"{1}\".",
                                              existingProperty.Hint.Text,
                                              newProperty.Hint.Text));

                    if (applyChanges)
                    {
                        existingProperty.Hint.Text = newProperty.Hint.Text;
                    }
                }
            }
            return changes;
        }



        private static IList<string> CheckNewHints(Property existingProperty, Property newProperty, bool applyChanges)
        {
            List<string> changes = new List<string>();

            if (existingProperty.Hint == null && newProperty.Hint != null && newProperty.Hint.Text.Trim().Length > 0)
            {
                // Property whithout a Hint
                changes.Add(string.Format("Add Hint to Property - With value: \r\n\"{0}\".",
                    newProperty.Hint.Text));

                if (applyChanges)
                {
                    existingProperty.Hint = newProperty.Hint;
                }
            }

            return changes;
        }
    }
}

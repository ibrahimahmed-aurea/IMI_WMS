using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.HbmGenerator.Support;
using Imi.HbmGenerator.Attributes;

namespace Cdc.HbmGenerator.Output
{
    public class HibernateMapGenerator
    {
        public static string Generate(Class baseClass)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            output.AppendLine("<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\">");
            output.AppendLine(string.Format("  <class name=\"{0}, {1}\" table=\"{2}\">", baseClass.Type.FullName
                                                                                       , baseClass.AssemblyShortName
                                                                                       , baseClass.StorageHint.TableName));


            foreach (Property property in baseClass.Properties)
            {
                // Primary key
                if (property.Name == "Id")
                {
                    if (property.TypeName == typeof(System.Guid).FullName)
                    {
                        output.AppendLine("    <id name=\"Id\" type=\"System.Guid\">");
                        output.AppendLine("      <column name=\"Id\" not-null=\"true\"/>");
                        output.AppendLine("      <generator class=\"assigned\"/>");
                        output.AppendLine("    </id>");
                    }
                    else
                    {
                        output.AppendLine("    <id name=\"Id\" type=\"Int32\">");
                        output.AppendLine("      <column name=\"Id\" not-null=\"true\"/>");
                        output.AppendLine("      <generator class=\"native\"/>");
                        output.AppendLine("    </id>");
                    }
                }
                else
                {
                    // Collection
                    if (property.IsCollection)
                    {
                        output.AppendLine(string.Format("    <bag name=\"{0}\" inverse=\"{1}\" lazy=\"{2}\" cascade=\"{3}\"{4}>", property.Name
                                                                                                                             , BoolToString(property.CollectionHint.Inverse)
                                                                                                                             , BoolToString(property.CollectionHint.Lazy)
                                                                                                                             , CascadeOperationToString(property.CollectionHint.Cascade)
                                                                                                                             ,FetchModeToString(property.CollectionHint.Fetch)));


                        string columnName = null;

                        if (!string.IsNullOrEmpty(property.CollectionHint.Column))
                            columnName = property.CollectionHint.Column;
                        else
                            columnName = string.Format("{0}Id", baseClass.Type.Name);

                        output.AppendLine(string.Format("      <key column=\"{0}\"/>", columnName));
                        output.AppendLine(string.Format("      <one-to-many class=\"{0}, {1}\"/>", property.TypeName
                                                                                                 , baseClass.AssemblyShortName));
                        output.AppendLine("    </bag>");
                    }
                    else
                    {
                        // Reference
                        if (property.IsReference)
                        {
                            string foreignKeyText = "";

                            if(! string.IsNullOrEmpty(property.StorageHint.ForeignKey))
                                foreignKeyText = string.Format(" foreign-key=\"{0}\"", property.StorageHint.ForeignKey);

                            string cascadeString = "";

                            if (property.StorageHint.Cascade != CascadeAssociationOperation.Default)
                            {
                                cascadeString = string.Format(" cascade=\"{0}\"", CascadeAssociationOperationToString(property.StorageHint.Cascade));
                            }

                            string uniqueAddon = "";

                            if (!string.IsNullOrEmpty(property.StorageHint.UniqueKey))
                            {
                                uniqueAddon = string.Format(" unique-key=\"{0}\"", property.StorageHint.UniqueKey);
                            }

                            // many-to-one doesn't support lazy=true, only lazy=false (and lazy=proxy)
                            string lazyString = "";

                            if (!property.StorageHint.Lazy)
                            {
                                lazyString = " lazy=\"false\"";
                            }

                            string fetchString = FetchModeToString(property.CollectionHint.Fetch);

                            output.AppendLine(string.Format("    <many-to-one name=\"{0}\"{1}{2}{3} class=\"{4}, {5}\"{6}{7}>"
                                                                       , property.Name
                                                                       , lazyString
                                                                       , fetchString
                                                                       , uniqueAddon
                                                                       , property.TypeName
                                                                       , baseClass.AssemblyShortName
                                                                       , foreignKeyText
                                                                       , cascadeString));

                            string columnName = null;

                            if (property.Name == property.StorageHint.Column)
                                columnName = string.Format("{0}Id", property.Name);
                            else
                                columnName = property.StorageHint.Column;

                            output.AppendLine(string.Format("      <column name=\"{0}\" not-null=\"{2}\"/>", columnName
                                                                                                           , property.StorageHint.Length
                                                                                                           , BoolToString(property.StorageHint.IsMandatory)));
                            output.AppendLine("    </many-to-one>");
                        }
                        else
                        {
                            // Regular type
                            string hibernateTypeName = TranslateToHibernateType(property.TypeName);

                            string typeAddon = "";

                            if (!string.IsNullOrEmpty(property.StorageHint.Type))
                            {
                                typeAddon = string.Format(" type=\"{0}\"", property.StorageHint.Type);
                            }
                            else
                            {
                                typeAddon = string.Format(" type=\"{0}\"", hibernateTypeName);
                            }

                            string uniqueAddon = "";

                            if (!string.IsNullOrEmpty(property.StorageHint.UniqueKey))
                            {
                                uniqueAddon = string.Format(" unique-key=\"{0}\"", property.StorageHint.UniqueKey);
                            }

                            output.AppendLine(string.Format("    <property name=\"{0}\"{1}{2}>", property.Name
                                                                                               , typeAddon
                                                                                               , uniqueAddon));



                            string columnName = null;

                            if (property.Name == property.StorageHint.Column)
                                columnName = property.Name;
                            else
                                columnName = property.StorageHint.Column;

                            if (!string.IsNullOrEmpty(property.StorageHint.SqlType))
                            {
                               output.AppendLine(string.Format("      <column name=\"{0}\" sql-type=\"{1}\" not-null=\"{2}\"/>", columnName
                                                                                                                               , property.StorageHint.SqlType
                                                                                                                               , BoolToString(property.StorageHint.IsMandatory)));
                            }
                            else
                            {
                                if (hibernateTypeName == "String")
                                {
                                   output.AppendLine(string.Format("      <column name=\"{0}\" length=\"{1}\" not-null=\"{2}\"/>", columnName
                                                                                                                                 , property.StorageHint.Length
                                                                                                                                 , BoolToString(property.StorageHint.IsMandatory)));
                                }
                                else 
                                {
                                   output.AppendLine(string.Format("      <column name=\"{0}\" not-null=\"{1}\"/>", columnName
                                                                                                                  , BoolToString(property.StorageHint.IsMandatory)));
                                }
                            }

                            output.AppendLine("    </property>");


                        }
                    }


                }

            }

            output.AppendLine("  </class>");
            output.AppendLine("</hibernate-mapping>");

            return output.ToString();
        }

        private static string FetchModeToString(FetchOperation fetchOperation)
        {
            string fetchString = "";

            if (fetchOperation != FetchOperation.NotSet)
            {
                switch (fetchOperation)
                {
                    case FetchOperation.Join: fetchString = " fetch=\"join\""; break;
                    case FetchOperation.Select: fetchString = " fetch=\"select\""; break;
                }
            }
            return fetchString;
        }

        private static object CascadeOperationToString(CascadeOperation cascadeOperation)
        {
            switch (cascadeOperation)
            {
                case CascadeOperation.All:
                    return "all";
                case CascadeOperation.AllDeleteOrphan:
                    return "all-delete-orphan";
                case CascadeOperation.Delete:
                    return "delete, merge, evict";
                case CascadeOperation.None:
                    return "none";
                case CascadeOperation.SaveUpdate:
                    return "save-update, merge, evict";
                default:
                    return "none";
            }

        }

        private static object CascadeAssociationOperationToString(CascadeAssociationOperation cascadeOperation)
        {
            switch (cascadeOperation)
            {
                case CascadeAssociationOperation.All:
                    return "all";
                case CascadeAssociationOperation.Delete:
                    return "delete, merge, evict";
                case CascadeAssociationOperation.None:
                    return "none";
                case CascadeAssociationOperation.SaveUpdate:
                    return "save-update, merge, evict";
                default:
                    return "none";
            }

        }

        private static string BoolToString(bool b) {
          return b.ToString().ToLower();
        }

        private static string TranslateToHibernateType(string p)
        {
            switch (p)
            {
                case "System.String":
                    return "String";
                case "System.Int32":
                    return "Int32";
                case "System.Boolean":
                    return "Boolean";
            }

            return p;
        }

        
    }
}

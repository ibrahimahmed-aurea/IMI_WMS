using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Imi.HbmGenerator.Attributes;

namespace Cdc.HbmGenerator.Support
{
    public class ClassReflector
    {
        public static Class ReflectType(Type reflectType)
        {
            return ReflectType(reflectType, false);
        }

        public static Class ReflectType(Type reflectType, bool forceAttribute)
        {
            Class myClass = new Class();
            myClass.Type = reflectType;
            myClass.StorageHint.TableName = reflectType.Name; 

            // Check for ClassStorageHint attribute
            ClassStorageHintAttribute classHint = Attribute.GetCustomAttribute(reflectType, typeof(ClassStorageHintAttribute)) as ClassStorageHintAttribute;
            if (classHint != null)
            {
                if (!string.IsNullOrEmpty(classHint.TableName))
                {
                    myClass.StorageHint.TableName = classHint.TableName;
                }
            }

            if (forceAttribute && (classHint == null))
                return null;

            myClass.Properties = null;

            List<PropertyInfo> properties = GetSortedPropertyList(reflectType);

            foreach (PropertyInfo propertyInfo in properties)
            {
                Property property = new Property();
                property.Name = propertyInfo.Name;
                property.StorageHint.Column = propertyInfo.Name;
                property.ParentClass = myClass;
                property.TypeName = propertyInfo.PropertyType.FullName;

                // Check for PropertyStorageHint attribute
                PropertyStorageHintAttribute propertyHint = Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyStorageHintAttribute)) as PropertyStorageHintAttribute;
                if (propertyHint != null)
                {
                    // Don't add this, continue with next property
                    if (propertyHint.Ignore)
                        continue;

                    if (string.IsNullOrEmpty(propertyHint.Column))
                        propertyHint.Column = property.Name;

                    property.StorageHint = propertyHint;
                }


                if (IsCollection(propertyInfo))
                {
                    string collType = ExtractGenericBaseType(propertyInfo);
                    property.TypeName = collType;
                    property.IsCollection = true;

                    // If no hint is provided a collection is not mandatory
                    if(propertyHint == null)
                        property.StorageHint.IsMandatory = false;

                    // Collection Storage Hint
                    CollectionStorageHintAttribute collectionHint = Attribute.GetCustomAttribute(propertyInfo, typeof(CollectionStorageHintAttribute)) as CollectionStorageHintAttribute;
                    if (collectionHint != null)
                    {
                        property.CollectionHint = collectionHint;
                    }
                }
                else
                {
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        // assume all enums are system.int32
                        property.TypeName = typeof(Int32).FullName;
                    }
                    else
                    {
                        // Reference in reflectType namespace
                        if (propertyInfo.PropertyType.Namespace.StartsWith(reflectType.Namespace))
                        {
                            property.IsReference = true;
                            property.TypeName = propertyInfo.PropertyType.FullName;

                            // Reflect association
                            //property.ReferencedClass = ReflectType(Type.GetType(property.TypeName));
                        }
                        else
                        {
                            // Is the type nullable ?
                            if (IsNullable(propertyInfo))
                            {
                                string nullableType = ExtractGenericBaseType(propertyInfo);

                                // Override storage hint
                                property.StorageHint.IsMandatory = false;
                                property.TypeName = nullableType;
                            }
                        }
                    }
                }

                myClass.Properties.Add(property);
            }

            return myClass;
        }

        private const string NullableIndicator = "System.Nullable`1[[";

        private static bool IsNullable(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.FullName.StartsWith(NullableIndicator);
        }

        private const string CollectionNamespace = "System.Collection";
        
        private static bool IsCollection(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.Namespace.StartsWith(CollectionNamespace);
        }

        private static string ExtractGenericBaseType(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.PropertyType.IsGenericType)
            {
                throw new ArgumentException("Property is not a generic type: {0}", propertyInfo.PropertyType.FullName);
            }

            String[] strings = propertyInfo.PropertyType.FullName.Split(new char[] { '[' });

            if (strings.Length >= 3)
            {
                string collType = strings[2].Split(new char[] { ',' })[0];
                return collType;
            }
            else
            {
                throw new ArgumentException("Cannot parse generic type: {0}", propertyInfo.PropertyType.FullName);
            }
        }


        private static List<PropertyInfo> GetSortedPropertyList(Type orgType)
        {
            List<Type> sortedTypes = new List<Type>();

            sortedTypes.Add(orgType);

            Type currentType = orgType.BaseType;

            while (currentType.BaseType != null)
            {
                sortedTypes.Add(currentType);
                currentType = currentType.BaseType;
            }

            List<PropertyInfo> sortedProperties = new List<PropertyInfo>();

            int idx = sortedTypes.Count - 1;

            while (idx >= 0)
            {
                PropertyInfo[] properties = sortedTypes[idx].GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

                foreach (PropertyInfo propInfo in properties)
                {
                    sortedProperties.Add(propInfo);
                }

                idx--;
            }

            return sortedProperties;
        }


    }
}

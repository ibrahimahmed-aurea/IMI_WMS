using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Cdc.MetaManager.BusinessLogic
{
    public class GenericMapper
    {
        public static TCollection MapListNew<TCollection, TFrom, TTo>(IEnumerable<TFrom> from, Converter<TFrom, TTo> converter)
            where TCollection : ICollection<TTo>, new()
        {
            TCollection list = new TCollection();
            
            foreach (TFrom fromObject in from)
                list.Add(converter(fromObject));

            return list;
        }

        public static void Map(object from, object to)
        {
            Map(from, to, null);
        }

        public static void Map(object from, object to, List<string> ignorePropertyNames)
        {
            foreach (PropertyInfo propertyInfoFrom in from.GetType().GetProperties())
            {
                if (ignorePropertyNames == null ||
                    !ignorePropertyNames.Contains(propertyInfoFrom.Name))
                {
                    PropertyInfo propertyInfoTo = to.GetType().GetProperty(propertyInfoFrom.Name);
                    if (propertyInfoTo != null
                        && propertyInfoTo.PropertyType == propertyInfoFrom.PropertyType
                        && propertyInfoTo.CanWrite)
                    {
                        propertyInfoTo.SetValue(to, propertyInfoFrom.GetValue(from, null), null);
                    }
                }
            }
        }

        public static TTo MapNew<TTo>(object from)
            where TTo : new()
        {
            if (from == null)
                throw new ArgumentNullException("from");

            TTo toObject = new TTo();
                        
            Type typefrom = from.GetType();
            Type typeTo = toObject.GetType();

            if (from is IDictionary)
            {
                foreach (object key in ((IDictionary)from).Keys)
                {
                    string propertyName = key as string;

                    if (propertyName != null)
                    {
                        PropertyInfo propertyInfoTo = typeTo.GetProperty(propertyName);
                        object propertyValue = ((IDictionary)from)[key];

                        if ((propertyInfoTo != null)
                            && (propertyInfoTo.CanWrite))
                        {
                            if ((propertyValue != null)
                                && (propertyInfoTo.PropertyType == propertyValue.GetType()))
                            {
                                propertyInfoTo.SetValue(toObject, propertyValue, null);
                            }
                            else if (propertyInfoTo.PropertyType is object)
                            {
                                propertyInfoTo.SetValue(toObject, null, null);
                            }
                        }
                    }
                }
            }
            else if (toObject is IDictionary)
            {
                foreach (PropertyInfo propertyInfoFrom in typefrom.GetProperties())
                {
                    ((IDictionary)toObject).Add(propertyInfoFrom.Name, propertyInfoFrom.GetValue(from, null));
                }
            }
            else
            {
                foreach (PropertyInfo propertyInfoFrom in typefrom.GetProperties())
                {
                    PropertyInfo propertyInfoTo = typeTo.GetProperty(propertyInfoFrom.Name);
                    if (propertyInfoTo != null
                        && propertyInfoTo.PropertyType == propertyInfoFrom.PropertyType
                        && propertyInfoTo.CanWrite)
                    {
                        propertyInfoTo.SetValue(toObject, propertyInfoFrom.GetValue(from, null), null);
                    }
                }
            }

            return toObject;
        }
        
    }
}

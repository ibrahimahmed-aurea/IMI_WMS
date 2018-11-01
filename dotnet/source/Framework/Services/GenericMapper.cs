using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace Imi.Framework.Services
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


        public static TTo MapNewCommonParameters<TTo>(IList<TTo> from) where TTo : new()
        {
            TTo toObject = new TTo();

            Dictionary<PropertyInfo, object> commonValues = new Dictionary<PropertyInfo, object>();

            foreach (TTo parametersObj in from)
            {
                foreach (PropertyInfo pi in typeof(TTo).GetProperties())
                {
                    if (pi.Name != "IsModified" && pi.Name != "RowIdentity")
                    {
                        if (commonValues.ContainsKey(pi))
                        {
                            if (commonValues[pi] != null)
                            {
                                if (pi.GetValue(parametersObj, null) == null || commonValues[pi].ToString() != pi.GetValue(parametersObj, null).ToString())
                                {
                                    commonValues[pi] = null;
                                }
                            }
                        }
                        else
                        {
                            commonValues.Add(pi, pi.GetValue(parametersObj, null));
                        }
                    }
                }
            }

            foreach (KeyValuePair<PropertyInfo, object> commonValue in commonValues)
            {
                commonValue.Key.SetValue(toObject, commonValue.Value, null);
            }

            return toObject;
        }


        public static Type GetExtendedDataCarrierType(Type baseCarrierType, Type extendCarrierType)
        {
            lock (baseCarrierType)
            {
                AppDomain myDomain = System.Threading.Thread.GetDomain();
                AssemblyName myAsmName = new AssemblyName();
                myAsmName.Name = "Extended" + baseCarrierType.Name + Guid.NewGuid().ToString();

                // To generate a persistable assembly, specify AssemblyBuilderAccess.RunAndSave.
                AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName,
                                                                AssemblyBuilderAccess.RunAndSave);
                // Generate a persistable single-module assembly.
                ModuleBuilder myModBuilder =
                    myAsmBuilder.DefineDynamicModule(myAsmName.Name, myAsmName.Name + ".dll");

                TypeBuilder myTypeBuilder = myModBuilder.DefineType("Extended" + baseCarrierType.Name,
                                                                TypeAttributes.Public);

                myTypeBuilder.SetParent(baseCarrierType);


                foreach (PropertyInfo pi in extendCarrierType.GetProperties())
                {
                    if (pi.Name != "IsModified" && pi.Name != "RowIdentity")
                    {
                        if (baseCarrierType.GetProperty(pi.Name) == null)
                        {
                            FieldBuilder newFieldBldr = myTypeBuilder.DefineField(pi.Name.ToLower(), pi.PropertyType, FieldAttributes.Private);

                            PropertyBuilder newPropBldr = myTypeBuilder.DefineProperty(pi.Name, pi.Attributes, pi.PropertyType, null);

                            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                            // Define the "get" accessor method.
                            MethodBuilder newGetPropMthdBldr = myTypeBuilder.DefineMethod("get_" + pi.Name, getSetAttr, pi.PropertyType, Type.EmptyTypes);

                            ILGenerator newGetIL = newGetPropMthdBldr.GetILGenerator();

                            newGetIL.Emit(OpCodes.Ldarg_0);
                            newGetIL.Emit(OpCodes.Ldfld, newFieldBldr);
                            newGetIL.Emit(OpCodes.Ret);

                            // Define the "set" accessor method.
                            MethodBuilder newSetPropMthdBldr = myTypeBuilder.DefineMethod("set_" + pi.Name, getSetAttr, null, new Type[] { pi.PropertyType });

                            ILGenerator newSetIL = newSetPropMthdBldr.GetILGenerator();

                            newSetIL.Emit(OpCodes.Ldarg_0);
                            newSetIL.Emit(OpCodes.Ldarg_1);
                            newSetIL.Emit(OpCodes.Stfld, newFieldBldr);
                            newSetIL.Emit(OpCodes.Ret);


                            newPropBldr.SetGetMethod(newGetPropMthdBldr);
                            newPropBldr.SetSetMethod(newSetPropMthdBldr);

                        }
                    }
                }

                return myTypeBuilder.CreateType();
            }
        }

        public static void MapExtendedValues(object extendedParameters, object fromDataCarrier)
        {
            Type baseType = extendedParameters.GetType().BaseType;
            Type extendedParametersType = extendedParameters.GetType();

            foreach (PropertyInfo pi in fromDataCarrier.GetType().GetProperties())
            {
                if (pi.Name != "IsModified" && pi.Name != "RowIdentity")
                {
                    if (baseType.GetProperty(pi.Name) == null)
                    {
                        object resultValue = pi.GetValue(fromDataCarrier, null);
                        extendedParametersType.GetProperty(pi.Name).SetValue(extendedParameters, resultValue, null);
                    }
                }
            }
        }

        public static void MapNamedParameters(object toObject, object fromObject, List<string> namedParameters)
        {
            if (fromObject != null && toObject != null && namedParameters != null)
            {
                List<PropertyInfo> fromObjectProperties = fromObject.GetType().GetProperties().ToList();

                foreach (string namedParameter in namedParameters)
                {
                    if (fromObjectProperties.Where(p => p.Name == namedParameter).Count() > 0)
                    {
                        PropertyInfo fromObjectProperty = fromObjectProperties.Where(p => p.Name == namedParameter).FirstOrDefault();
                        PropertyInfo toObjectProperty = toObject.GetType().GetProperty(namedParameter);

                        if (toObjectProperty.PropertyType == fromObjectProperty.PropertyType)
                        {
                            object copyValue = fromObjectProperty.GetValue(fromObject, null);
                            toObjectProperty.SetValue(toObject, copyValue, null);
                        }
                    }
                }
            }
        }

    }
}

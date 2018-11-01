using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public class ReportInterfaceParameterList : List<ReportInterfaceParameter>
    {
        public ReportInterfaceParameterList() { }

        public void Add(string name, string type)
        {
            Add(new ReportInterfaceParameter(name, type));
        }

        public void Add(string name, string type, ReportInterfaceParameterDirection direction)
        {
            Add(new ReportInterfaceParameter(name, type, direction));
        }

        public void Add(string name, string type, string defaultValue)
        {
            Add(new ReportInterfaceParameter(name, type, defaultValue));
        }

        public void Add(string name, string type, string defaultValue, ReportInterfaceParameterDirection direction)
        {
            Add(new ReportInterfaceParameter(name, type, defaultValue, direction));
        }

        public int MaxNameLength
        {
            get
            {
                return Count > 0 ? this.Max(par => par.Name.Length) : 0;
            }
        }

        public int MaxTypeLength
        {
            get
            {
                return Count > 0 ? this.Max(par => par.Type.Length) : 0;
            }
        }

        public IList<string> GetNames()
        {
            return this.Select(par => par.Name).ToList();
        }

        public IList<string> GetNameAndTypePadded()
        {
            return GetNameAndTypePadded(0);
        }

        public IList<string> GetNameAndTypePadded(int minPaddingLength)
        {
            IList<string> list = new List<string>();

            int maxParamNameLen = Math.Max(MaxNameLength, minPaddingLength);

            foreach (ReportInterfaceParameter param in this)
            {
                string propName = param.Name.PadRight(maxParamNameLen);

                list.Add(string.Format("{0} {1}", propName, param.FullParameterType));
            }

            return list;
        }

        public IList<string> GetParameterStubsOracleProcedure()
        {
            List<string> parameterList = new List<string>();

            bool foundDefault = false;

            foreach (ReportInterfaceParameter parameter in this)
            {
                if (foundDefault)
                {
                    if (string.IsNullOrEmpty(parameter.DefaultValue))
                        parameterList.Add(string.Format("{0} => REQUIRED", parameter.Name));
                    else
                        parameterList.Add(parameter.NameStubForCallingParameter);
                }
                else
                {
                    parameterList.Add(parameter.NameStubForCallingParameter);

                    if (!string.IsNullOrEmpty(parameter.DefaultValue))
                    {
                        foundDefault = true;
                    }
                }
            }

            return parameterList;
        }

    }

}

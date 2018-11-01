using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public enum ReportInterfaceParameterDirection { In, Out, InOut };

    public class ReportInterfaceParameter
    {
        public ReportInterfaceParameter(string name, string type, string defaultValue, ReportInterfaceParameterDirection direction)
        {
            Name = name;
            ParameterType = type;
            DefaultValue = defaultValue;
            Direction = direction;
        }

        public bool HasDefaultValue { get { return !string.IsNullOrEmpty(DefaultValue); } }

        public string OracleDirection
        {
            get
            {
                switch (Direction)
                {
                    case ReportInterfaceParameterDirection.InOut: return "in out";
                    case ReportInterfaceParameterDirection.Out: return "out";
                    default: return string.Empty;
                }
            }
        }

        public string NameStubForCallingParameter
        {
            get
            {
                return string.IsNullOrEmpty(DefaultValue) ? Name : string.Format("{0} => {1}", Name, DefaultValue);
            }
        }


        public string FullParameterType
        {
            get
            {
                if (!string.IsNullOrEmpty(OracleDirection))
                {
                    if (!string.IsNullOrEmpty(DefaultValue))
                    {
                        return string.Format("{0} {1} := {2}", OracleDirection, ParameterType, DefaultValue);
                    }
                    else
                    {
                        return string.Format("{0} {1}", OracleDirection, ParameterType);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(DefaultValue))
                    {
                        return string.Format("{0} := {1}", ParameterType, DefaultValue);
                    }
                    else
                    {
                        return ParameterType;
                    }
                }
            }
        }

        public ReportInterfaceParameter(string name, string type, ReportInterfaceParameterDirection direction) : this(name, type, string.Empty, direction) { }

        public ReportInterfaceParameter(string name, string type, string defaultValue) : this(name, type, defaultValue, ReportInterfaceParameterDirection.In) { }

        public ReportInterfaceParameter(string name, string type) : this(name, type, string.Empty, ReportInterfaceParameterDirection.In) { }

        public string Name { get; private set; }
        public string ParameterType { get; private set; }
        public string DefaultValue { get; private set; }
        public string Type { get; private set; }
        public ReportInterfaceParameterDirection Direction { get; private set; }
    }
}

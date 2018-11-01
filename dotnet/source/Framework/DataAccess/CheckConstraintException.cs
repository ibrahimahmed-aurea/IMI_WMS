using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.DataAccess
{
    [global::System.Serializable]
    public class CheckConstraintException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public bool IsNullConstraint { get; set; }
        public string PropertyName { get; set; }

        public CheckConstraintException() 
        { 
            
        }

        public CheckConstraintException(string message, string tableName, string columnName, bool isNullConstraint)
            : this(message, tableName, columnName, isNullConstraint, null, null)
        {
        }

        public CheckConstraintException(string message, string tableName, string columnName, bool isNullConstraint, string propertyName)
            : this(message, tableName, columnName, isNullConstraint, propertyName, null) 
        {
        }
                
        public CheckConstraintException(string message, string tableName, string columnName, bool isNullConstraint, string propertyName, Exception inner)
            : base(message, inner) 
        {
            TableName = tableName;
            ColumnName = columnName;
            IsNullConstraint = isNullConstraint;
            PropertyName = propertyName;
        }

        protected CheckConstraintException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

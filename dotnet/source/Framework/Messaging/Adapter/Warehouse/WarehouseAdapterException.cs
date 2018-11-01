using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Adapter.Warehouse
{
    /// <summary>
    /// The exception that is thrown when the <see cref="WarehouseAdapter"/> receives an alarm from the WMS.
    /// </summary>
    [global::System.Serializable]
    public class WarehouseAdapterException : AdapterException
    {
        private string alarmId;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="WarehouseAdapterException"/> class.</para>
        /// </summary>
        public WarehouseAdapterException() { }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="WarehouseAdapterException"/> class.</para>
        /// </summary>
        /// <param name="alarmId">
        /// </param>
        /// <param name="message">
        /// </param>
        public WarehouseAdapterException(string alarmId, string message) : base(message) 
        {
            this.alarmId = alarmId;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="WarehouseAdapterException"/> class.</para>
        /// </summary>
        /// <param name="alarmId">
        /// The alarm Id returned from the WMS.
        /// </param>
        /// <param name="message">
        /// </param>
        /// <param name="inner">
        /// </param>
        public WarehouseAdapterException(string alarmId, string message, Exception inner) : base(message, inner)
        {
            this.alarmId = alarmId;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="WarehouseAdapterException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected WarehouseAdapterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Returns the alarmd Id.
        /// </summary>
        public string AlarmId
        {
            get
            {
                return alarmId;
            }
            set
            {
                alarmId = value;
            }
        }
    }


}

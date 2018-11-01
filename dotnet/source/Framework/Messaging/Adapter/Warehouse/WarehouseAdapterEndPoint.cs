using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Data;

namespace Imi.Framework.Messaging.Adapter.Warehouse
{
    /// <summary>
    /// Represents a communication endpoint to IMI Warehouse.
    /// </summary>
    public class WarehouseAdapterEndPoint : AdapterEndPoint
    {
        private IDbConnection connection;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="WarehouseAdapterEndPoint"/> class.</para>
        /// </summary>
        /// <param name="adapter">
        /// The adapter owning the endpoint.
        /// </param>
        /// <param name="instanceId">
        /// The Warehouse instance Id string.
        /// </param>
        /// <param name="connection">
        /// The underlying database connection object.
        /// </param>
        public WarehouseAdapterEndPoint(AdapterBase adapter, string instanceId, IDbConnection connection) 
            : base(adapter, new UriBuilder("warehouse", instanceId, 0, connection.GetHashCode().ToString()).Uri)
                    
        {
            this.connection = connection;
        }
        
        /// <summary>
        /// Returns the underlying database connection object.
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return connection;
            }
        }
                        
    }
}

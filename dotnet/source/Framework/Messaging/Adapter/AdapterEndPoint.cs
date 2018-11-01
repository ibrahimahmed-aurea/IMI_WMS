using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Adapter
{
    /// <summary>
    /// Represents a communication endpoint.
    /// </summary>
    public class AdapterEndPoint
    {
        private AdapterBase adapter;
        private readonly Uri uri;


        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterEndPoint"/> class.</para>
        /// </summary>
        /// <param name="adapter">
        /// The Adapter which owns this endpoint.
        /// </param>
        /// <param name="uri">
        /// The URI respresented by this endpoint.
        /// </param>
        public AdapterEndPoint(AdapterBase adapter, Uri uri)
        {
            this.adapter = adapter;
            this.uri = uri;
        }

        /// <summary>
        /// Returns the Adapter which owns this endpoint.
        /// </summary>
        public AdapterBase Adapter
        {
            get
            {
                return adapter;
            }
        }

        /// <summary>
        /// Returns the Uri representing this endpoint.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return uri;
            }    
        }

        /// <summary>Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.</summary>
        /// <returns>A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return uri.ToString();
        }

    }
}

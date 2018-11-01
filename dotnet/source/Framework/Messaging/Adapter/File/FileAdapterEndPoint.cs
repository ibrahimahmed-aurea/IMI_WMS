using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.MWFramework.Adapter.File
{
    public class FileAdapterEndPoint : BaseAdapterEndPoint
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="FileAdapterEndPoint"/> class.</para>
        /// </summary>
        /// <param name="adapter">
        /// The adapter which owns the endpoint.
        /// </param>
        /// <param name="filename">
        /// The file which the endpoint represents.
        /// </param>
        public FileAdapterEndPoint(BaseAdapter adapter, string filename)
            : base(adapter, new Uri(filename))
        { 
        
        }
                
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Voice.Vocollect
{
    /// <summary>
    /// Represents a part of a message received from the voice terminal.
    /// </summary>
    public class VocollectMessagePart : MessagePart
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectMessagePart"/> class.</para>
        /// </summary>
        public VocollectMessagePart() : base(new VocollectPropertyCollection())
        {
                        
        }
        
    }
}

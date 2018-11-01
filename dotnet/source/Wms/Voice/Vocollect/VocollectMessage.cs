using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Voice.Vocollect
{
    /// <summary>
    /// Represents a message received from the voice terminal.
    /// </summary>
    public class VocollectMessage : MultiPartMessage
    {

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectMessage"/> class.</para>
        /// </summary>
        public VocollectMessage()
            : base("http://www.im.se/wms/voice/vocollect/voicedirect", null, new VocollectPropertyCollection())
        {
            
        }
                
    }
}

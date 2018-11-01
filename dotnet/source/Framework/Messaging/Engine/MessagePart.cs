using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Represents a message part.
    /// </summary>
    public class MessagePart
    {
        private Collection<MessagePart> messagePartCollection;
        private PropertyCollection propertyCollection;
        private Stream dataStream;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessagePart"/> class.</para>
        /// </summary>
        public MessagePart()
        {
            messagePartCollection = new Collection<MessagePart>();
            propertyCollection = new PropertyCollection();
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessagePart"/> class.</para>
        /// </summary>
        /// <param name="propertyCollection">
        /// The <see cref="PropertyCollection"/> for storing of message properties.
        /// </param>
        protected MessagePart(PropertyCollection propertyCollection)
        {
            this.propertyCollection = propertyCollection;
        }

        /// <summary>
        /// Returns the data stream of the message.
        /// </summary>
        public virtual Stream Data
        {
            get
            {
                return dataStream;
            }
            set
            {
                dataStream = value;
            }
        }

        /// <summary>
        /// Returns a collection of message properties.
        /// </summary>
        public virtual PropertyCollection Properties
        {
            get
            {
                return propertyCollection;
            }
        }

        /// <summary>
        /// Returns a collection of message parts.
        /// </summary>
        public Collection<MessagePart> Parts
        {
            get
            {
                return messagePartCollection;
            }
        }

    }
}

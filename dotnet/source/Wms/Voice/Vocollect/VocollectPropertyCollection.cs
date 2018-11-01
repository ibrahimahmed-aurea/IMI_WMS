using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Voice.Vocollect
{
    /// <summary>
    /// Represents sequential list of message properties in a <see cref="VocollectMessage"/>.
    /// </summary>
    public class VocollectPropertyCollection : PropertyCollection
    {
        private List<string> propertyCollection;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectPropertyCollection"/> class.</para>
        /// </summary>
        /// <param name="immutable">
        /// True if the properties should be immutable (read only).
        /// </param>
        public VocollectPropertyCollection(bool immutable)
            : base(immutable)
        {
            propertyCollection = new List<string>();
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectPropertyCollection"/> class.</para>
        /// </summary>
        public VocollectPropertyCollection() 
            : this(false)
        {

        }

        /// <summary>
        /// Writes a property to the property collection.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        public override void Write(string name, object value)
        {
            base.Write(name, value);

            if (value != null)
            {
                if (!propertyCollection.Contains(name))
                    propertyCollection.Add(name);
            }
            else
                propertyCollection.Remove(name);
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        /// <filterpriority>2</filterpriority>
        public override System.Collections.IEnumerator GetEnumerator()
        {
            return propertyCollection.GetEnumerator();
        }
    }
}

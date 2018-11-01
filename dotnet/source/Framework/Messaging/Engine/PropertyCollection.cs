using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace Imi.Framework.Messaging.Engine
{

    /// <summary>
    /// Container class for message properties.
    /// </summary>
    public class PropertyCollection
    {
        private Dictionary<string, object> propertyDictionary = new Dictionary<string, object>();
        private bool immutable = true;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PropertyCollection"/> class.</para>
        /// </summary>
        /// <param name="immutable">
        /// True if the properties should be immutable (read only).
        /// </param>
        public PropertyCollection(bool immutable)
            : base()
        {
            this.immutable = immutable;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PropertyCollection"/> class.</para>
        /// </summary>
        public PropertyCollection()
            : this(false)
        {
        }

        /// <summary>
        /// Returns the value of the specified property.
        /// </summary>
        /// <param name="name">The name of property.</param>
        /// <returns>The value of the property.</returns>
        public object this[string name]
        {
            get
            {
                return Read(name);
            }
            set
            {
                Write(name, value);
            }
        }

        /// <summary>
        /// Writes a property to the collection.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        public virtual void Write(string name, object value)
        {
            lock (propertyDictionary)
            {
                if (immutable)
                    throw new InvalidOperationException("The property collection is not mutable.");

                propertyDictionary[name] = value;
            }
        }

        /// <summary>
        /// Removes a property from the collection.
        /// </summary>
        /// <param name="name">The name of the property to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if key is not found in the Dictionary.</returns>
        public virtual bool Remove(string name)
        {
            lock (propertyDictionary)
            {
                if (immutable)
                    throw new InvalidOperationException("The property collection is not mutable.");
                
                return propertyDictionary.Remove(name);
            }
        }

        /// <summary>
        /// Reads the specified property as a <see cref="string"/>.
        /// </summary>
        /// <param name="name">The name of the property to read.</param>
        /// <returns>A <see cref="string"/> representation of the the property value.</returns>
        public string ReadAsString(string name)
        {
            object o = Read(name);

            if (o != null)
            {
                return Read(name).ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the specified property as a <see cref="int"/>.
        /// </summary>
        /// <param name="name">The name of the property to read.</param>
        /// <returns>The value of the property casted to an <see cref="int"/>.</returns>
        public int ReadAsInt(string name)
        {
            return Convert.ToInt32(Read(name));
        }

        /// <summary>
        /// Reads the specified property as a <see cref="double"/>.
        /// </summary>
        /// <param name="name">The name of the property to read.</param>
        /// <returns>The value of the property casted to a <see cref="double"/>.</returns>
        public short ReadAsShort(string name)
        {
            return Convert.ToInt16(Read(name));
        }

        /// <summary>
        /// Reads the specified property as a <see cref="decimal"/>.
        /// </summary>
        /// <param name="name">The name of the property to read.</param>
        /// <returns>The value of the property casted to a <see cref="decimal"/>.</returns>
        public decimal ReadAsDecimal(string name)
        {
            return Convert.ToDecimal(Read(name));
        }

        /// <summary>
        /// Reads the specified property as a <see cref="short"/>.
        /// </summary>
        /// <param name="name">The name of the property to read.</param>
        /// <returns>The value of the property casted to a <see cref="short"/>.</returns>
        public double ReadAsDouble(string name)
        {
            return Convert.ToDouble(Read(name));
        }

        /// <summary>
        /// Reads a property from the property collection.
        /// </summary>
        /// <param name="name">The name of the property to read.</param>
        /// <returns>The value of the property.</returns>
        public virtual object Read(string name)
        {
            object retVal = null;

            lock (propertyDictionary)
            {

                try
                {
                    retVal = propertyDictionary[name];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new PropertyNotFoundException(name, ex);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Checks if a property exists in the property collection.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>True if the property exists, othwerise false.</returns>
        public virtual bool Contains(string name)
        {
            return propertyDictionary.ContainsKey(name);
        }

        /// <summary>
        /// Returns the number of properties.
        /// </summary>
        /// <returns>The number of properties in the property collection.</returns>
        public virtual int Count
        {
            get
            {
                return propertyDictionary.Count;
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        /// <filterpriority>2</filterpriority>
        public virtual IEnumerator GetEnumerator()
        {
            return propertyDictionary.Keys.GetEnumerator();
        }

        /// <summary>
        /// Makes the properties in the collection immutable.
        /// </summary>
        public void Lock()
        {
            lock (propertyDictionary)
            {
                immutable = true;
            }
        }


        /// <summary>
        /// Copies the specified property to another property collection.
        /// </summary>
        /// <param name="name">The name of the property to copy.</param>
        /// <param name="target">The target property collection.</param>
        public virtual void Copy(string name, PropertyCollection target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (this.Contains(name))
                target.Write(name, this.Read(name));
        }

        /// <summary>
        /// Clears all properties in the property collection.
        /// </summary>
        public virtual void Clear()
        {
            lock (propertyDictionary)
                propertyDictionary.Clear();
        }

    }
}

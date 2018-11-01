using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Determines the session policy for the attributed subscriber.
    /// </summary>
    public enum SessionPolicy
    { 
        /// <summary>
        /// The subscriber requires a valid session object.
        /// </summary>
        Required,
        /// <summary>
        /// No session object required.
        /// </summary>
        None
    }

    /// <summary>
    /// Specifies the security requirements of the attributed subscriber.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SessionPolicyAttribute : Attribute
    {
        private SessionPolicy policy;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="SessionPolicyAttribute"/> class.</para>
        /// </summary>
        /// <param name="policy">
        /// The <see cref="SessionPolicy"/> required by the subscriber.
        /// </param>
        public SessionPolicyAttribute(SessionPolicy policy)
        {
            this.policy = policy;
        }

        /// <summary>
        /// Returns the <see cref="SessionPolicy"/> for the attributed subscriber."
        /// </summary>
        public SessionPolicy Policy
        {
            get
            {
                return policy;
            }
        }
    }
}

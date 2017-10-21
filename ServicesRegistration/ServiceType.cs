using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesRegistration
{
    /// <summary>
    /// Type loaded to register
    /// </summary>
    internal class ServiceType
    {
        /// <summary>
        /// The implementation type to register
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Service settings
        /// </summary>
        public ServiceAttribute Attribute { get; set; }
    }
}

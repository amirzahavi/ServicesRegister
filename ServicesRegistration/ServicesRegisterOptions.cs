using System.Collections.Generic;

namespace ServicesRegistration
{
    /// <summary>
    /// Configuration object for loading services
    /// </summary>
    public class ServicesRegisterOptions
    {
        /// <summary>
        /// Flag to load all available assemblies reference from startup class assembly.
        /// </summary>
        public bool LoadAllAssemblies { get; set; } = true;
        /// <summary>
        /// List of available assemblies to load that is not in the <code>IgnoreAssemblies</code> list.
        /// </summary>
        public IList<string> AvailableAssemblies { get; set; }
        /// <summary>
        /// List of assemblies to ignore.
        /// </summary>
        public IList<string> IgnoreAssemblies { get; set; }
    }
}

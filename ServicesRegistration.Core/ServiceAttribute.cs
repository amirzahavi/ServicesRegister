using System;

namespace ServicesRegistration.Core
{
    /// <summary>
    /// Mark a service to be registered into the .NET Dependency Injection mechanisem
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        /// <summary>
        /// The life time of the service
        /// </summary>
        public ServiceLifetime LifeTime { get; set; }
        /// <summary>
        /// The service type to be registered. If not assign the Class type that the attribute assigned to will be the service.
        /// </summary>
        public Type Service { get; set; }

        public ServiceAttribute()
        {
            LifeTime = ServiceLifetime.Transient;
        }

        public ServiceAttribute(ServiceLifetime lifetime, Type service = null)
        {
            LifeTime = lifetime;
            Service = service;
        }
    }
}

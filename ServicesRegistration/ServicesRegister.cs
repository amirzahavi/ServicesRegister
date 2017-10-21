using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using ServicesRegistration.Core;
using System.Reflection;

namespace ServicesRegistration
{
    /// <summary>
    /// The register engine that loads the marked services in all relevant assemblies.
    /// </summary>
    public static class ServicesRegister
    {
        /// <summary>
        /// Register all marked services with <code>ServiceAttribute</code> from all assemblies referenced in startup class project.
        /// </summary>
        /// <param name="services">services collection from the <code>ConfigureServices</code> method in Startup class</param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services) => RegisterServices(services, SetDefaultOptions);

        /// <summary>
        /// Register all marked services with <code>ServiceAttribute</code> from all available assemblies configured in <code>ServicesRegisterOptions</code> class.
        /// </summary>
        /// <param name="services">services collection from the <code>ConfigureServices</code> method in Startup class</param>
        /// <param name="options">The configuration object to set avilable and ignored assemblies</param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services, Action<ServicesRegisterOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException("action");

            var option = new ServicesRegisterOptions
            {
                AvailableAssemblies = new List<string>(),
                IgnoreAssemblies = new List<string>()
            };

            options(option);

            if (option.LoadAllAssemblies)
                SetDefaultOptions(option);

            option.AvailableAssemblies = option.AvailableAssemblies.Distinct().ToList();
            option.IgnoreAssemblies = option.IgnoreAssemblies.Distinct().ToList();

            var serviceTypes = LoadServices(option);
            RegisterServicesType(services, serviceTypes);

            return services;
        }

        /// <summary>
        /// Register all service types loaded for available assemblies.
        /// </summary>
        /// <param name="services">services collection from the <code>ConfigureServices</code> method in Startup class</param>
        /// <param name="types">All loaded types</param>
        private static void RegisterServicesType(IServiceCollection services, IList<ServiceType> types)
        {
            //register single service (inner function)
            void RegisterService(ServiceType type)
            {
                switch (type.Attribute.LifeTime)
                {
                    case Core.ServiceLifetime.Transient:

                        if (GetImplementation(type.Attribute, out Type _interface))
                            services.AddTransient(_interface, type.Type);
                        else
                            services.AddTransient(type.Type);
                        break;

                    case Core.ServiceLifetime.Scoped:

                        if (GetImplementation(type.Attribute, out _interface))
                            services.AddScoped(_interface, type.Type);
                        else
                            services.AddScoped(type.Type);
                        break;

                    case Core.ServiceLifetime.Singleton:

                        if (GetImplementation(type.Attribute, out _interface))
                            services.AddSingleton(_interface, type.Type);
                        else
                            services.AddSingleton(type.Type);
                        break;

                    default:
                        throw new InvalidOperationException($"Lifetime {type.Attribute.LifeTime} is invalid");
                }
            }
            //(inner function)
            bool GetImplementation(ServiceAttribute attr, out Type _interface)
            {
                if (attr.Service == null)
                {
                    _interface = null;
                    return false;
                }

                _interface = attr.Service;
                return true;
            }

            foreach (var type in types)
                RegisterService(type);
        }

        private static bool GetImplementation(ServiceAttribute attribute, out Type @interface)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load services from available assemblies by configuration object.
        /// </summary>
        /// <param name="options">The configuration object</param>
        /// <returns>List of all loaded services</returns>
        private static List<ServiceType> LoadServices(ServicesRegisterOptions options)
        {
            var assemblyNames = options.AvailableAssemblies.Except(options.IgnoreAssemblies);
            var serviceTypes = new List<ServiceType>();

            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);

                var types = from t in assembly.GetTypes()
                            let attr = t.GetCustomAttribute<ServiceAttribute>()
                            where attr != null
                            select new ServiceType { Type = t, Attribute = attr };

                serviceTypes.AddRange(types);
            }

            return serviceTypes;
        }

        private static void SetDefaultOptions(ServicesRegisterOptions options)
        {
            options.AvailableAssemblies.Add(Assembly.GetEntryAssembly().GetName().FullName);

            Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .ToList()
                .ForEach(assemblyName => options.AvailableAssemblies.Add(assemblyName.FullName));
        }
    }
}

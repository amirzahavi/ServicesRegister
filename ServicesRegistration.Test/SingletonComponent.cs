using ServicesRegistration.Core;
using System;

namespace ServicesRegistration.Test
{
    [Service(ServiceLifetime.Singleton,typeof(IComponent))]
    public class SingletonComponent : IComponent
    {
        public int Num { get; set; }

        public SingletonComponent()
        {
            Num = new Random().Next(1000);
        }
    }
}

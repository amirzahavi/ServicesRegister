using ServicesRegistration.Core;
using System;

namespace ServicesRegistration.Test
{
    [Service]
    public class TransientComponent
    {
        public int Num { get; set; }

        public TransientComponent()
        {
            Num = new Random().Next(1000);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HostedServiceAttribute : Attribute
    {
        public string Endpoint
        {
            get;
            private set;
        }

        public HostedServiceAttribute(string endpoint)
        {
            Endpoint = endpoint;
        }
    }
}
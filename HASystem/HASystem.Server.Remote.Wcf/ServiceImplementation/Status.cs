using HASystem.Shared.Remote.Wcf.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.ServiceImplementation
{
    [HostedService("Status")]
    public class Status : IStatus
    {
        public string GetStatus()
        {
            return "running";
        }
    }
}
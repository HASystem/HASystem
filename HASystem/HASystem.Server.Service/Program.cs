using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            if (Environment.UserInteractive)
            {
                new WindowsService().Start();
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[]
                {
                    new WindowsService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
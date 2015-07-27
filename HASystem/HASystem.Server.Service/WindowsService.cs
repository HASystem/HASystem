using HASystem.Server.Remote.Wcf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using System.Threading.Tasks;

namespace HASystem.Server.Service
{
    public partial class WindowsService : ServiceBase
    {
        private List<ServiceHost> hostedServices = new List<ServiceHost>();

        public WindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //running as a service sets the current directory to C:\windows\system32
            //so set to exe location
            Directory.SetCurrentDirectory(Path.GetDirectoryName(typeof(WindowsService).Assembly.Location));

            Manager.Instance.Start();

            ServicesSection section = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
            if (section != null)
            {
                foreach (ServiceElement element in section.Services)
                {
                    Type serviceType = typeof(Hook).Assembly.GetType(element.Name); //not nice but the best way I know to load this
                    if (serviceType == null)
                        throw new ConfigurationErrorsException(String.Format("Service-Type '{0}' not found", element.Name));
                    var host = new WebServiceHost(serviceType);
                    hostedServices.Add(host);
                    host.Open();
                }
            }
            else
            {
                throw new ConfigurationErrorsException("No services defined in configuration");
            }
        }

        protected override void OnStop()
        {
            foreach (ServiceHost host in hostedServices)
            {
                host.Close();
            }
        }

        public void Start()
        {
            OnStart(Environment.GetCommandLineArgs());
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
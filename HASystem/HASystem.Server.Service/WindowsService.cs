using HASystem.Server.Remote.Wcf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
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
            //running as a service sets the current direcotry to C:\windows\system32
            //so set to exe location
            Directory.SetCurrentDirectory(Path.GetDirectoryName(typeof(WindowsService).Assembly.Location));

            Manager.Instance.Start();

            foreach (var serviceType in typeof(Hook).Assembly.GetTypes().Where(p => p.GetCustomAttribute<ServiceContractAttribute>() != null))
            {
                var host = new WebServiceHost(serviceType);
                host.Open();
                hostedServices.Add(host);
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
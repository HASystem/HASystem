using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
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

            //TODO: detect / read possible endpoints

            //TODO: search all services
            //TODO: start all found services

            var serviceType = typeof(Status);
            WebServiceHost host = new WebServiceHost(serviceType, new Uri("http://localhost/v1/"));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IStatus), new WebHttpBinding(), "/status");
            host.Open();
            hostedServices.Add(host);
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
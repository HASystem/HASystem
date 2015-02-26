using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
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
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(typeof(WindowsService).Assembly.Location));

            //TODO: detect / read possible endpoints

            //TODO: search all services
            //TODO: start all found services

            //WebServiceHost host = new WebServiceHost(serviceType, endPoint, new Uri("/v1/"));
            //hostedServices.Add(host);
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
        }
    }
}
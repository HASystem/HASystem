using HASystem.Server.Remote.Wcf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
            //running as a service sets the current directory to C:\windows\system32
            //so set to exe location
            Directory.SetCurrentDirectory(Path.GetDirectoryName(typeof(WindowsService).Assembly.Location));

            Manager.Instance.Start();

            List<Uri> baseAddresses = new List<Uri>();
            baseAddresses.Add(new Uri("http://localhost:80/v1/"));

            foreach (Type serviceType in typeof(Hook).Assembly.GetTypes().Where(t => t.GetCustomAttribute<HostedServiceAttribute>() != null))
            {
                ServiceHost host = CreateServiceHost(serviceType, baseAddresses);
                hostedServices.Add(host);
                host.Open();
            }
        }

        private ServiceHost CreateServiceHost(Type serviceType, List<Uri> baseAddresses)
        {
            HostedServiceAttribute hostedServiceAttribute = serviceType.GetCustomAttribute<HostedServiceAttribute>();
            Uri[] specificBaseAddresses = baseAddresses.Select(u => new Uri(u, hostedServiceAttribute.Endpoint)).ToArray();

            WebServiceHost host = new WebServiceHost(serviceType, specificBaseAddresses);
            host.Description.Name = serviceType.FullName;

            foreach (Type interfaceType in serviceType.GetInterfaces().Where(i => i.GetCustomAttribute<ServiceContractAttribute>() != null))
            {
                //soap
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
                host.AddServiceEndpoint(interfaceType, basicHttpBinding, "soap");

                //rest
                WebHttpBinding webHttpBinding = new WebHttpBinding();

                //with xml
                ServiceEndpoint endpoint = host.AddServiceEndpoint(interfaceType, webHttpBinding, "xml");

                //with json
                endpoint = host.AddServiceEndpoint(interfaceType, webHttpBinding, "json");
                endpoint.EndpointBehaviors.Add(new WebHttpBehavior()
                {
                    DefaultOutgoingResponseFormat = WebMessageFormat.Json
                });

                //mex
                host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                {
                    HttpGetEnabled = true,
                    HttpsGetEnabled = true
                });
                host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                //autodetect rest
                endpoint = host.AddServiceEndpoint(interfaceType, webHttpBinding, "");
                endpoint.EndpointBehaviors.Add(new WebHttpBehavior()
                {
                    AutomaticFormatSelectionEnabled = true
                });

                //debug
                host.Description.Behaviors.Find<ServiceDebugBehavior>().HttpHelpPageEnabled = true;
                host.Description.Behaviors.Find<ServiceDebugBehavior>().HttpsHelpPageEnabled = true;
                host.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
            }

            return host;
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
using HASystem.Server.Logic.DispatcherTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class House
    {
        private List<Device> devices = new List<Device>();
        private List<LogicComponent> logicComponents = new List<LogicComponent>();
        private Dispatcher dispatcher = new Dispatcher();

        public IReadOnlyCollection<Device> Devices
        {
            get
            {
                return devices;
            }
        }

        public IReadOnlyCollection<LogicComponent> Components
        {
            get
            {
                return logicComponents;
            }
        }

        public void Start()
        {
            dispatcher.Start();
        }

        public void AddComponent(LogicComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            if (component.House == this)
                return;
            if (component.House != null)
                throw new InvalidOperationException("component is already assigned");

            component.House = this;
            lock (logicComponents)
            {
                logicComponents.Add(component);
            }

            component.Init();

            EnqueueTask(new UpdateComponentTask(component));
        }

        public void RemoveComponent(LogicComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (component.House != this)
                throw new InvalidOperationException("component is not assigned to this");

            component.House = null;

            lock (logicComponents)
            {
                logicComponents.Remove(component);
            }

            component.RemoveOutputConnections();
            component.RemoveInputConnections();

            dispatcher.RemoveTasksForComponent(component);
        }

        public void EnqueueTask(IDispatcherTask task)
        {
            dispatcher.Enqueue(task);
        }

        public void AddDevice(Device device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            lock (devices)
            {
                if (devices.Where(p => p.MACAddress == device.MACAddress).FirstOrDefault() != null)
                {
                    throw new ArgumentException("device was already added");
                }
                devices.Add(device);
            }
        }

        public void RemoveDevice(PhysicalAddress macAddress)
        {
            lock (devices)
            {
                Device device = devices.Where(p => Object.Equals(p.MACAddress, macAddress)).FirstOrDefault();
                devices.Remove(device);
            }
        }
    }
}
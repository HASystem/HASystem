using System;
using System.Collections.Generic;
using System.Linq;
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
            logicComponents.Add(component);

            component.Init();

            EnqueueForUpdate(component);
        }

        public void RemoveComponent(LogicComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (component.House != this)
                throw new InvalidOperationException("component is not assigned to this");

            component.House = null;
            logicComponents.Remove(component);

            component.RemoveOutputConnections();
            component.RemoveInputConnections();

            dispatcher.RemoveComponent(component);
        }

        public void EnqueueForUpdate(LogicComponent component)
        {
            dispatcher.Enqueue(component);
        }
    }
}
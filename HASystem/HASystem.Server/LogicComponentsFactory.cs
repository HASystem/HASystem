using HASystem.Server.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Server
{
    internal class LogicComponentsFactory
    {
        private Dictionary<Guid, Type> componentTypes = new Dictionary<Guid, Type>();

        public LogicComponent CreateComponent(Guid guid)
        {
            if (guid == null)
                throw new ArgumentNullException("guid");

            Type type = null;
            if (componentTypes.TryGetValue(guid, out type))
            {
                return (LogicComponent)Activator.CreateInstance(type, true);
            }
            else
            {
                throw new ArgumentException(String.Format("no type for guid {0}", guid));
            }
        }

        public void Init()
        {
            foreach (Type componentType in typeof(LogicComponentsFactory).Assembly.GetTypes().Where(p => p.IsAssignableFrom(typeof(LogicComponent))))
            {
                ComponentAttribute att = componentType.GetCustomAttribute<ComponentAttribute>();
                if (att == null)
                {
                    continue;
                }

                if (componentTypes.ContainsKey(att.Guid))
                {
                    throw new InvalidOperationException(String.Format("multiple components with guid '{0}' found", att.Guid));
                }
                componentTypes.Add(att.Guid, componentType);
            }
        }

        public IEnumerable<Guid> Components
        {
            get
            {
                return componentTypes.Keys;
            }
        }
    }
}
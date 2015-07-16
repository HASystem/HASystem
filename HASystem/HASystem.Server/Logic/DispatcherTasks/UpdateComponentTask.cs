using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.DispatcherTasks
{
    public class UpdateComponentTask : IDispatcherTask
    {
        public LogicComponent ConcerningComponent
        {
            get;
            private set;
        }

        public void Execute()
        {
            ConcerningComponent.Update();
        }

        public UpdateComponentTask(LogicComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            ConcerningComponent = component;
        }
    }
}
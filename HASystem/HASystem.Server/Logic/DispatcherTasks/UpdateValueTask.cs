using HASystem.Shared.ValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.DispatcherTasks
{
    public class UpdateValueTask : IDispatcherTask
    {
        public LogicInput To
        {
            get;
            private set;
        }

        public Value Value
        {
            get;
            private set;
        }

        public LogicComponent ConcerningComponent
        {
            get { return To.Component; }
        }

        public UpdateValueTask(LogicInput to, Value value)
        {
            if (to == null)
                throw new ArgumentNullException("to");

            To = to;
            Value = value;
        }

        public void Execute()
        {
            To.Value = Value;
            To.Component.Update();
        }
    }
}
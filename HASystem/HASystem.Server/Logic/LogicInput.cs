using HASystem.Shared.ValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class LogicInput
    {
        public int Index
        {
            get;
            private set;
        }

        public LogicComponent Component
        {
            get;
            private set;
        }

        public Type InputType
        {
            get;
            private set;
        }

        internal LogicOutput ConnectedBy
        {
            get;
            set;
        }

        public Value Value
        {
            get;
            internal set;
        }

        public LogicInput(LogicComponent component, int index, Type inputType)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (inputType == null)
                throw new ArgumentNullException("inputType");
            if (!typeof(Value).IsAssignableFrom(inputType))
                throw new ArgumentException("InputType has to inherit from Value", "inputType");

            Component = component;
            Index = index;
            InputType = inputType;
        }
    }
}
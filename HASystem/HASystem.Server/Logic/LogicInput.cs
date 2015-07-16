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
            get
            {
                if (ConnectedBy == null)
                {
                    return null;
                }
                return ConnectedBy.Value;
            }
        }

        public LogicInput(LogicComponent component, int index, Type inputType)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (inputType == null)
                throw new ArgumentNullException("inputType");

            Component = component;
            Index = index;
            InputType = inputType;
        }
    }
}
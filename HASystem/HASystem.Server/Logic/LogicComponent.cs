using HASystem.Server.Physical.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public abstract class LogicComponent
    {
        private static readonly LogicInput[] emptyInputs = new LogicInput[0];
        private static readonly LogicOutput[] emptyOutputs = new LogicOutput[0];

        private LogicInput[] inputs = emptyInputs;
        private LogicOutput[] outputs = emptyOutputs;
        private ComponentConfig config = null;

        public int Id
        {
            get;
            private set;
        }

        public abstract Guid ComponentType
        {
            get;
        }

        public DateTime LastModified
        {
            get;
            private set;
        }

        public House House
        {
            get;
            internal set;
        }

        //TODO: unsure if we can solve this with this solution
        public PhysicalComponent MappedComponent
        {
            get;
            set;
        }

        public void SetDirty()
        {
            LastModified = DateTime.UtcNow;

            House.EnqueueForUpdate(this);
        }

        public LogicInput[] Inputs
        {
            get { return inputs; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                RemoveInputConnections();

                inputs = value;
                SetDirty();
            }
        }

        public LogicOutput[] Outputs
        {
            get { return outputs; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                RemoveOutputConnections();

                outputs = value;
                SetDirty();
            }
        }

        public ComponentConfig Config
        {
            get
            {
                if (config == null)
                {
                    lock (this)
                    {
                        if (config == null)
                        {
                            config = new ComponentConfig();
                        }
                    }
                }
                return config;
            }
            internal set
            {
                if (Object.Equals(config, value))
                    return;
                ComponentConfig old = this.config;
                config = value;
                OnComponenentConfigChanged(old, config);
                SetDirty();
            }
        }

        public abstract void Update();

        public abstract void Init();

        protected virtual void OnComponenentConfigChanged(ComponentConfig oldValue, ComponentConfig newValue)
        {
        }

        internal void RemoveOutputConnections()
        {
            foreach (LogicOutput output in Outputs)
            {
                output.RemoveConnections();
            }
        }

        internal void RemoveInputConnections()
        {
            foreach (LogicInput input in Inputs)
            {
                LogicOutput output = input.ConnectedBy;
                if (output != null)
                {
                    output.RemoveConnection(input);
                }
            }
        }
    }
}
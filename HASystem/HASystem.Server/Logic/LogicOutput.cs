using HASystem.Server.Logic.DispatcherTasks;
using HASystem.Shared.ValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class LogicOutput
    {
        private Value value;
        private List<LogicInput> connections = new List<LogicInput>();

        public int Index
        {
            get;
            private set;
        }

        public Value Value
        {
            get { return value; }
            set
            {
                if (Object.Equals(this.value, value))
                    return;
                this.value = value;

                SendValueToConnections();

                SetDirty();
            }
        }

        public LogicComponent Component
        {
            get;
            private set;
        }

        public Type OutputType
        {
            get;
            private set;
        }

        public IReadOnlyCollection<LogicInput> Connections
        {
            get { return connections; }
        }

        public DateTime LastModified
        {
            get;
            private set;
        }

        public LogicOutput(LogicComponent component, int index, Type outputType)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (outputType == null)
                throw new ArgumentNullException("outputType");
            if (!typeof(Value).IsAssignableFrom(outputType))
                throw new ArgumentException("OutputType has to inherit from Value", "outputType");

            Component = component;
            Index = index;
            OutputType = outputType;
        }

        public void AddConnection(LogicInput input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            lock (input)
            {
                if (input.ConnectedBy == this)
                    return;

                if (input.ConnectedBy != null)
                    throw new ArgumentException("input is already connected");

                //we need to check if type is compatible
                if (!input.InputType.IsAssignableFrom(OutputType))
                    throw new ArgumentException(String.Format("port-type mismatch: can't connect input-type '{0}' to output-type '{1}'", input.InputType, OutputType));

                input.ConnectedBy = this;
                lock (connections)
                {
                    connections.Add(input);
                }
            }
            SendValueToConnection(input, Value);
        }

        public bool RemoveConnection(LogicInput input)
        {
            lock (input)
            {
                if (input.ConnectedBy != this)
                {
                    return false;
                }

                lock (connections)
                {
                    input.ConnectedBy = null;
                    SendValueToConnection(input, null);
                    return connections.Remove(input);
                }
            }
        }

        public void RemoveConnections()
        {
            lock (connections)
            {
                foreach (LogicInput connection in connections)
                {
                    connection.ConnectedBy = null;
                    connections.Remove(connection);
                    SendValueToConnection(connection, Value);
                }
            }
        }

        protected void SetDirty()
        {
            LastModified = DateTime.UtcNow;
        }

        private void SendValueToConnections()
        {
            SendValueToConnections(Value);
        }

        private void SendValueToConnections(Value value)
        {
            lock (connections)
            {
                foreach (LogicInput connection in connections)
                {
                    SendValueToConnection(connection, value);
                }
            }
        }

        private void SendValueToConnection(LogicInput connection, Value value)
        {
            Component.House.EnqueueTask(new UpdateValueTask(connection, value));
        }
    }
}
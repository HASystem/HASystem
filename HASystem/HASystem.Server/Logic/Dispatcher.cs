using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class Dispatcher
    {
        private LinkedList<LogicComponent> openComponents = new LinkedList<LogicComponent>();
        private Thread processingThread = null;
        private System.Threading.ManualResetEventSlim threadInterrupt = new ManualResetEventSlim(false);

        public DateTime LastDispatch
        {
            get;
            protected set;
        }

        public void Enqueue(LogicComponent component)
        {
            lock (openComponents)
            {
                if (openComponents.Contains(component))
                    return;

                openComponents.AddLast(component);
                threadInterrupt.Set();
            }
        }

        public void RemoveComponent(LogicComponent component)
        {
            lock (openComponents)
            {
                openComponents.Remove(component);
            }
        }

        public void Start()
        {
            if (processingThread != null)
            {
                throw new InvalidOperationException("already started");
            }
            processingThread = new Thread(WorkThread);
            processingThread.Start();
        }

        public void Stop()
        {
            processingThread.Abort();
        }

        private void WorkThread()
        {
            while (true)
            {
                try
                {
                    if (openComponents.First == null)
                    {
                        threadInterrupt.Wait(500);
                    }
                    else
                    {
                        LogicComponent component = null;
                        lock (openComponents)
                        {
                            component = openComponents.First.Value;
                            openComponents.RemoveFirst();

                            if (openComponents.First == null)
                            {
                                //this was the last component
                                threadInterrupt.Reset();
                            }
                        }
                        DateTime lastModified = component.LastModified;
                        component.Update();
                        LastDispatch = lastModified;
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    //TODO: we need to log this exception!!
                    System.Diagnostics.Trace.Fail("critical exception in dispatcher!!", ex.ToString());
                }
            }
        }
    }
}
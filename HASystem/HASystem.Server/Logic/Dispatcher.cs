using HASystem.Server.Logic.DispatcherTasks;
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
        private LinkedList<IDispatcherTask> taskQueue = new LinkedList<IDispatcherTask>();
        private Thread processingThread = null;
        private System.Threading.ManualResetEventSlim threadInterrupt = new ManualResetEventSlim(false);

        public void Enqueue(IDispatcherTask task)
        {
            lock (taskQueue)
            {
                taskQueue.AddLast(task);
                threadInterrupt.Set();
            }
        }

        public void RemoveTasksForComponent(LogicComponent component)
        {
            lock (taskQueue)
            {
                LinkedListNode<IDispatcherTask> current = taskQueue.First;
                while (current != null)
                {
                    current = current.Next;
                    if (current.Previous.Value.ConcerningComponent == component)
                    {
                        taskQueue.Remove(current.Previous);
                    }
                }
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
                    if (taskQueue.First == null)
                    {
                        threadInterrupt.Wait(5000);
                    }
                    else
                    {
                        IDispatcherTask task = null;
                        lock (taskQueue)
                        {
                            task = taskQueue.First.Value;
                            taskQueue.RemoveFirst();

                            if (taskQueue.First == null)
                            {
                                //this was the last component
                                threadInterrupt.Reset();
                            }
                        }
                        task.Execute();
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
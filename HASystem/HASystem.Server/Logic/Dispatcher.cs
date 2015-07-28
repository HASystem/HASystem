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
        //TODO: some kind of priority queue?
        private LinkedList<IDispatcherTask> taskQueue = new LinkedList<IDispatcherTask>();

        private Thread processingThread = null;
        private bool running = false;
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
                    if (current.Value.ConcerningComponent == component)
                    {
                        LinkedListNode<IDispatcherTask> next = current.Next;
                        taskQueue.Remove(current);
                        current = next;
                    }
                    else
                    {
                        current = current.Next;
                    }
                }
            }
        }

        public void Start()
        {
            if (processingThread != null || running)
            {
                throw new InvalidOperationException("already started");
            }
            running = true;
            processingThread = new Thread(WorkThread);
            processingThread.Start();
        }

        public void Stop()
        {
            running = false;

            //we give the thread a timeout of 100 * 10 = 1000ms to terminate
            for (int i = 0; i < 100; i++)
            {
                threadInterrupt.Set();
                processingThread.Join(10);
                if (processingThread.ThreadState == ThreadState.Stopped)
                {
                    return; //thread was able to stop by itself
                }
            }

            //we need to kill him
            processingThread.Abort();
        }

        private void WorkThread()
        {
            while (running)
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
                            if (taskQueue.First == null)
                                continue;

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
                    //TODO: if a component fails to often we can also concern to disable it?
                    System.Diagnostics.Trace.Fail("critical exception in dispatcher!!", ex.ToString());
                }
            }
        }
    }
}
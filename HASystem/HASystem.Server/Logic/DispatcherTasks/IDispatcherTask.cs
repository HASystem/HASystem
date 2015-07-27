using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.DispatcherTasks
{
    public interface IDispatcherTask
    {
        LogicComponent ConcerningComponent
        {
            get;
        }

        void Execute();
    }
}
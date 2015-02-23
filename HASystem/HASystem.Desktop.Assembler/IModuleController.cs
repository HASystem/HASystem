using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Assembler
{
    public interface IModuleController
    {
        CompositionContainer CompositionContainer { get; set; }
        void Run();
    }
}

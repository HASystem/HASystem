using HASystem.Desktop.Application.Controllers;
using HASystem.Desktop.Application.Views;
using HASystem.Desktop.Application.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Assembler
{
    [Export(typeof(IModuleController)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleController : IModuleController
    {
        public CompositionContainer CompositionContainer { get; set; }
        public IWindow Window { get; private set; }

        public MainController Controller { get; private set; }


        [ImportingConstructor]
        public ModuleController(IWindow window)
        {
            Window = window;
            //Controller = controller;
        }

        public void Run()
        {
            Window.Show();
        }
    }
}

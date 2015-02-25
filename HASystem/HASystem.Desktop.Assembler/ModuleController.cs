using HASystem.Desktop.Application.Controllers;
using HASystem.Desktop.Application.Views;
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
        #region properties
        public CompositionContainer CompositionContainer { get; set; }
        public StartWindow Window { get; private set; }

        public MainController Controller { get; private set; }
        #endregion

        #region ctor
        [ImportingConstructor]
        public ModuleController(StartWindow window, MainController controller)
        {
            Window = window;
            Controller = controller;
        }
        #endregion

        #region methods
        public void Run()
        {
            Window.Show();
        }
        #endregion
    }
}

using HASystem.Desktop.Application.Controllers;
using HASystem.Desktop.Application.Views;
using HASystem.Desktop.Utilities;
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
    public class ModuleController : NotifyPropertyChangedBase, IModuleController
    {
        #region fields
        private StartWindow window;
        private MainController controller;
        #endregion

        #region properties
        public CompositionContainer CompositionContainer { get; set; }
        public StartWindow Window 
        {
            get { return window; }
            private set
            {
                window = value;
                OnPropertyChanged();
            }
        }

        public MainController Controller 
        {
            get { return controller; }
            private set
            {
                controller = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ctor
        [ImportingConstructor]
        public ModuleController(StartWindow window, MainController controller)
        {
            Window = window;
            Window.DataContext = this;

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

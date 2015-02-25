using HASystem.Desktop.Application.PresentationModels;
using HASystem.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Application.Controllers
{
    [Export(typeof(MainController)), PartCreationPolicy(CreationPolicy.Shared)]
    public class MainController : NotifyPropertyChangedBase
    {
        #region fields
        private MainPresentationModel presentationModel;
        #endregion

        #region properties
        public MainPresentationModel PresentationModel 
        { 
            get { return presentationModel; }
            private set
            {
                presentationModel = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ctor
        [ImportingConstructor]
        public MainController(MainPresentationModel presentationModel)
        {
            PresentationModel = presentationModel;
        }
        #endregion
    }
}

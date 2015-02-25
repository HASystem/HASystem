using HASystem.Desktop.Application.PresentationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Application.Controllers
{
    [Export(typeof(MainController)), PartCreationPolicy(CreationPolicy.Shared)]
    public class MainController
    {
        #region properties
        public MainPresentationModel PresentationModel { get; private set; }
        #endregion

        #region ctor
        [ImportingConstructor]
        public MainController()
        {
            //PresentationModel = presentationModel;
        }
        #endregion
    }
}

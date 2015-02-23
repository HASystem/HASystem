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
        public MainPresentationModel PresentationModel { get; set; }

        [ImportingConstructor]
        public MainController(MainPresentationModel presentationModel)
        {
            PresentationModel = presentationModel;
        }
    }
}

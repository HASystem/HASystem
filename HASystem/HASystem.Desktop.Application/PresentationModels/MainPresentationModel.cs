using HASystem.Desktop.Application.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Application.PresentationModels
{
    [Export(typeof(MainPresentationModel)), PartCreationPolicy(CreationPolicy.Shared)]
    public class MainPresentationModel
    {
        public IMainView View { get; set; }

        [ImportingConstructor]
        public MainPresentationModel(IMainView mainView)
        {
            View = mainView;
        }
    }
}

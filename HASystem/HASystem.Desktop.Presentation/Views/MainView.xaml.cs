using HASystem.Desktop.Application.Views;
using System.ComponentModel.Composition;

namespace HASystem.Desktop.Presentation.Views
{
    [Export(typeof(IMainView)), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainView : IMainView
    {
        #region ctor
        public MainView()
        {
            InitializeComponent();
        }
        #endregion
    }
}

using HASystem.Desktop.Application.Views;
using HASystem.Desktop.Presentation.Utils;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace HASystem.Desktop.Presentation.Views
{
    [Export(typeof(IMainView)), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainView : IMainView
    {
        #region fields
        private DrawingHelper helper;
        #endregion

        #region ctor
        public MainView()
        {
            InitializeComponent();
            helper = new DrawingHelper();
        }
        #endregion

        #region methods
        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var control = new Gate();
            var position = e.MouseDevice.GetPosition(designer);

            Canvas.SetTop(control, position.Y);
            Canvas.SetLeft(control, position.X);

            if (!helper.CollisionExists(control))
            {
                helper.AddControl(control);
                designer.Children.Add(control);
            }
        }
        #endregion


    }
}

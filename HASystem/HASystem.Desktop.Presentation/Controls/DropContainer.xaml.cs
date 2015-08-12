using HASystem.Desktop.Presentation.Utils;
using System.Windows.Controls;

namespace HASystem.Desktop.Presentation.Controls
{
    /// <summary>
    /// Interaktionslogik für DropContainer.xaml
    /// </summary>
    public partial class DropContainer : UserControl
    {
        #region fields
        private DrawingHelper helper;
        #endregion

        public DropContainer()
        {
            InitializeComponent();
            helper = new DrawingHelper();
        }

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

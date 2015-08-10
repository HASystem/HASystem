using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HASystem.Desktop.Presentation.Utils
{
    public class DrawingHelper
    {
        #region properties
        private List<UserControl> Controls { get; set; }
        #endregion

        #region ctor
        public DrawingHelper()
        {
            Controls = new List<UserControl>();
        }
        #endregion

        #region methods
        public bool CollisionExists(FrameworkElement element)
        {
            var rectangle1 = new Rect(Canvas.GetLeft(element), Canvas.GetTop(element), element.ActualWidth, element.ActualHeight);

            return (Controls.Any(c => new Rect(Canvas.GetLeft(c), Canvas.GetTop(c), c.ActualWidth, c.ActualHeight).IntersectsWith(rectangle1)));
        }

        public void AddControl(UserControl control)
        {
            Controls.Add(control);
        }
        #endregion
    }
}

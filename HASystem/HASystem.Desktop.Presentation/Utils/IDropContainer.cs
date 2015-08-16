using System.Windows;

namespace HASystem.Desktop.Presentation.Utils
{
    public interface IDropContainer
    {
        void OnContainerDragEnter(DragEventArgs e);
        void OnContainerDragLeave(DragEventArgs e);
    }
}
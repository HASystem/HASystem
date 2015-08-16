using System.Collections.Generic;
using System.Timers;
using System.Windows;

namespace HASystem.Desktop.Presentation.Utils
{
    public static class DragDropHelper
    {
        class DragContainerInfo
        {
            public Timer DragTimer;
            public bool IsDragging;
            public bool IsDraggingInside;
            public DragEventArgs DragLeaveEventArgs;
        }

        static Dictionary<IDropContainer, DragContainerInfo> m_dragContainerInfo
            = new Dictionary<IDropContainer, DragContainerInfo>();

        // quick lookup the associated drop container of a drop timer
        static Dictionary<Timer, IDropContainer> m_timerToContainer
             = new Dictionary<Timer, IDropContainer>();

        public static void HandleDropContainer<T>(T dropContainer)
            where T : DependencyObject, IDropContainer
        {
            if (!m_dragContainerInfo.ContainsKey(dropContainer))
            {
                DragContainerInfo info = new DragContainerInfo();

                Timer dragTimer = new Timer();
                dragTimer.Interval = 1;
                dragTimer.Elapsed += new ElapsedEventHandler(OnDragTimerElapsed);

                info.DragTimer = dragTimer;

                m_timerToContainer[dragTimer] = dropContainer;

                m_dragContainerInfo.Add(dropContainer, info);

                DragDrop.AddDragEnterHandler(dropContainer, OnContainerDragEnter);
                DragDrop.AddDragLeaveHandler(dropContainer, OnContainerDragLeave);
            }
        }

        static void OnDragTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer dragTimer = sender as Timer;
            IDropContainer container = m_timerToContainer[dragTimer];
            DragContainerInfo info = m_dragContainerInfo[container];

            if (!info.IsDragging)
            {
                info.IsDraggingInside = false;
                container.OnContainerDragLeave(info.DragLeaveEventArgs);
            }

            dragTimer.Stop();
        }

        static void OnContainerDragEnter(object sender, DragEventArgs e)
        {
            IDropContainer container = sender as IDropContainer;
            DragContainerInfo info = m_dragContainerInfo[container];

            if (info.IsDraggingInside == false)
            {
                info.IsDraggingInside = true;
                container.OnContainerDragEnter(e);
            }

            info.IsDragging = true;
            info.DragTimer.Stop();
        }

        static void OnContainerDragLeave(object sender, DragEventArgs e)
        {
            IDropContainer container = sender as IDropContainer;
            DragContainerInfo info = m_dragContainerInfo[container];

            info.IsDragging = false;
            info.DragLeaveEventArgs = e;
            info.DragTimer.Start();
        }
    }
}

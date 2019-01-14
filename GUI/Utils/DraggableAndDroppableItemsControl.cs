using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Utils
{
    public class DraggableAndDroppableItemsControl: ListView
    {
        private Point _startPoint;

        public static readonly DependencyProperty DropFormatProperty;
        public static readonly DependencyProperty DragFormatProperty;

        static DraggableAndDroppableItemsControl()
        {

            DropFormatProperty = DependencyProperty.Register("DropFormat", typeof(string), typeof(DraggableAndDroppableItemsControl), new UIPropertyMetadata(null));
            DragFormatProperty = DependencyProperty.Register("DragFormat", typeof(string), typeof(DraggableAndDroppableItemsControl), new UIPropertyMetadata(null));
        }

        public DraggableAndDroppableItemsControl()
        {
            AllowDrop = true;
        }
        public string DropFormat
        {
            get
            {
                return (string)GetValue(DropFormatProperty);
            }
            set
            {
                SetValue(DropFormatProperty, value);
            }
        }

        public string DragFormat
        {
            get
            {
                return (string)GetValue(DragFormatProperty);
            }
            set
            {
                SetValue(DragFormatProperty, value);
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                {
                    base.OnPreviewMouseMove(e);
                    return;
                }
                // Find the data behind the ListViewItem
                object draggingObject = ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject(DragFormat, draggingObject);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
            base.OnPreviewMouseMove(e);
        }

        private static T FindAncestor<T>(DependencyObject current)
    where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
        public static readonly RoutedEvent DropItemEvent = EventManager.RegisterRoutedEvent(
    "DropItem", RoutingStrategy.Bubble, typeof(DragEventHandler), typeof(DraggableAndDroppableItemsControl));
        

        public event DragEventHandler DropItem
        {
            add { AddHandler(DropItemEvent, value); }
            remove { RemoveHandler(DropItemEvent, value); }
        }

        void RaiseDropItemEvent(object data)
        {
            DropEventArgs newEventArgs = new DropEventArgs(DropItemEvent, data);
            RaiseEvent(newEventArgs);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DropFormat) || this == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }

            base.OnDragEnter(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DropFormat))
            {
                var transferingObject = e.Data.GetData(DropFormat);
                RaiseDropItemEvent(transferingObject);
            }

            base.OnDrop(e);
        }
    }
}

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
    public class DraggableItemsControl: ListView
    {
        private Point _startPoint;

        public static readonly DependencyProperty FormatProperty;

        static DraggableItemsControl()
        {
            FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(DraggableItemsControl), new UIPropertyMetadata(null));
        }
        public string Format
        {
            get
            {
                return (string)GetValue(FormatProperty); 
            }
            set
            {
                SetValue(FormatProperty, value);
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

                if(listViewItem == null)
                {
                    base.OnPreviewMouseMove(e);
                    return;
                }
                // Find the data behind the ListViewItem
                object draggingObject = ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject(Format, draggingObject);
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

    }
}

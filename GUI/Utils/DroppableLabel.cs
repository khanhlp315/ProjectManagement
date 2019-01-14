using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Utils
{
    public class DroppableLabel: Label
    {
        public static readonly DependencyProperty FormatProperty;
        public static readonly RoutedEvent DropItemEvent = EventManager.RegisterRoutedEvent(
    "DropItem", RoutingStrategy.Bubble, typeof(DragEventHandler), typeof(DroppableLabel));

        static DroppableLabel()
        {
            FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(DroppableLabel), new UIPropertyMetadata(null));
        }

        public DroppableLabel()
        {
            AllowDrop = true;
        }

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


        protected override void OnDragEnter(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(Format) || this == e.Source)
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
            if (e.Data.GetDataPresent(Format))
            {
                var transferingObject = e.Data.GetData(Format);
                RaiseDropItemEvent(transferingObject);
            }

            base.OnDrop(e);


        }
    }
}

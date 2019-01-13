using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.Utils
{
    public class DropEventArgs: RoutedEventArgs
    {
        private readonly object _data;
        public object Data
        {
            get
            {
                return _data;
            }
        }

        public DropEventArgs(RoutedEvent routedEvent, object data): base(routedEvent)
        {
            _data = data;
        }
    }
}

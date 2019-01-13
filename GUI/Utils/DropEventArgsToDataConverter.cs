using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GUI.Utils
{
    public class DropEventArgsToDataConverter : IValueConverter
    {
        public static DropEventArgsToDataConverter Default = new DropEventArgsToDataConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var arg = (DropEventArgs)value;
            return arg.Data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DropEventArgs(DroppableItemsControl.DropItemEvent, value) ;
        }
    }
}

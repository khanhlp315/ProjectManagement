using DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GUI.Utils
{
    [ValueConversion(typeof(SprintState), typeof(string))]

    class SprintStateToSprintTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (SprintState)value;
            if(state == SprintState.QUEUING)
            {
                return "Start Sprint";
            }
            else
            {
                return "End Sprint";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

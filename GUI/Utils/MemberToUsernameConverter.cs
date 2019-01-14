using BUS;
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
    [ValueConversion(typeof(Member), typeof(string))]
    public class MemberToUsernameConverter : IValueConverter
    {
        private static TeamBUS _bus = new TeamBUS();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return "Not yet assigned";
            }

            else
            {
                return _bus.GetUserFromMember(((Member)value).Id).Username;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

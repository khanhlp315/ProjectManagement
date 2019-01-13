using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Utils
{
    public class UserTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserTemplate
        {
            get;
            set;
        }

        public DataTemplate AddTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return AddTemplate;
            }
            else if (item is User)
            {
                return UserTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}

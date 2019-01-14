using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void CloseMenu(object sender, RoutedEventArgs e)
        {
            //ButtonCloseMenu.Visibility = Visibility.Collapsed;
            //ButtonOpenMenu.Visibility = Visibility.Visible;
            drawer.IsLeftDrawerOpen = false;
            
        }

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            //ButtonCloseMenu.Visibility = Visibility.Visible;
            //ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }
    }
}

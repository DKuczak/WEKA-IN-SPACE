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

namespace Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Filmik.Visibility = Visibility.Visible;
        }

        private void Film_MediaEnded(object sender, RoutedEventArgs e)
        {
            Film.Children.Remove(Filmik);
            Filmik.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void DoPrzodu(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Visible;
        }
        private void Cofanie(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void Wranking(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Visible;
        }
        private void Wustawienia(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Visible;
        }
        private void Render(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Width = 420;
        }
        private void Wyjscie(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

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
using System.Windows.Threading;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int Wynik = 0;
        int liczbap = 0;
        Random generator = new Random();
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        int CzasPocisku = 0;
        int LimitLotuPocisku = 90;
        bool gameOver = false;

        DispatcherTimer czasGry = new DispatcherTimer();
        Gracz pl1;
        Przeciwnicy p, pr, prz;

        public MainWindow()
        {
            InitializeComponent();
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Filmik.Visibility = Visibility.Visible;

        }
        private void Rozpocznij() {
            pl1 = new Gracz();
            pl1.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/Gracz1.png"));
            player.Fill = pl1.tekstura;
            czasGry.Tick += GameLoop;
            czasGry.Interval = TimeSpan.FromMilliseconds(10);
            czasGry.Start();

            Canvas.Focus();
            wygeneruj1();
            pr = new Przeciwnicy { limit = 10, szerokość = 45, wielkość = 60, szybkość = 10 };
            prz = new Przeciwnicy();

        }
        private void wygeneruj1()
        {
            p = new Przeciwnicy
            {
                limit = 10,
                szerokość = 75,
                wielkość = 75,
                szybkość = 6,

            };
            p.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/przeciwnik1.png"));
            for (int i = 0; i < p.limit; i++)
            {
                Canvas.Children.Add(p.StwórzPrzeciwnika("wróg"));

            }
            liczbap += p.limit;
        }
        private void GameLoop(object sender, EventArgs e)
        {
            if (liczbap == 0)
            {
                wygeneruj1();
            }

            Wynik++;
            Rect Hitboxp = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            TimerTicks.Content = "Wynik: " + Wynik;

            if (pl1.Lewo == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - 10);
            }
            if (pl1.Prawo == true && Canvas.GetLeft(player) + 80 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + 10);
            }
            if (pl1.Góra == true && Canvas.GetTop(player) > 0)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - 10);
            }
            if (pl1.Dół == true && Canvas.GetTop(player) + 110 < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + 10);
            }

            List<Rectangle> Przeciwnicy = new List<Rectangle>();
            foreach (var x in Canvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "pocisk")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect pocisk = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    foreach (var y in Canvas.Children.OfType<Rectangle>())
                    {
                        string Tag = (string)y.Tag;
                        if (y is Rectangle && Tag == "wróg")
                        {
                            Rect hitboxprz = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (pocisk.IntersectsWith(hitboxprz))
                            {
                                itemsToRemove.Add(x);
                                itemsToRemove.Add(y);
                                liczbap--;
                            }
                        }
                    }
                }

                else if (x is Rectangle && (string)x.Tag == "wróg")
                {
                    Przeciwnicy.Add(x);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + p.szybkość);
                    if (Canvas.GetLeft(x) > 1920)
                    {
                        Canvas.SetLeft(x, -80);
                        Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));
                    }
                    Rect hitboxprz = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(hitboxprz))
                    {
                        KoniecGry("Nie żyjesz! ");
                    }
                }


                else if (x is Rectangle && (string)x.Tag == "ppocisk")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10);

                    if (Canvas.GetTop(x) > 1080)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect ppocisk = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(ppocisk))
                    {
                        KoniecGry("Nie żyjesz! ");
                    }
                }
            }
            foreach (var x in Przeciwnicy)
            {
                if (generator.Next(0, 100) > 98)
                {
                    PociskPrzeciwnika(Canvas.GetLeft(x), Canvas.GetTop(x));
                }
            }
            foreach (Rectangle i in itemsToRemove)
            {
                Canvas.Children.Remove(i);
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                pl1.Lewo = true;
            }
            if (e.Key == Key.Right)
            {
                pl1.Prawo = true;
            }
            if (e.Key == Key.Up)
            {
                pl1.Góra = true;
            }
            if (e.Key == Key.Down)
            {
                pl1.Dół = true;
            }
            if (e.Key == Key.Space && !e.IsRepeat)
            {
                Rectangle Pocisk = new Rectangle
                {
                    Tag = "pocisk",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.Blue,
                    Stroke = Brushes.White
                };

                Canvas.SetTop(Pocisk, Canvas.GetTop(player) - Pocisk.Height);
                Canvas.SetLeft(Pocisk, Canvas.GetLeft(player) + player.Width / 2);

                Canvas.Children.Add(Pocisk);
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                pl1.Lewo = false;
            }
            if (e.Key == Key.Right)
            {
                pl1.Prawo = false;
            }
            if (e.Key == Key.Up)
            {
                pl1.Góra = false;
            }
            if (e.Key == Key.Down)
            {
                pl1.Dół = false;
            }
        }
        private void PociskPrzeciwnika(double x, double y)
        {
            Rectangle przeciwnikaPocisk = new Rectangle
            {
                Tag = "ppocisk",
                Height = 40,
                Width = 15,
                Fill = Brushes.Red,
                Stroke = Brushes.Yellow
            };

            Canvas.SetTop(przeciwnikaPocisk, y);
            Canvas.SetLeft(przeciwnikaPocisk, x);

            Canvas.Children.Add(przeciwnikaPocisk);
        }
        private void KoniecGry(string wiad)
        {
            gameOver = true;
            czasGry.Stop();
            uded.Content += " " + wiad + " Wciśnij enter do dalszej gry";
        }

        private void Film_MediaEnded(object sender, RoutedEventArgs e)
        {
            Film.Children.Remove(Filmik);
            Filmik.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void DoPrzodu(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Visible;
        }
        private void Cofanie(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void Wranking(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Visible;
        }
        private void Wustawienia(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Visible;
        }
        private void Gra1(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ranking.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Visible;
            Rozpocznij();
        }
        private void Wyjscie(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

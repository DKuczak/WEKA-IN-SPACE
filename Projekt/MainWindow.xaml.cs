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

        ImageBrush pocisk = new ImageBrush();
        DispatcherTimer czasGry = new DispatcherTimer();
        Gracz pl1;
        Przeciwnicy p, pr, prz;
        Pocisk pocisk1;

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
            pl1 = new Gracz { hp=100 };
            pl1.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/Gracz1.png"));
            player.Fill = pl1.tekstura;
            pocisk1 = new Pocisk();
            czasGry.Tick += GameLoop;
            czasGry.Interval = TimeSpan.FromMilliseconds(10);
            czasGry.Start();

            Canvas.Focus();
            pr = new Przeciwnicy { limit = 1, szerokość = 45, wielkość = 60, szybkość = 10 };
            prz = new Przeciwnicy();
            p = new Przeciwnicy
            {
                limit = 5,
                szerokość = 75,
                wielkość = 75,
                szybkość = 6,
                wartość = 100

            };
            wygeneruj1();

        }
        private void generator_pocisku() {
            Canvas.Children.Add(pocisk1.PociskPrzeciwnika(0,0));
        }
        private void wygeneruj1()
        {
            p.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/przeciwnik1.png"));
            for (int i = 0; i < p.limit; i++)
            {
                Canvas.Children.Add(p.StwórzPrzeciwnika("wróg"));

            }
            liczbap += p.limit;
            p.limit++;
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
                                Wynik += p.wartość;
                                
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
                        if (pl1.hp <= 0)
                            KoniecGry("Nie żyjesz! ");
                        else pl1.hp -= 10;
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
                        if (pl1.hp <= 0)
                            KoniecGry("Nie żyjesz! ");
                        else pl1.hp -= 1;
                    }
                }
                else if (x is Rectangle && (string)x.Tag == "przedmiot")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10);

                    if (Canvas.GetTop(x) > 1080)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect przedmiot = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(przedmiot))
                    {
                        pl1.hp += 2;
                    }
                }
            }
            foreach (var x in Przeciwnicy)
            {
                if (generator.Next(0, 100) > 98)
                {
                    Canvas.Children.Add(pocisk1.PociskPrzeciwnika(Canvas.GetLeft(x), Canvas.GetTop(x)));
                }
            }
            if (generator.Next(0, 100) > 99)
                                {
                                    Canvas.Children.Add(apteczka.przedmiot(Canvas.GetLeft(player), Canvas.GetTop(player) - 800));
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
                Canvas.Children.Add(pocisk2.PociskGracza(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width));
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                
                    uded.Visibility = Visibility.Hidden;
                    gameOver = false;
                    foreach (var x in Canvas.Children.OfType<Rectangle>())
                    {
                        if (x is Rectangle && (string)x.Tag == "pocisk" || x is Rectangle && (string)x.Tag == "wróg" || x is Rectangle && (string)x.Tag == "ppocisk")
                        {
                            itemsToRemove.Add(x);
                        }
                    }

                    Canvas.SetLeft(player, 915);
                    Canvas.SetTop(player, 972);
                    Wynik = 0;
                    czasGry.Tick -= GameLoop;
                    czasGry.Start();

                    Canvas.Visibility = Visibility.Collapsed;
                    Menu.Visibility = Visibility.Visible;

            }
            if (e.Key == Key.Escape && gameOver == true)
            {
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
        private void KoniecGry(string wiad)
        {
            gameOver = true;
            czasGry.Stop();
            uded.Content = " " + wiad + " Wciśnij enter do dalszej gry lub ESC aby wyjść!";
            uded.Visibility = Visibility.Visible;
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

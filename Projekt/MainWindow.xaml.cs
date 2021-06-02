using System;
using System.Collections.Generic;
using System.IO;
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
        
        
        string nazwa;
        int Wynik = 0;
        int Wynik2 = 0;
        int liczbap = 0;
        Random generator = new Random();
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        int CzasPocisku = 0;
        int LimitLotuPocisku = 90;
        bool gameOver = false;
        bool kierunek = true;
        bool multiplayer = false;

        ImageBrush pocisk = new ImageBrush();
        DispatcherTimer czasGry = new DispatcherTimer();
        Gracz pl1 = new Gracz(100);
        Gracz pl2 = new Gracz(100);      
        Przeciwnicy boss, p;
        Pocisk pocisk1,ppocisk;
        Przedmiot apteczka;

        public MainWindow()
        {
            InitializeComponent();
            
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Filmik.Visibility = Visibility.Visible;

        }
        private void Rozpocznij(bool multiplayer) {

            if (multiplayer == true)
            {
                player2.Visibility = Visibility.Visible;
                Życie2.Visibility = Visibility.Visible;
                pl1.hp = 100;
                pl2.hp = 100;
            }
            else
            {
                player2.Visibility = Visibility.Hidden;
                Życie2.Visibility = Visibility.Hidden;
                pl1.hp = 100;
            }

            ppocisk = new Pocisk(10, 10);
            pocisk1 = new Pocisk(10, 20);
            apteczka = new Przedmiot();
            ppocisk.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/strzal_przeciwnika.png"));
            pocisk1.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/strzal_gracza.png"));
            apteczka.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/apteczka.png"));

            pl1.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/Gracz1.png"));
            pl2.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/Gracz2.png"));
            player.Fill = pl1.tekstura;
            player2.Fill = pl2.tekstura;

            czasGry.Tick += GameLoop;
            czasGry.Interval = TimeSpan.FromMilliseconds(10);
            czasGry.Start();

            Canvas.Focus();
            p = new Przeciwnicy(5, 75, 75, 6, 100, 1);
            boss = new Przeciwnicy(1, 60, 100, 1, 1000, 500);

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
            p.left = 10;
        }
        private void wygeneruj2()
        {
            boss.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/boss.png"));
            for (int i = 0; i < boss.limit; i++)
            {
                Canvas.Children.Add(boss.StwórzBossa("Boss"));

            }
            liczbap += boss.limit;
        }
        private void GameLoop(object sender, EventArgs e)
        {
            int o = 1;
           
           if (liczbap==0 && p.limit == 10 * o ) 
            {
                wygeneruj2();
                p.limit++;
                o++;
            }
            else if (liczbap == 0)
            {
                wygeneruj1();
            }
            if (pl1.hp <= 1)
                KoniecGry();
            if (pl2.hp <= 1 && multiplayer == true)
                KoniecGry();

            Wynik++;
            Wynik2++;
            Rect Hitboxp = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            Rect Hitboxp2 = new Rect(Canvas.GetLeft(player2), Canvas.GetTop(player2), player2.Width, player2.Height);
            TimerTicks.Content = "Wynik: " + Wynik;            
            Życie.Content = "Życie pl1: " + pl1.hp;
            Życie2.Content = "Życie pl2: " + pl2.hp;





            if (pl1.Lewo == true && Canvas.GetLeft(player) > 0)
            {
                pl1.Ruch_Lewo(player);
            }
            if (pl1.Prawo == true && Canvas.GetLeft(player) + 80 < Application.Current.MainWindow.Width)
            {
                pl1.Ruch_Prawo(player);
            }
            if (pl1.Góra == true && Canvas.GetTop(player) > 0)
            {
                pl1.Ruch_Góra(player);
            }
            if (pl1.Dół == true && Canvas.GetTop(player) + 110 < Application.Current.MainWindow.Height)
            {
                pl1.Ruch_Dół(player);
            }


            if (pl2.Lewo == true && Canvas.GetLeft(player2) > 0)
            {
                pl2.Ruch_Lewo(player2);
            }
            if (pl2.Prawo == true && Canvas.GetLeft(player2) + 80 < Application.Current.MainWindow.Width)
            {
                pl2.Ruch_Prawo(player2);
            }
            if (pl2.Góra == true && Canvas.GetTop(player2) > 0)
            {
                pl2.Ruch_Góra(player2);
            }
            if (pl2.Dół == true && Canvas.GetTop(player2) + 110 < Application.Current.MainWindow.Height)
            {
                pl2.Ruch_Dół(player2);
            }

            List<Rectangle> Przeciwnicy = new List<Rectangle>();
            
            foreach (var x in Canvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "pocisk")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - pocisk1.spdbullet());

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
                        else if (y is Rectangle && Tag == "Boss")
                        {
                            Rect hitboxboss = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (pocisk.IntersectsWith(hitboxboss))
                            {
                                itemsToRemove.Add(x);
                                if (boss.hp < 0)
                                {
                                    itemsToRemove.Add(y);
                                    Wynik += boss.wartość;
                                    liczbap--;
                                }
                                else
                                {
                                    boss.hp -= pocisk1.getDamage();                                    
                                }

                            }
                        }
                    }
                }

                else if (x is Rectangle && (string)x.Tag == "wróg")
                {
                    Przeciwnicy.Add(x);
                    p.poruszanie_przeciwnika(x, p);
                    Rect hitboxprz = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(hitboxprz))
                    {
                        KoniecGry();
                    }
                    if (Hitboxp2.IntersectsWith(hitboxprz) && multiplayer == true)
                    {
                        KoniecGry();
                    }
                }
                else if (x is Rectangle && (string)x.Tag == "Boss")
                {
                    Przeciwnicy.Add(x);
                    boss.RuchBoss(x, boss,ref kierunek);
                    Rect hitboxboss = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(hitboxboss))
                    {
                        KoniecGry();

                    }
                    if (Hitboxp2.IntersectsWith(hitboxboss) && multiplayer == true)
                    {
                        KoniecGry();

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
                        pl1.hp -= 10;
                        itemsToRemove.Add(x);
                    }
                    if (Hitboxp2.IntersectsWith(ppocisk) && multiplayer == true)
                    {
                        pl2.hp -= 10;
                        itemsToRemove.Add(x);
                    }
                }
                else if (x is Rectangle && (string)x.Tag == "przedmiot")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 4);

                    if (Canvas.GetTop(x) > 1080)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect przedmiot = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(przedmiot))
                    {
                        if (pl1.hp > 75)
                        {
                            pl1.hp = 100;
                            itemsToRemove.Add(x);
                        }
                        else if (pl1.hp < 100)
                        {
                            pl1.hp += 25;
                            itemsToRemove.Add(x);
                        }
                    }
                    if (Hitboxp2.IntersectsWith(przedmiot) && multiplayer == true)
                    {
                        if (pl2.hp > 75)
                        {
                            pl2.hp = 100;
                            itemsToRemove.Add(x);
                        }
                        else if (pl2.hp < 100)
                        {
                            pl2.hp += 25;
                            itemsToRemove.Add(x);
                        }
                    }
                }
            }
            foreach (var x in Przeciwnicy)
            {
                if (generator.Next(0, 100) > 98)
                {
                    Canvas.Children.Add(ppocisk.PociskPrzeciwnika(Canvas.GetLeft(x), Canvas.GetTop(x)));
                }
            }
            if (generator.Next(0, 1000) > 998)
                                {
                                    Canvas.Children.Add(apteczka.przedmiot(generator.Next(0,1920), 0));
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
                Canvas.Children.Add(pocisk1.PociskGracza(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width));
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                nazwa = nick.Text;

                if (nazwa == "")
                {
                    nazwa = "Bezimienny";
                }
                

                Ranking obecnyWynik = new Ranking(nazwa, Wynik);

                obecnyWynik.zapisRankingu(obecnyWynik);
                    
                uded.Visibility = Visibility.Hidden;
                nick.Visibility = Visibility.Hidden;
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
                Canvas.SetLeft(player2, 990);
                Canvas.SetTop(player2, 972);
                Wynik = 0;
                
                czasGry.Tick -= GameLoop;
                czasGry.Start();

                Canvas.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Visible;

            }
            if (e.Key == Key.Escape && gameOver == true)
            {
                nazwa = nick.Text;
                Ranking obecnyWynik = new Ranking(nazwa, Wynik);

                obecnyWynik.zapisRankingu(obecnyWynik);

                Application.Current.Shutdown();
            }
            if (e.Key == Key.A && multiplayer == true)
            {
                pl2.Lewo = true;
            }
            if (e.Key == Key.D && multiplayer == true)
            {
                pl2.Prawo = true;
            }
            if (e.Key == Key.W && multiplayer == true)
            {
                pl2.Góra = true;
            }
            if (e.Key == Key.S && multiplayer == true)
            {
                pl2.Dół = true;
            }
            if (e.Key == Key.E && !e.IsRepeat && multiplayer == true)
            {
                Canvas.Children.Add(pocisk1.PociskGracza(Canvas.GetLeft(player2), Canvas.GetTop(player2), player2.Width));
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
            if (e.Key == Key.A)
            {
                pl2.Lewo = false;
            }
            if (e.Key == Key.D)
            {
                pl2.Prawo = false;
            }
            if (e.Key == Key.W)
            {
                pl2.Góra = false;
            }
            if (e.Key == Key.S)
            {
                pl2.Dół = false;
            }
        }

        private void KoniecGry()
        {
            gameOver = true;
            czasGry.Stop();
            uded.Content = "Nie żyjesz! Wciśnij enter do dalszej gry lub ESC aby wyjść!";
            liczbap = 0;
            nick.Visibility = Visibility.Visible;
            uded.Visibility = Visibility.Visible;
        }

        private void Film_MediaEnded(object sender, RoutedEventArgs e)
        {
            Film.Children.Remove(Filmik);
            Filmik.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void DoPrzodu(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Visible;
        }
        private void Cofanie(object sender, RoutedEventArgs e)
        {

            nowyWynik.Content = "";

            Filmik.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void Wranking(object sender, RoutedEventArgs e)
        {
            Ranking pojemnik = new Ranking();

            List<Ranking> ranking = new List<Ranking>();

            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            ranking = pojemnik.odczytRankingu();

            if (ranking == null)
            {
                nowyWynik.Content = "Niedobrze!!!\nRanking na razie jest pusty! \nSpróbuj go uzupełnić pokonując przeciwników!";
            }
            else
            {
                int numeracja = 1;           

                foreach (Ranking rankingObiekt in ranking)
                {
                    //dodawanie nowych linijek wyników

                    nowyWynik.Content += numeracja + ". " + rankingObiekt.nazwaGracza + " " + rankingObiekt.wynik + "\n";

                    numeracja++;
                }   
            }
            TablicaWynikow.Visibility = Visibility.Visible;
        }


        private void Wustawienia(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Visible;
        }

        private void Coop(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Visible;

            multiplayer = true;
            Rozpocznij(multiplayer);
        }

        private void Gra1(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            Menu2.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Visible;

            multiplayer = false;
            Rozpocznij(multiplayer);
        }
        private void Wyjscie(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

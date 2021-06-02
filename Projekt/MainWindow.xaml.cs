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
        bool rywalizacja = false;
        bool smiercGracz1 = false;
        bool smiercGracz2 = false;
        int o = 1;
        ImageBrush pocisk = new ImageBrush();
        DispatcherTimer czasGry = new DispatcherTimer();
        Gracz pl1 = new Gracz(100);
        Gracz pl2 = new Gracz(100);      
        Przeciwnicy boss, p;
        Pocisk pocisk1,ppocisk,pocisk2;
        Przedmiot apteczka, Power;

        public MainWindow()
        {
            InitializeComponent();
            
            Menu.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Filmik.Visibility = Visibility.Visible;

        }
        private void Rozpocznij(bool multiplayer, bool rywalizacja, bool smiercGracza1, bool smiercGracza2) 
        {

            player.Visibility = Visibility.Visible;

            if (multiplayer == true)
            {
                
                player2.Visibility = Visibility.Visible;
                Życie2.Visibility = Visibility.Visible;
                pl1.hp = 100;
                pl2.hp = 100;

                if (rywalizacja == true)
                {
                    TimerTicks2.Visibility = Visibility.Visible;
                }
                else
                {
                    TimerTicks2.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                TimerTicks2.Visibility = Visibility.Hidden;
                player2.Visibility = Visibility.Hidden;
                Życie2.Visibility = Visibility.Hidden;
                pl1.hp = 100;
            }

            ppocisk = new Pocisk(10, 10);
            pocisk1 = new Pocisk(10, 20);
            pocisk2 = new Pocisk(10, 20);

            apteczka = new Przedmiot();
            ppocisk.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/strzal_przeciwnika.png"));
            pocisk1.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/strzal_gracza.png"));
            pocisk2.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/strzal_gracza.png"));
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
            boss = new Przeciwnicy(1, 60, 100, 1, 1000, 500);
            boss.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/boss.png"));
            for (int i = 0; i < boss.limit; i++)
            {
                Canvas.Children.Add(boss.StwórzBossa("Boss"));

            }
            liczbap += boss.limit;
            p.hp += 20;
            p.zwiększ_szybkość();
        }
        private void GameLoop(object sender, EventArgs e)
        {
            
           
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
            if (pl1.hp <= 1 && rywalizacja == false)
            {
                KoniecGry();
            }
            else if (pl1.hp <= 1 && rywalizacja == true)
            {
                smiercGracz1 = true;
                player.Visibility = Visibility.Hidden;
                Życie.Content = "Życie pl1: 0";
                int WynikKoncowy1 = Wynik;
                TimerTicks.Content = "Wynik pl1: " + WynikKoncowy1;

            }
            
                
            if (pl2.hp <= 1 && multiplayer == true && rywalizacja == false)
            {
                KoniecGry();
            }
            else if (pl2.hp <= 1 && multiplayer == true && rywalizacja == true)
            {
                smiercGracz2 = true;
                player2.Visibility = Visibility.Hidden;
                Życie2.Content = "Życie pl2: 0";
                int WynikKoncowy2 = Wynik2;
                TimerTicks2.Content = "Wynik pl2: " + WynikKoncowy2;

            }
                

            if (smiercGracz1 == false)
            {
                Wynik++;
            }

            if (smiercGracz2 == false)
            {
                Wynik2++;
            }
            if (smiercGracz1 == true && smiercGracz2 == true)
            {
                KoniecGry();
            }




                Rect Hitboxp = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            Rect Hitboxp2 = new Rect(Canvas.GetLeft(player2), Canvas.GetTop(player2), player2.Width, player2.Height);
            TimerTicks.Content = "Wynik pl1: " + Wynik;
            TimerTicks2.Content = "Wynik pl2: " + Wynik2;
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
                if (x is Rectangle && (string)x.Tag == "Gracz1" && smiercGracz1 == false)
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
                                if (p.hp < 0)
                                {
                                    itemsToRemove.Add(y);
                                    liczbap--;
                                    Wynik += p.wartość;
                                }
                                else p.hp -= pocisk1.getDamage();
                                
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
                else if (x is Rectangle && (string)x.Tag == "Gracz2")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - pocisk2.spdbullet());

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
                                if (p.hp < 0)
                                {
                                    itemsToRemove.Add(y);
                                    liczbap--;
                                    if (rywalizacja == true)
                                    {
                                        Wynik2 += p.wartość;
                                    }
                                    else
                                    {
                                        Wynik += p.wartość;
                                    }                                    
                                }
                                else p.hp -= pocisk2.getDamage();

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
                                    liczbap--;
                                    if (rywalizacja == true)
                                    {
                                        Wynik2 += p.wartość;
                                    }
                                    else
                                    {
                                        Wynik += p.wartość;
                                    }
                                }
                                else
                                {
                                    boss.hp -= pocisk2.getDamage();
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
                    if (Hitboxp.IntersectsWith(hitboxprz) && smiercGracz1 == false && rywalizacja == false)
                    {
                        KoniecGry();
                    }
                    else if (Hitboxp.IntersectsWith(hitboxprz) && smiercGracz1 == false && rywalizacja == true)
                    {
                        smiercGracz1 = true;
                        pl1.hp = 0;
                        player.Visibility = Visibility.Hidden;
                        Życie.Content = "Życie pl1: 0";
                        int WynikKoncowy1 = Wynik;
                        TimerTicks.Content = "Wynik pl1: " + WynikKoncowy1;
                    }
                    if (Hitboxp2.IntersectsWith(hitboxprz) && multiplayer == true && smiercGracz2 == false && rywalizacja == false)
                    {

                        KoniecGry();
                    }
                    else if (Hitboxp2.IntersectsWith(hitboxprz) && smiercGracz2 == false && rywalizacja == true)
                    {
                        pl2.hp = 0;
                        smiercGracz2 = true;
                        player2.Visibility = Visibility.Hidden;
                        Życie2.Content = "Życie pl2: 0";
                        int WynikKoncowy2 = Wynik2;
                        TimerTicks.Content = "Wynik pl2: " + WynikKoncowy2;
                    }
                }
                else if (x is Rectangle && (string)x.Tag == "Boss")
                {
                    Przeciwnicy.Add(x);
                    boss.RuchBoss(x, boss,ref kierunek);
                    Rect hitboxboss = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(hitboxboss) && smiercGracz1 == false && rywalizacja == false)
                    {
                        KoniecGry();
                    }
                    else if (Hitboxp.IntersectsWith(hitboxboss) && smiercGracz1 == false && rywalizacja == true)
                    {
                        smiercGracz1 = true;
                        player.Visibility = Visibility.Hidden;
                        Życie.Content = "Życie pl1: 0";
                        int WynikKoncowy1 = Wynik;
                        TimerTicks.Content = "Wynik pl1: " + WynikKoncowy1;
                    }
                    if (Hitboxp2.IntersectsWith(hitboxboss) && multiplayer == true && smiercGracz2 == false && rywalizacja == false)
                    {
                        KoniecGry();
                    }
                    else if (Hitboxp2.IntersectsWith(hitboxboss) && smiercGracz2 == false && rywalizacja == true)
                    {
                        smiercGracz2 = true;
                        player2.Visibility = Visibility.Hidden;
                        Życie2.Content = "Życie pl2: 0";
                        int WynikKoncowy2 = Wynik2;
                        TimerTicks.Content = "Wynik pl2: " + WynikKoncowy2;
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
                    if (Hitboxp.IntersectsWith(ppocisk) && smiercGracz1 == false)
                    {
                        pl1.hp -= 10;
                        itemsToRemove.Add(x);
                    }
                    if (Hitboxp2.IntersectsWith(ppocisk) && multiplayer == true && smiercGracz2 == false)
                    {
                        pl2.hp -= 10;
                        itemsToRemove.Add(x);
                    }
                }
                else if (x is Rectangle && (string)x.Tag == "apteczka")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 4);

                    if (Canvas.GetTop(x) > 1080)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect przedmiot = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(przedmiot) && smiercGracz1 == false)
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
                    if (Hitboxp2.IntersectsWith(przedmiot) && multiplayer == true && smiercGracz2 == false)
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
                else if (x is Rectangle && (string)x.Tag == "PowerUp")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 4);

                    if (Canvas.GetTop(x) > 1080)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect przedmiot = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(przedmiot))
                    {
                            itemsToRemove.Add(x);
                        pocisk1.PowerUp();
                    }
                    if (Hitboxp2.IntersectsWith(przedmiot) && multiplayer == true)
                    {
                        itemsToRemove.Add(x);
                        pocisk2.PowerUp();
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
                                    Canvas.Children.Add(apteczka.przedmiot(generator.Next(0,1920), 0,"apteczka"));
                                }
            if (generator.Next(0, 1000) > 999)
            {
                Canvas.Children.Add(Power.przedmiot(generator.Next(0, 1920), 0, "PowerUp"));
            }
            foreach (Rectangle i in itemsToRemove)
            {
                Canvas.Children.Remove(i);
            }

            
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && smiercGracz1 == false)
            {
                pl1.Lewo = true;
            }
            if (e.Key == Key.Right && smiercGracz1 == false)
            {
                pl1.Prawo = true;
            }
            if (e.Key == Key.Up && smiercGracz1 == false)
            {
                pl1.Góra = true;
            }
            if (e.Key == Key.Down && smiercGracz1 == false)
            {
                pl1.Dół = true;
            }
            if (e.Key == Key.Space && !e.IsRepeat && smiercGracz1 == false)
            {
                Canvas.Children.Add(pocisk1.PociskGracza(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, "Gracz1"));
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                if (multiplayer == false)
                {
                    nazwa = nick.Text;

                    if (nazwa == "")
                    {
                        nazwa = "Bezimienny";
                    }


                    Ranking obecnyWynik = new Ranking(nazwa, Wynik);

                    obecnyWynik.zapisRankingu(obecnyWynik);
                }
                
                    
                uded.Visibility = Visibility.Hidden;
                nick.Visibility = Visibility.Hidden;
                gameOver = false;
                foreach (var x in Canvas.Children.OfType<Rectangle>())
                {
                    if (x is Rectangle && (string)x.Tag == "Gracz1" || x is Rectangle && (string)x.Tag == "Gracz2" || x is Rectangle && (string)x.Tag == "wróg" || x is Rectangle && (string)x.Tag == "ppocisk")
                    {
                        itemsToRemove.Add(x);
                    }
                }

                Canvas.SetLeft(player, 915);
                Canvas.SetTop(player, 972);
                Canvas.SetLeft(player2, 990);
                Canvas.SetTop(player2, 972);
                Wynik = 0;
                Wynik2 = 0;
                
                czasGry.Tick -= GameLoop;
                czasGry.Start();

                Canvas.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Visible;

            }
            if (e.Key == Key.Escape && gameOver == true)
            {
                if (multiplayer == false)
                {
                    nazwa = nick.Text;

                    if (nazwa == "")
                    {
                        nazwa = "Bezimienny";
                    }

                    Ranking obecnyWynik = new Ranking(nazwa, Wynik);

                    obecnyWynik.zapisRankingu(obecnyWynik);
                }
                

                Application.Current.Shutdown();
            }
            if (e.Key == Key.A && multiplayer == true && smiercGracz2 == false)
            {
                pl2.Lewo = true;
            }
            if (e.Key == Key.D && multiplayer == true && smiercGracz2 == false)
            {
                pl2.Prawo = true;
            }
            if (e.Key == Key.W && multiplayer == true && smiercGracz2 == false)
            {
                pl2.Góra = true;
            }
            if (e.Key == Key.S && multiplayer == true && smiercGracz2 == false)
            {
                pl2.Dół = true;
            }
            if (e.Key == Key.E && !e.IsRepeat && multiplayer == true && smiercGracz2 == false)
            {
                Canvas.Children.Add(pocisk2.PociskGracza(Canvas.GetLeft(player2), Canvas.GetTop(player2), player2.Width, "Gracz2"));
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
            if (multiplayer == false)
            {               
                uded.Visibility = Visibility.Visible;
                nick.Visibility = Visibility.Visible;

            }           
            uded.Visibility = Visibility.Visible;
        }

        private void Film_MediaEnded(object sender, RoutedEventArgs e)
        {
            Film.Children.Remove(Filmik);
            Filmik.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }
        private void Cofanie(object sender, RoutedEventArgs e)
        {

            nowyWynik.Content = "";

            Filmik.Visibility = Visibility.Collapsed;
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
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Visible;
        }

        private void Coop(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Visible;

            rywalizacja = false;
            multiplayer = true;
            smiercGracz1 = false;
            smiercGracz2 = false;
            Rozpocznij(multiplayer, rywalizacja, smiercGracz1, smiercGracz2);
        }

        private void Rywalizacja(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Visible;

            rywalizacja = true;
            multiplayer = true;
            smiercGracz1 = false;
            smiercGracz2 = false;
            Rozpocznij(multiplayer, rywalizacja, smiercGracz1, smiercGracz2);
        }

        private void Gra1(object sender, RoutedEventArgs e)
        {
            Filmik.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            TablicaWynikow.Visibility = Visibility.Collapsed;
            Ustawienia.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Visible;

            rywalizacja = false;
            multiplayer = false;
            smiercGracz1 = false;
            smiercGracz2 = false;
            Rozpocznij(multiplayer, rywalizacja, smiercGracz1, smiercGracz2);
        }
        private void Wyjscie(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

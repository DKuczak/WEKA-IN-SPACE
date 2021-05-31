﻿using System;
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
using System.IO;


namespace Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double Wynik = 0;
        double[] Wyniki= new double [5];
        int liczbap = 0;
        Random generator = new Random();
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        int CzasPocisku = 0;
        int LimitLotuPocisku = 90;
        bool gameOver = false;

        ImageBrush pocisk = new ImageBrush();
        DispatcherTimer czasGry = new DispatcherTimer();
        Gracz pl1 = new Gracz( 100);
        Gracz pl2 = new Gracz(100);
        Przeciwnicy boss, p;
        Pocisk pocisk1,pocisk2,ppocisk;
        Przedmiot apteczka;

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
            
            pl1.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/Gracz1.png"));
            pl2.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/Gracz1.png"));
            player.Fill = pl1.tekstura;
            pocisk1 = new Pocisk(10, 20);
            pocisk2 = new Pocisk(10, 20);
            ppocisk = new Pocisk(10, 10);
            apteczka = new Przedmiot();
            czasGry.Tick += GameLoop;
            czasGry.Interval = TimeSpan.FromMilliseconds(10);
            czasGry.Start();

            Canvas.Focus();
            p = new Przeciwnicy(5, 75, 75, 6, 100, 1);
            boss = new Przeciwnicy(1, 100, 100, 6, 1000, 500);

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
        private void wygeneruj2()
        {
            boss.tekstura.ImageSource = new BitmapImage(new Uri("pack://application:,,,/materialy/przeciwnik1.png"));
            for (int i = 0; i < p.limit; i++)
            {
                Canvas.Children.Add(boss.StwórzBossa("Boss"));

            }
            liczbap += p.limit;
            p.limit++;
        }
        private void GameLoop(object sender, EventArgs e)
        {
            int o = 1;
           if (liczbap==0 && p.limit == 10 * o) 
            {
                wygeneruj2();
            }
            else if (liczbap == 0)
            {
                wygeneruj1();
            }
           
            Wynik++;
            Rect Hitboxp = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            TimerTicks.Content = "Wynik: " + Wynik;
            Życie.Content = "Życie: " + pl1.hp;
            Rank.Content = "Ranking: " + "\n" + Wyniki[0] + "\n" + Wyniki[1] + "\n" + Wyniki[2] + "\n" + Wyniki[3] + "\n" + Wyniki[4];


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
                                }
                                else boss.hp -= pocisk1.getDamage();
                                liczbap--;
                                Wynik += boss.wartość;

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
                        KoniecGry("Nie żyjesz! ");
                    }
                }
                else if (x is Rectangle && (string)x.Tag == "Boss")
                {
                    Przeciwnicy.Add(x);
                    boss.RuchBoss(x, boss);
                    Rect hitboxboss = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Hitboxp.IntersectsWith(hitboxboss))
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
                        if (pl1.hp <= 1)
                            KoniecGry("Nie żyjesz! ");
                        else pl1.hp -= 10;
                        itemsToRemove.Add(x);
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
                        if (pl1.hp < 100)
                            pl1.hp += 25;
                        itemsToRemove.Add(x);
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
                                    Canvas.Children.Add(apteczka.przedmiot(Canvas.GetLeft(player), 0));
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
            if (Wyniki[4]<Wynik)
            {
                Wyniki[4] = Wynik;
            }
            else if (Wyniki[3] < Wynik)
            {
                Wyniki[4] = Wyniki[3];
                Wyniki[3] = Wynik;
            }
            else if (Wyniki[2] < Wynik)
            {
                Wyniki[4] = Wyniki[3];
                Wyniki[3] = Wyniki[2];
                Wyniki[2] = Wynik;
            }
            else if (Wyniki[1] < Wynik)
            {
                Wyniki[4] = Wyniki[3];
                Wyniki[3] = Wyniki[2];
                Wyniki[2] = Wyniki[1];
                Wyniki[1] = Wynik;
            }
            else if (Wyniki[0] < Wynik)
            {
                Wyniki[4] = Wyniki[3];
                Wyniki[3] = Wyniki[2];
                Wyniki[2] = Wyniki[1];
                Wyniki[1] = Wyniki[0];
                Wyniki[0] = Wynik;
            }
            liczbap = 0;
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

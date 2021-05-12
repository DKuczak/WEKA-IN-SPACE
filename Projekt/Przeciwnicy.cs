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

namespace Klasy_do_WIS
{
    class Przeciwnicy : Cechy_wspólne
    {
        public int limit;
        public int liczba_p;
            int left = 10;

        public Rectangle StwórzPrzeciwnika(string nazwa)
        {
                Rectangle newEnemy = new Rectangle
                {
                    Tag = nazwa,
                    Height = wielkość,
                    Width = szerokość,
                    Fill = tekstura
                };
                Canvas.SetTop(newEnemy, 10);
                Canvas.SetLeft(newEnemy, left);
                left -= 60;


            return newEnemy;
            
        }
        void wypuszczenie_pocisku(int czas)
        {

        }
    }
}

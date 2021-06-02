﻿using System;
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
    class Przeciwnicy : Cechy_wspólne
    {
        public int limit;
        public int left = 10;
        public int wartość;

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
                left -= 80;


            return newEnemy;
            
        }
        public Rectangle StwórzBossa(string nazwa)
        {
            Rectangle newEnemy = new Rectangle
            {
                Tag = nazwa,
                Height = wielkość,
                Width = szerokość,
                Fill = tekstura
            };
            Canvas.SetTop(newEnemy, 0);
            Canvas.SetLeft(newEnemy, 800);


            return newEnemy;

        }
        public Przeciwnicy(int limit, int szerokość, int wielkość, int szybkość, int wartość, int hp) 
        {
            this.limit = limit;
            this.szerokość = szerokość;
            this.wielkość = wielkość;
            this.szybkość = szybkość;
            this.wartość = wartość;
            this.hp = hp;
        }
        public int zwiększ_szybkość()
        {
            szybkość++ ;
            return szybkość;
        }
    }
}

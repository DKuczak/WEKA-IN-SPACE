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
    class Pocisk
    {
        public Rectangle PociskPrzeciwnika(double x, double y)
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

            return (przeciwnikaPocisk);
        }
    }
}

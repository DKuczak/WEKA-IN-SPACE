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
    class Pocisk
    {
        int obrażenia;
        int szybkość_pocisku;
        public ImageBrush tekstura = new ImageBrush();
        public Rectangle PociskPrzeciwnika(double x, double y)
        {
            Rectangle przeciwnikaPocisk = new Rectangle
            {
                Tag = "ppocisk",
                Height = 40,
                Width = 15,
                Fill = tekstura
            };

            Canvas.SetTop(przeciwnikaPocisk, y);
            Canvas.SetLeft(przeciwnikaPocisk, x);

            return (przeciwnikaPocisk);
        }
        public Rectangle PociskGracza(double x, double y, double w, string nazwaGracza)
        {
            Rectangle Pocisk = new Rectangle
            {
                Tag = nazwaGracza,
                Height = 20,
                Width = 5,
                Fill = tekstura
            };

            Canvas.SetTop(Pocisk, y - Pocisk.Height);
            Canvas.SetLeft(Pocisk, x + w / 2);

            return (Pocisk);
        }
        public Pocisk(int obrażenia, int szybkość_pocisku) 
        {
            this.obrażenia = obrażenia;
            this.szybkość_pocisku = szybkość_pocisku;
        }
        public int getDamage() { return obrażenia; }
        public int spdbullet() { return szybkość_pocisku; }
    }
}

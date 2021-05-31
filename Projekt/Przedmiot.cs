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
    class Przedmiot
    {
        public ImageBrush tekstura = new ImageBrush();
        public Rectangle przedmiot(double x, double y)
        {
            Rectangle przedmiot = new Rectangle
            {
                Tag = "przedmiot",
                Height = 40,
                Width = 15,
                Fill = tekstura
            };

            Canvas.SetTop(przedmiot, y);
            Canvas.SetLeft(przedmiot, x);

            return (przedmiot);
        }
    }
}

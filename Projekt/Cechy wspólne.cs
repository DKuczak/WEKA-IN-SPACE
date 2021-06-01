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
    class Cechy_wspólne
    {
        protected int wielkość;
        protected int szerokość;
        public ImageBrush tekstura = new ImageBrush();
        protected int szybkość;
        public int hp;
        public int getspeed() { return szybkość; }
        public Rectangle Ruch_Lewo(Rectangle x) 
        {
            Canvas.SetLeft(x, Canvas.GetLeft(x) - 10); 
            return x; 
        }
        public Rectangle Ruch_Prawo(Rectangle x)
        {
            Canvas.SetLeft(x, Canvas.GetLeft(x) + 10);
            return x;
        }
        public Rectangle Ruch_Góra(Rectangle x)
        {
            Canvas.SetTop(x, Canvas.GetTop(x) - 10);
            return x;
        }
        public Rectangle Ruch_Dół(Rectangle x)
        {
            Canvas.SetTop(x, Canvas.GetTop(x) + 10);
            return x;
        }
        public Rectangle poruszanie_przeciwnika(Rectangle x, Przeciwnicy p) 
        {
            Canvas.SetLeft(x, Canvas.GetLeft(x) + p.getspeed());
            if (Canvas.GetLeft(x) > 1920)
            {
                Canvas.SetLeft(x, -80);
                Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));
            }
            return x;
        }
        public Rectangle RuchBoss(Rectangle x, Przeciwnicy boss ,ref bool z) 
        {
            
            if (Canvas.GetTop(x) < 200) 
            {
                Canvas.SetTop(x, Canvas.GetTop(x) + boss.getspeed());
                
            }
            if (z == true && Canvas.GetLeft(x) > 0)
            {
                Canvas.SetLeft(x, Canvas.GetLeft(x) - boss.getspeed());
                
            }
            else 
            {
                Canvas.SetLeft(x, Canvas.GetLeft(x) + boss.getspeed());
            }
            if (Canvas.GetLeft(x) <= 1) z = false;
            else if (Canvas.GetLeft(x) >= 1920-100) z = true;
            return x; 
        }
    }
}

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
    }
}

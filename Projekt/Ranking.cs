using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projekt
{
    class Ranking
    {
        public string nazwaGracza { get; set; }
        public int wynik { get; set; }

        public Ranking() { }
        public Ranking(string nazwa, int wynik)
        {
            this.nazwaGracza = nazwa;
            this.wynik = wynik;
        }

        public void zapisRankingu(Ranking wynik)
        {
            string drogaDoPliku = "ranking.txt";
            StreamWriter plik;
            // sprawdzenie czy plik istnieje
            if (!File.Exists(drogaDoPliku))
            {
                //jeśli plik nie istnieje należy go stworzyć
                plik = File.CreateText("ranking.txt");
            }
            else
            {
                plik = new StreamWriter(drogaDoPliku, true);
            }

            plik.WriteLine($"{wynik.nazwaGracza} {wynik.wynik}");           
            plik.Close();

        }
        public List<Ranking> odczytRankingu()
        {
            List<Ranking> listaWynikow = new List<Ranking>();
            string drogaDoPliku = "ranking.txt";
            //sprawdzenie czy plik istnieje
            if (!File.Exists(drogaDoPliku))
            {
                //jeśli plik nie istnieje ranking jest pusty
                listaWynikow = null;
            }
            else
            {
                string[] linie = File.ReadAllLines(drogaDoPliku, Encoding.UTF8);

                for (int x = 0; x <linie.Length; x++)
                {
                    string[] obecnaLinia = linie[x].Split(' ');

                    Ranking rankingObiekt = new Ranking
                    {
                        nazwaGracza = obecnaLinia[0],
                        wynik = Convert.ToInt32(obecnaLinia[1]),
                    };
                    listaWynikow.Add(rankingObiekt);
                }

                listaWynikow.Sort((x, y) => x.wynik.CompareTo(y.wynik));
                listaWynikow.Reverse();
            }
            return listaWynikow;
        }


    }
}

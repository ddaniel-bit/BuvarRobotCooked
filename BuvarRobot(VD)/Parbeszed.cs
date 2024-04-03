using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuvarRobot_VD_
{
    internal class Parbeszed
    {
        bool jatekos;
        string title;
        string szoveg;
        string kep;

        public Parbeszed(bool jatekos, string title, string szoveg, string kep)
        {
            this.jatekos = jatekos;
            this.title = title;
            this.szoveg = szoveg;
            this.kep = kep;
        }
        public bool Jatekos { get { return this.jatekos; } }
        public string Title { get { return this.title; } }
        public string Szoveg { get { return this.szoveg; } }
        public string Kep { get { return this.kep; } }

    }
}

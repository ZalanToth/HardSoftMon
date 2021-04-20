using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardSoftMon
{
    class Drives
    {
        public string Gyarto { get; set; }
        public string Nev { get; set; }
        public string Particio { get; set; }
       // public string Helyossz { get; set; }

        public Drives(string gyar, string nev, string hm /*string ho*/)
        {
            this.Gyarto = gyar;
            this.Nev = nev;
            this.Particio = hm;
            //this.Helyossz = ho;
        }
    }
}

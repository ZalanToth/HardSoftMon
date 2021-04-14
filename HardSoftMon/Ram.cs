using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardSoftMon
{
    class Ram
    {
        public string Tag { get; set; }
        public string Kapacitas { get; set; }
        public string Tipus { get; set; }
        public Ram(string tag, string kap, string tip)
        {
            this.Tag = tag;
            this.Kapacitas = kap;
            this.Tipus = tip;
        }

    }
}

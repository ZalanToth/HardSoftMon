using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardSoftMon
{
    class Ram
    {
        public string tag { get; set; }
        public string kapacitas { get; set; }
        public string tipus { get; set; }
        public Ram(string tag, string kap, string tip)
        {
            this.tag = tag;
            this.kapacitas = kap;
            this.tipus = tip;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer
{
    class Audio : Media
    {
        public Audio(string n, string p, Duration l, bool hasv) : base(n,p,l)
        {
            hasvideo = hasv;
        }

        public bool hasvideo { get; set; }
    }
}

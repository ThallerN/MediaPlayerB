using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer
{
    class Media
    {
        public Media(string n,string p, Duration l)
        {
            name = n;
            path = p;
            laenge = l;
        }

        public string name { get; set; }

        public string path { get; set; }

        Duration laenge { get; set; }

        public string GetPath()
        {
            return path;
        }

        public override string ToString()
        {
            return name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer
{
    class Video : Media
    {
        public Video(string n, string p, Duration l, bool hasa, string height, string width) : base(n, p, l)
        {
            resolution = width + "x" + height;
            hasaudio = hasa;
        }

        public string resolution { get; set; }
        public bool hasaudio { get; set; }
    }
}

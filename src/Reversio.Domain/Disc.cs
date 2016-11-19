using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class Disc
    {

        public static Disc Dark = new Disc(-1);
        public static Disc None = new Disc(0);
        public static Disc Light = new Disc(1);

        private Disc(int color)
        {
            Color = color;
        }

        public int Color { get; }
    }
}

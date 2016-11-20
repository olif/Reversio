using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class DiscColor
    {
        public static DiscColor Black = new DiscColor(-1);
        public static DiscColor White = new DiscColor(1);

        private DiscColor(int color)
        {
            Color = color;
        }

        public int Color { get; }
    }
}

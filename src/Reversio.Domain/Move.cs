using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class Move
    {
        public Position Position { get; }

        public Move(int x, int y, Disc color)
        {
            Position = new Position(x, y);
            Color = color;
        }

        public Disc Color { get; }
    }

    public enum Disc
    {
        Dark = -1,
        Light = 1
    }
}

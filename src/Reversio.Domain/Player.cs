using System;

namespace Reversio.Domain
{
    public abstract class ActivePlayer : Player 
    {
        protected ActivePlayer(string name, DiscColor disc) : base(name)
        {
            Disc = disc;
        }

        public DiscColor Disc { get; }
    }

    public class BlackPlayer : ActivePlayer
    {
        public BlackPlayer(string name)
            :base(name, DiscColor.Black)
        {
        }
    }

    public class WhitePlayer : ActivePlayer
    {
        public WhitePlayer(string name)
            : base(name, DiscColor.White)
        {
        }
    }
}

using System;

namespace Reversio.Domain
{
    public abstract class Player : Participant
    {
        protected Player(Guid id, string name, Disc disc) : base(id, name)
        {
            Disc = disc;
        }

        public Disc Disc { get; }
    }

    public class BlackPlayer : Player
    {
        public BlackPlayer(Guid id, string name)
            :base(id, name, Disc.Dark)
        {
        }
    }

    public class WhitePlayer : Player
    {
        public WhitePlayer(Guid id, string name)
            : base(id, name, Disc.Light)
        {
        }
    }
}

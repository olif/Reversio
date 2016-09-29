using System;

namespace Reversio.Domain
{
    public class Player
    {
        public Disc  Disc { get; }

        public Player(Bystander participant, Disc disc)
        {
            Disc = disc;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; internal set; }
    }
}

using System;

namespace Reversio.Domain
{
    public class Player
    {
        public Disc  Disc { get; }

        public Player(User user, Disc disc)
        {
            Disc = disc;
            Id = user.Id;
        }

        public Guid Id { get; internal set; }
    }
}

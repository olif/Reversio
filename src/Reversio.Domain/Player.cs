using System;

namespace Reversio.Domain
{
    public class Player
    {
        public Disc  Disc { get; }

        public Player(Bystander participant, Disc disc)
        {
            Disc = disc;
            Name = participant.Name;
            Id = participant.Id;
        }

        public Guid Id { get; internal set; }
        public string Name { get; set; }
    }
}

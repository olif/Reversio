﻿using System;

namespace Reversio.Domain
{
    public class Player
    {
        public Disc  Disc { get; }

        public Player(Participant participant, Disc disc)
        {
            Disc = disc;
            Id = participant.Id;
        }

        public Guid Id { get; internal set; }
    }
}

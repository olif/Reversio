using System;
using Reversio.Domain;

namespace Reversio.Server.Models
{
    public class MoveModel
    {
        public Player Player { get; set; }

        public PositionModel Position { get; set; }

        public Guid GameId { get; set; }

        public class PositionModel
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Position ToPosition()
            {
                return new Position(X, Y);
            }
        }
    }
}
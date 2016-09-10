using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class GameState
    {
        public GameState(
            Guid gameId,
            Board board,
            Disc discOfNextMove
            )
        {
            GameId = gameId;
            CurrentState = board.CurrentState;
            DiscOfNextMove = discOfNextMove;
        }

        public Guid GameId { get; }

        public Disc DiscOfNextMove { get; }

        public int[,] CurrentState { get; }
    }
}

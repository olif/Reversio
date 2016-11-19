using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain.Events
{
    public delegate void GameStartedHandler(object sender, Guid observerId, GameStartedEventArgs eventArgs);

    public class GameStartedEventArgs
    {
        public GameStartedEventArgs(GameState currentState, Disc assignedDisc)
        {
            CurrentState = currentState;
            PlayerAssignedDisc = assignedDisc;
        }

        public Disc PlayerAssignedDisc { get; set; }

        public GameState CurrentState { get; set; }
    }
}

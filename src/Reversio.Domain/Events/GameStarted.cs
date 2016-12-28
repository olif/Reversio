using System;

namespace Reversio.Domain.Events
{
    public delegate void GameStartedHandler(object sender, Player participant, GameStartedEventArgs eventArgs);

    public class GameStartedEventArgs : EventArgs
    {
        public GameStartedEventArgs(GameStatus currentState, ActivePlayer activePlayer)
        {
            CurrentState = currentState;
            PlayerAssignedDisc = activePlayer.Disc;
        }

        public DiscColor PlayerAssignedDisc { get; set; }

        public GameStatus CurrentState { get; set; }
    }
}

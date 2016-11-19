using System;

namespace Reversio.Domain.Events
{
    public delegate void GameStateChangedHandler(object sender, Guid observerId, GameStateChangedEventArgs e);

    public class GameStateChangedEventArgs
    {
        public GameStateChangedEventArgs(GameState currentState)
        {
            CurrentState = currentState;
        }

        public GameState CurrentState { get; set; }
    }
}

using System;

namespace Reversio.Domain.Events
{
    public delegate void GameStateChangedHandler(object sender, Player participant, GameStateChangedEventArgs e);

    public class GameStateChangedEventArgs : EventArgs
    {
        public GameStateChangedEventArgs(GameState currentState)
        {
            CurrentState = currentState;
        }

        public GameState CurrentState { get; set; }
    }
}

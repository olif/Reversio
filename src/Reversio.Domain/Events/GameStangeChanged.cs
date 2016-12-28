using System;

namespace Reversio.Domain.Events
{
    public delegate void GameStateChangedHandler(object sender, Player participant, GameStateChangedEventArgs e);

    public class GameStateChangedEventArgs : EventArgs
    {
        public GameStateChangedEventArgs(GameStatus currentState)
        {
            CurrentState = currentState;
        }

        public GameStatus CurrentState { get; set; }
    }
}

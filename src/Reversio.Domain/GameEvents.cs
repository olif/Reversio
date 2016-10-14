using System;

namespace Reversio.Domain
{
    public delegate void GameFinishedHandler(object sender, GameFinishedEventArgs e);

    public delegate void GameStateChangedHandler(object sender, GameStateChangedEventArgs e);

    public class GameStateChangedEventArgs
    {
        public GameStateChangedEventArgs(GameState currentState)
        {
            CurrentState = currentState;
        }

        public GameState CurrentState { get; set; }
    }

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(GameState currentState)
        {
            CurrentState = currentState;
        }

        public GameState CurrentState { get; }
    }


}

using System;

namespace Reversio.Domain
{
    public delegate void GameFinishedHandler(object sender, GameFinishedEventArgs e);

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(GameState currentState)
        {
            CurrentState = currentState;
        }

        public GameState CurrentState { get; }
    }
}

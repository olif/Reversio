using System;

namespace Reversio.Domain.Events
{
    public delegate void GameCreatedHandler(object sender, GameCreatedEventArgs eventArgs);

    public class GameCreatedEventArgs : EventArgs
    {
        public GameCreatedEventArgs(GameStatus currentState)
        {
            CurrentState = currentState;
        }

        public GameStatus CurrentState { get; set; }
    }
}

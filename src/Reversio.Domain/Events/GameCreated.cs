using System;

namespace Reversio.Domain.Events
{
    public delegate void GameCreatedHandler(object sender, GameCreatedEventArgs eventArgs);

    public class GameCreatedEventArgs
    {
        public GameCreatedEventArgs(GameState currentState)
        {
            CurrentState = currentState;
        }

        public GameState CurrentState { get; set; }
    }
}

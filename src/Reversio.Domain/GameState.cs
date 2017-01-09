using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class GameState : ValueObject<GameState>
    {
        public static GameState WaitingForOpponent = new GameState("WaitingForOpponent");
        public static GameState Playing = new GameState("Playing");
        public static GameState Finished = new GameState("Finished");
        public static GameState FinishedByWalkover = new GameState("FinishedByWalkover");

        public string State { get; }

        private GameState(string state)
        {
            State = state;
        }

        protected override bool EqualsCore(GameState other)
        {
            return this.State == other.State;
        }

        protected override int GetHashCodeCore()
        {
            return State.GetHashCode();
        }
    }
}


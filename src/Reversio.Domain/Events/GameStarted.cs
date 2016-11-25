﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain.Events
{
    public delegate void GameStartedHandler(object sender, Player participant, GameStartedEventArgs eventArgs);

    public class GameStartedEventArgs
    {
        public GameStartedEventArgs(GameState currentState, ActivePlayer activePlayer)
        {
            CurrentState = currentState;
            PlayerAssignedDisc = activePlayer.Disc;
        }

        public DiscColor PlayerAssignedDisc { get; set; }

        public GameState CurrentState { get; set; }
    }
}
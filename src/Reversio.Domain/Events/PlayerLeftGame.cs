using System;

namespace Reversio.Domain.Events
{
    public delegate void PlayerLeftGameHandler(object sender, PlayerLeftGameEventArgs e);

    public class PlayerLeftGameEventArgs : EventArgs
    {
        public PlayerLeftGameEventArgs(Guid gameId, Player player)
        {
            GameId = gameId;
            Player = player;
        }

        public Guid GameId { get; set; }

        public Player Player { get; set; }
    }
}

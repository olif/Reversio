using System;

namespace Reversio.Domain
{
    internal class PlayerNotRegisteredException : Exception
    {
        private Player player;

        public PlayerNotRegisteredException()
        {
        }

        public PlayerNotRegisteredException(string message) : base(message)
        {
        }

        public PlayerNotRegisteredException(Player player)
        {
            this.player = player;
        }

        public PlayerNotRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
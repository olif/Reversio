using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    /// <summary>
    /// Represents an ongoing game
    /// </summary>
    public class Game
    {
        private readonly Player _whitePlayer;
        private Player _blackPlayer;
        private readonly Board _board;

        public Game(User firstPlayer)
        {
            _board = new Board();
            _whitePlayer = new Player(firstPlayer, Disc.Dark, _board);
        }

        public void JoinOpponent(User joiningUser)
        {
            _blackPlayer = new Player(joiningUser, Disc.Light, _board);
        }

        public Player GetPlayer(User user)
        {
            if (user.Id == _whitePlayer.Id) return _whitePlayer;
            if(user.Id == _blackPlayer.Id) return _blackPlayer;
            throw new ArgumentException("Invalid argument");
        }
    }
}

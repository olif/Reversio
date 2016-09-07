using System;

namespace Reversio.Domain
{
    /// <summary>
    /// Represents an ongoing game
    /// </summary>
    public class Game
    {
        private Player _blackPlayer;
        private Player _whitePlayer;
        private Disc _discOfNextMove;
        public bool GameFinished = false;
        public readonly Board Board;

        public Game(User firstPlayer) : this(firstPlayer, new Board())
        {
        }

        internal Game(User firstPlayer, Board board)
        {
            Board = board;
            _blackPlayer = new Player(firstPlayer, Disc.Dark);
            _discOfNextMove = Disc.Dark;
        }

        public void JoinOpponent(User joiningUser)
        {
            if(_whitePlayer != null)
                throw new InvalidOperationException("An opponent has already joined the game");

            _whitePlayer = new Player(joiningUser, Disc.Light);
        }

        public bool UserMakesMove(User user, Position position)
        {
            var result = false;
            var player = GetPlayerFromUser(user);
            if (player == null) return false;

            if (player.Disc == _discOfNextMove)
            {
                var move = new Move(position, player.Disc);
                result =  Board.TryDoMove(move);
                var otherDisc = ToggleDisc(player.Disc);
                if (Board.HasMoves(otherDisc))
                {
                    _discOfNextMove = otherDisc;
                }
                else if(Board.HasMoves(player.Disc))
                {
                    _discOfNextMove = player.Disc;
                }
                else
                {
                    GameFinished = true;
                }
            }

            return result;
        }

        public Disc ToggleDisc(Disc disc)
        {
            if(disc == Disc.Dark) return Disc.Light;
            return Disc.Dark;
        }

        private Player GetPlayerFromUser(User user)
        {
            if(user.Id == _blackPlayer.Id) return _blackPlayer;
            if(user.Id == _whitePlayer.Id) return _whitePlayer;
            return null;
        }
    }
}

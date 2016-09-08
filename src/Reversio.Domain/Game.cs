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

        public Game(Participant firstPlayer) : this(firstPlayer, new Board())
        {
        }

        internal Game(Participant firstPlayer, Board board)
        {
            Board = board;
            _blackPlayer = new Player(firstPlayer, Disc.Dark);
            _discOfNextMove = Disc.Dark;
        }

        public void JoinOpponent(Participant joiningParticipant)
        {
            if(_whitePlayer != null)
                throw new InvalidOperationException("An opponent has already joined the game");

            _whitePlayer = new Player(joiningParticipant, Disc.Light);
        }

        public bool UserMakesMove(Participant participant, Position position)
        {
            var player = GetActivePlayerFromParticipant(participant);
            if (!IsPlayersTurn(player))
            {
                return false;
            }

            var validMove = Board.TryDoMove(new Move(position, player.Disc));
            if (!validMove) return false;

            var nextDisc = ToggleDisc(player);
            if (nextDisc == null)
            {
                GameFinished = true;
            }
            else
            {
                _discOfNextMove = nextDisc.Value;
            }

            return true;
        }

        private Disc? ToggleDisc(Player player)
        {
            var opponentsDisc = player.Disc == Disc.Dark ? Disc.Light : Disc.Dark;
            if (Board.HasMoves(opponentsDisc))
            {
                return opponentsDisc;
            }
            if (Board.HasMoves(player.Disc))
            {
                return player.Disc;
            }

            return null;
        }

        private bool IsPlayersTurn(Player player) => player.Disc == _discOfNextMove;

        private Player GetActivePlayerFromParticipant(Participant participant)
        {
            if(participant.Id == _blackPlayer.Id) return _blackPlayer;
            if(participant.Id == _whitePlayer.Id) return _whitePlayer;
            throw new ArgumentException("The participant is not a player in this game");
        }
    }
}

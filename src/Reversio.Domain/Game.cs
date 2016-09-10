using System;

namespace Reversio.Domain
{

    /// <summary>
    /// Represents an ongoing game
    /// </summary>
    public class Game
    {
        private readonly Player _blackPlayer;
        private Player _whitePlayer;
        private Disc _discOfNextMove;
        public readonly Board Board;
        public Guid GameId { get; }

        public Game(Bystander firstPlayer) : this(firstPlayer, new Board())
        {}

        internal Game(Bystander firstPlayer, Board board)
        {
            GameId = Guid.NewGuid();
            Board = board;
            _blackPlayer = new Player(firstPlayer, Disc.Dark);
            _discOfNextMove = Disc.Dark;
        }

        public GameState CurrentState => new GameState(GameId, Board, _discOfNextMove);

        public event GameFinishedHandler GameFinished;

        public event PlayerJoinedHandler PlayerJoined;

        private void OnGameFinished()
        {
            GameFinished?.Invoke(this, new GameFinishedEventArgs(CurrentState));
        }

        public void JoinOpponent(Bystander joiningParticipant)
        {
            if(_whitePlayer != null)
                throw new InvalidOperationException("An opponent has already joined the game");

            _whitePlayer = new Player(joiningParticipant, Disc.Light);
            OnPlayerJoined();
        }

        public bool PlayerMakesMove(Player player, Position position)
        {
            if (!IsPlayersTurn(player))
            {
                return false;
            }

            var validMove = Board.TryDoMove(new Move(position, player.Disc));
            if (!validMove) return false;

            var nextDisc = ToggleDisc(player);
            if (nextDisc == null)
            {
                OnGameFinished();
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

        public Player GetActivePlayer(Guid playerId)
        {
            if(playerId == _blackPlayer.Id) return _blackPlayer;
            if(playerId == _whitePlayer.Id) return _whitePlayer;
            throw new ArgumentException("The participant is not a player in this game");
        }

        private void OnPlayerJoined()
        {
            PlayerJoined?.Invoke(this);
        }
    }
}

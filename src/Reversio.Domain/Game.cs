using System;
using System.Collections.Generic;
using System.Linq;

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
        private IReadOnlyList<Position> _lastPiecesFlipped;
        private Move _lastValidMove;

        public Game(Bystander firstPlayer) : this(firstPlayer, new Board())
        {}

        internal Game(Bystander firstPlayer, Board board)
        {
            GameId = Guid.NewGuid();
            Board = board;
            _blackPlayer = new Player(firstPlayer, Disc.Dark);
            _discOfNextMove = Disc.Dark;
            _lastPiecesFlipped = null;
            _lastValidMove = null;
        }

        public GameState CurrentState => new GameState(GameId, Board, _discOfNextMove);

        public event GameFinishedHandler GameFinished;

        public event GameStateChanged GameStateChanged;

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

        public IReadOnlyList<Position> PlayerMakesMove(Player player, Position position)
        {
            if (!IsPlayersTurn(player))
            {
                return null;
            }

            var move = new Move(position, player.Disc);
            var piecesToFlip = Board.TryDoMove(move);
            if (!piecesToFlip.Any())
            {
                return null;
            }

            var nextDisc = ToggleDisc(player);
            if (nextDisc == null)
            {
                OnGameFinished();
                return null;
            }

            _lastValidMove = move;
            _lastPiecesFlipped = piecesToFlip;
            _discOfNextMove = nextDisc;

            OnGameStateChanged();
            return piecesToFlip;
        }

        private Disc ToggleDisc(Player player)
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

        public virtual void OnGameStateChanged()
        {
            GameStateChanged?.Invoke(this, new GameStateChangedEventArgs(CurrentState));
        }
    }
}

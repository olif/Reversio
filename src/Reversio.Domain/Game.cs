using System;
using System.Collections.Generic;
using System.Linq;
using Reversio.Domain.Events;

namespace Reversio.Domain
{

    /// <summary>
    /// Represents an ongoing game
    /// </summary>
    public class Game
    {
        private readonly BlackPlayer _blackPlayer;
        private WhitePlayer _whitePlayer;
        private Disc _discOfNextMove;
        public readonly Board Board;
        public Guid GameId { get; }
        private IReadOnlyList<Position> _lastPiecesFlipped;
        private Move _lastValidMove;
        private bool _isGameFinished;
        private readonly ICollection<Participant> _observers;

        public Game(BlackPlayer player) : this(player, new Board())
        {}

        internal Game(BlackPlayer player, Board board)
        {
            GameId = Guid.NewGuid();
            Board = board;
            _blackPlayer = player;
            _discOfNextMove = Disc.Dark;
            _lastPiecesFlipped = null;
            _lastValidMove = null;
            _isGameFinished = false;

            _observers = new List<Participant>();
            _observers.Add(player);
        }

        public GameState CurrentState => new GameState(GameId, 
            Board, 
            _discOfNextMove, 
            _lastPiecesFlipped, 
            _lastValidMove, 
            _blackPlayer,
            _whitePlayer,
            _isGameFinished);


        public event GameStateChangedHandler GameStateChanged;

        public event PlayerJoinedHandler PlayerJoined;

        public void JoinOpponent(WhitePlayer player)
        {
            if(_whitePlayer != null)
                throw new InvalidOperationException("An opponent has already joined the game");

            _whitePlayer = player;
            _observers.Add(player);
            OnPlayerJoined();
        }

        public void JoinObserver(Participant observer)
        {
            _observers.Add(observer);
        }

        public IReadOnlyList<Position> PlayerMakesMove(Player player, Position position)
        {
            if (!IsPlayersTurn(player))
            {
                return null;
            }

            var move = new Move(position, player.Disc);
            var piecesToFlip = Board.TryDoMove(move);
            if (piecesToFlip == null || piecesToFlip.Count == 0)
            {
                return null;
            }

            var nextDisc = ToggleDisc(player);
            if (nextDisc == null)
            {
                _isGameFinished = true;
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
            foreach (var observer in _observers)
            {
                GameStateChanged?.Invoke(this, observer.Id, new GameStateChangedEventArgs(CurrentState));
            }
        }
    }
}

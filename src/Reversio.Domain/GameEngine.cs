using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversio.Domain.Events;

namespace Reversio.Domain
{
    public class GameEngine
    {
        private object _waitingToPlayLock = new object();
        private Player _waitingPlayer;
        public event GameStartedHandler GameStarted;
        private readonly IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();
        public event GameStateChangedHandler GameStateChanged;

        public void PutPlayerInQueue(Player player)
        {
            lock (_waitingToPlayLock)
            {
                if (_waitingPlayer != null)
                {
                    var blackPlayer = new BlackPlayer(_waitingPlayer.Name);
                    var whitePlayer = new WhitePlayer(player.Name);
                    _waitingPlayer = null;

                    var createdGame = CreateNewGame(blackPlayer);
                    var startedGame = JoinGame(createdGame.GameId, whitePlayer);

                    OnGameStarted(blackPlayer, startedGame);
                    OnGameStarted(whitePlayer, startedGame);
                }
                else
                {
                    _waitingPlayer = player;
                }
            }

        }

        public GameState CreateNewGame(BlackPlayer player)
        {
            var game = new Game(player);
            game.GameStateChanged += (e, observerId, args) =>
            {
                GameStateChanged?.Invoke(this, observerId, args);
            };
            _activeGames.Add(game.GameId, game);
            return game.CurrentState;
        }

        public GameState JoinGame(Guid gameId, WhitePlayer player)
        {
            var game = _activeGames[gameId];
            game.JoinOpponent(player);
            return game.CurrentState;
        }

        protected virtual void OnGameStarted(ActivePlayer player, GameState currentState)
        {
            GameStarted?.Invoke(this, player, new GameStartedEventArgs(currentState, player));
        }
    }
}

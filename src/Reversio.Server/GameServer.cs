using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Reversio.Domain;
using Reversio.Domain.Events;

namespace Reversio.Server
{
    public class GameServer
    {
        public static GameServer Instance = new GameServer();

        private readonly IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();

        private Participant _observerWaitingToPlay;
        private readonly object _waitingToPlayLock = new object();

        public IReadOnlyList<Game> ActiveGames => _activeGames.Values.ToList().AsReadOnly();

        private GameServer() { }

        public event GameStateChangedHandler GameStateChanged;

        /// <summary>
        /// GameCreated is meant to broadcast to all observers that a new game has been initiated
        /// </summary>
        public event GameCreatedHandler GameCreated;

        /// <summary>
        /// GameStarted is mean to tell the players to this particular game that the 
        /// game has started
        /// </summary>
        public event GameStartedHandler GameStarted;

        public void PutObserverInNewGameQueue(Participant observer)
        {
            lock (_waitingToPlayLock)
            {
                if (_observerWaitingToPlay != null)
                {
                    var blackPlayer = new BlackPlayer(_observerWaitingToPlay.Id, _observerWaitingToPlay.Name);
                    var whitePlayer = new WhitePlayer(observer.Id, observer.Name);
                    _observerWaitingToPlay = null;

                    var createdGame = CreateNewGame(blackPlayer);
                    var startedGame = JoinGame(createdGame.GameId, whitePlayer);

                    OnGameCreated(startedGame);

                    OnGameStarted(blackPlayer, startedGame);
                    OnGameStarted(whitePlayer, startedGame);
                }
                else
                {
                    _observerWaitingToPlay = observer;
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

        public GameState JoinObserver(Guid gameId, Participant observer)
        {
            var game = _activeGames[gameId];
            game.JoinObserver(observer);
            return game.CurrentState;
        }

        public IReadOnlyList<Position> MakeMove(Guid gameId, Guid playerId, Position position)
        {
            var game = _activeGames[gameId];
            var player = game.GetActivePlayer(playerId);
            return game.PlayerMakesMove(player, position);
        }

        protected virtual void OnGameCreated(GameState currentState)
        {
            GameCreated?.Invoke(this, new GameCreatedEventArgs(currentState));
        }

        protected virtual void OnGameStarted(Player player, GameState currentState)
        {
            GameStarted?.Invoke(this, player.Id, new GameStartedEventArgs(currentState, player.Disc));
        }
    }
}

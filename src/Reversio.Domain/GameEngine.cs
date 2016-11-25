using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Reversio.Domain.Events;

namespace Reversio.Domain
{
    public class GameEngine
    {
        private object _waitingToPlayLock = new object();
        private Player _waitingPlayer;
        public event GameStartedHandler GameStarted;
        private readonly IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();
        private readonly IDictionary<string, Player> _registeredPlayers = new ConcurrentDictionary<string, Player>();
        public event GameStateChangedHandler GameStateChanged;

        public static GameEngine Instance = new GameEngine();

        private GameEngine()
        {
        }

        public object ActiveGames => _activeGames.Values.ToList().AsReadOnly();

        public void PutPlayerInQueue(Player player)
        {
            AssertPlayerIsRegistered(player);

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

        private void AssertPlayerIsRegistered(Player player)
        {
            if (!_registeredPlayers.ContainsKey(player.Name))
            {
                throw new PlayerNotRegisteredException(player);
            }
        }

        public void RegisterPlayer(Player player)
        {
            if (!_registeredPlayers.ContainsKey(player.Name))
            {
                _registeredPlayers.Add(player.Name, player);
            }
        }

        public GameState CreateNewGame(Player player)
        {
            AssertPlayerIsRegistered(player);

            var blackPlayer = new BlackPlayer(player.Name);

            var game = new Game(blackPlayer);
            game.GameStateChanged += (e, observerId, args) =>
            {
                GameStateChanged?.Invoke(this, observerId, args);
            };
            _activeGames.Add(game.GameId, game);
            return game.CurrentState;
        }

        public GameState JoinGame(Guid gameId, Player player)
        {
            AssertPlayerIsRegistered(player);

            var whitePlayer = new WhitePlayer(player.Name);

            var game = _activeGames[gameId];
            game.JoinOpponent(whitePlayer);
            return game.CurrentState;
        }

        protected virtual void OnGameStarted(ActivePlayer player, GameState currentState)
        {
            GameStarted?.Invoke(this, player, new GameStartedEventArgs(currentState, player));
        }

        public IReadOnlyList<Position> MakeMove(Guid gameId, Player player, Position position)
        {
            AssertPlayerIsRegistered(player);

            var game = _activeGames[gameId];
            return game.PlayerMakesMove(player, position);
        }
    }
}

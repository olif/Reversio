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
        private readonly object _waitingToPlayLock = new object();
        private Player _waitingPlayer;
        public event GameStartedHandler GameStarted;
        private readonly IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();
        private readonly IDictionary<string, Player> _registeredPlayers = new ConcurrentDictionary<string, Player>();
        private readonly IList<Tuple<Player, Player>> _invitations = new List<Tuple<Player, Player>>();

        public event GameStateChangedHandler GameStateChanged;
        public event GameInvitationHandler PlayerInvitedToNewGame;
        public event GameInvitationHandler GameInvitationDeclined;

        public static GameEngine Instance = new GameEngine();

        private GameEngine()
        {
        }

        public IReadOnlyList<GameStatus> ActiveGames => _activeGames.Values.Select(x => x.CurrentState).ToList().AsReadOnly();

        public IReadOnlyList<Player> RegisteredPlayers => _registeredPlayers.Values.ToList().AsReadOnly();

        public bool TryInvitePlayerToGame(Player inviter, Player opponent)
        {
            if (_invitations.Any(x => x.Item1 == inviter))
            {
                throw new Exception("Player can only make one invitation at a time");
            }  

            // Check if the invited player already has joined a game. If so, decline the invitation.
            if(_activeGames.Any(x => x.Value.HasPlayer(opponent)))
            {
                return false;
            }

            _invitations.Add(new Tuple<Player, Player>(inviter, opponent));
            OnPlayerInvitedToNewGame(new InvitationEventArgs(inviter, opponent));

            return true;
        }

        public void InvitationResponse(Player invitee, Player inviter, bool challangeAccepted)
        {
            if (challangeAccepted)
            {
                StartNewGame(invitee, inviter);
            }
            else
            {
                OnGameInvitationDeclined(new ChallangeDeclinedEventArgs(inviter, invitee));
            }

            _invitations.Remove(new Tuple<Player, Player>(inviter, invitee));
        }

        private void StartNewGame(Player invitee, Player inviter)
        {
            var blackPlayer = new BlackPlayer(inviter.Name);
            var whitePlayer = new WhitePlayer(invitee.Name);
            var game = CreateNewGame(blackPlayer);
            var state = JoinGame(game.GameId, whitePlayer);
            OnGameStarted(blackPlayer, state);
            OnGameStarted(whitePlayer, state);
        }

        public void PutPlayerInQueue(Player player)
        {
            AssertPlayerIsRegistered(player);

            lock (_waitingToPlayLock)
            {
                if (_waitingPlayer != null)
                {
                    StartNewGame(_waitingPlayer, player);
                    _waitingPlayer = null;
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

        public GameStatus CreateNewGame(Player player)
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

        public GameStatus JoinGame(Guid gameId, Player player)
        {
            AssertPlayerIsRegistered(player);

            var whitePlayer = new WhitePlayer(player.Name);

            var game = _activeGames[gameId];
            game.JoinOpponent(whitePlayer);
            return game.CurrentState;
        }

        protected virtual void OnGameStarted(ActivePlayer player, GameStatus currentState)
        {
            GameStarted?.Invoke(this, player, new GameStartedEventArgs(currentState, player));
        }

        public IReadOnlyList<Position> MakeMove(Guid gameId, Player player, Position position)
        {
            AssertPlayerIsRegistered(player);

            var game = _activeGames[gameId];
            return game.PlayerMakesMove(player, position);
        }

        protected virtual void OnPlayerInvitedToNewGame(InvitationEventArgs e)
        {
            PlayerInvitedToNewGame?.Invoke(this, e);
        }

        protected virtual void OnGameInvitationDeclined(InvitationEventArgs e)
        {
            GameInvitationDeclined?.Invoke(this, e);
        }

        public void ObserveGame(Guid gameId, Player player)
        {
            AssertPlayerIsRegistered(player);

            var game = _activeGames[gameId];
            game.JoinObserver(player);
        }
    }
}

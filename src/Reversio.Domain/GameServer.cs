using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class GameServer
    {
        public static GameServer Instance = new GameServer();

        private readonly IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();
        private readonly  IDictionary<Guid, Bystander> _bystanders = new ConcurrentDictionary<Guid, Bystander>();

        public IReadOnlyList<Game> ActiveGames => _activeGames.Values.ToList().AsReadOnly();

        private GameServer() { }

        public void RegisterBystander(Bystander bystander)
        {
            _bystanders.Add(bystander.Id, bystander);
        }

        public GameState CreateNewGame(Bystander bystander)
        {
            var game = new Game(bystander);
            _activeGames.Add(game.GameId, game);
            return game.CurrentState;
        }

        public IReadOnlyList<Position> MakeMove(Guid gameId, Bystander bystander, Position position)
        {
            var game = _activeGames[gameId];
            var player = game.GetActivePlayer(bystander.Id);
            return game.PlayerMakesMove(player, position);
        }
    }
}

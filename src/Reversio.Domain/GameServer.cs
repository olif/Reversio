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

        public IReadOnlyList<Game> ActiveGames => _activeGames.Values.ToList().AsReadOnly();

        private GameServer() { }

        public void RegisterBystander(Bystander bystander)
        {
            ;
        }

        public GameState CreateNewGame(Bystander bystander)
        {
            var game = new Game(bystander);
            _activeGames.Add(game.GameId, game);
            return game.CurrentState;
        }
    }
}

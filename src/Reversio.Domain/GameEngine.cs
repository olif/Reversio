using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class GameEngine
    {
        public IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();

        public GameEngine()
        {
        }

        public Game CreateNewGame(Bystander firstPlayer)
        {
            var game = new Game(firstPlayer);
            _activeGames.Add(game.GameId, game);

            return game;
        }
    }
}

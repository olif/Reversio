using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Reversio.Domain;
using System.Linq;

namespace Reversio.Server
{
    public class GameServer
    {
        private readonly IDictionary<Guid, Game> _gamesTable = new ConcurrentDictionary<Guid, Game>();

        public Game CreateNewGame(Bystander firstPlayer)
        {
            var game = new Game(firstPlayer);
            _gamesTable.Add(game.GameId, game);
            return game;
        }

        public IEnumerable<Game> GetGames => _gamesTable.Values;
    }
}

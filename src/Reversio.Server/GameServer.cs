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
        private readonly IDictionary<string, Bystander> _bystanders = new ConcurrentDictionary<string, Bystander>();

        public Game CreateNewGame(Bystander firstPlayer)
        {
            var game = new Game(firstPlayer);
            _gamesTable.Add(game.GameId, game);
            return game;
        }

        public void RegisterBystander(Bystander bystander)
        {
            _bystanders.Add(bystander.Name, bystander);
        }

        public IEnumerable<Game> ActiveGames => _gamesTable.Values;

        public IEnumerable<Bystander> Bystanders => _bystanders.Values;
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Reversio.Domain
{
    public class GamesTable
    {
        private readonly IDictionary<Guid, Game> _activeGames = new ConcurrentDictionary<Guid, Game>();

        public Game CreateNewGame(Bystander firstPlayer)
        {
            var game = new Game(firstPlayer);
            _activeGames.Add(game.GameId, game);

            return game;
        }

        public Game GetGameById(Guid id)
        {
            return _activeGames[id];
        }
    }
}

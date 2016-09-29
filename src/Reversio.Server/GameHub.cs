using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Reversio.Domain;

namespace Reversio.Server
{
    public class GameHub : Hub
    {
        private GameServer _gameServer;

        public GameHub(GameServer server)
        {
            _gameServer = server;
        }

        public IEnumerable<Game> GetGames()
        {
            return _gameServer.ActiveGames;
        }
    }
}

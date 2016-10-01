using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Reversio.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Reversio.WebSocketServer;

namespace Reversio.Server
{
    public class GameServer : WebSocketServer.ServerOld
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
        protected override async Task OnConnected(WebSocketConnection connection)
        {
            await Send(connection.Id, connection.Id.ToString());
        }

        protected override async Task OnMessageReceived(WebSocketConnection connection, string msg)
        {
            Debug.WriteLine(msg);
            var t = new Timer((state) => connection.Close().Wait(), null, 3000, 1000000);
        }

        protected override Task OnClose(WebSocketConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}

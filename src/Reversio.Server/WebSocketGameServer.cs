using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversio.Domain;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameServer : WebSocketServer
    {
        private GameServer _gameServer;

        public WebSocketGameServer(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void OnConnectionOpened(IWebSocketConnection conn)
        {
            ;
        }

        public override void OnMessageReceived(IWebSocketConnection conn, string message)
        {
            ;
        }

        public override void OnConnectionClosed(IWebSocketConnection conn)
        {
            ;
        }
    }
}

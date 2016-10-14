using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Reversio.Domain;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameBrokerFactory : IWebSocketBrokerFactory
    {
        public WebSocketBroker Create(HttpContext context, IWebSocketConnection connection)
        {
            var agent = new WebSocketGameBroker(GameServer.Instance, context);
            connection.OnOpen += () => agent.OnConnectionOpened(connection);
            connection.OnClose += () => agent.OnConnectionClosed(connection);
            connection.OnMessage += (msg) => agent.OnMessageReceived(connection, msg);

            return agent;
        }
    }
}

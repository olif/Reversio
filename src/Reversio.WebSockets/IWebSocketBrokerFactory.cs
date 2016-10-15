using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public abstract class WebSocketBrokerFactory
    {
        internal void AfterCreate(HttpContext context, IWebSocketConnection connection)
        {
            var broker = Create(context, connection);
            broker.Context = context;

            var conn = connection as WebSocketConnection;
            if(conn == null) throw new ArgumentException();
            
            conn.OnOpen += () => broker.OnConnectionOpened(connection);
            conn.OnClose += () => broker.OnConnectionClosed(connection);
            conn.OnMessage += (msg) => broker.OnMessageReceived(connection, msg);
        }

        public abstract WebSocketBroker Create(HttpContext context, IWebSocketConnection connection);
    }
}

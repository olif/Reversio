using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.Server
{
    public class WebSocketServer
    {
        private readonly IDictionary<Guid, WebSocketConnection> _connections =
            new ConcurrentDictionary<Guid, WebSocketConnection>();

        public WebSocketServer()
        {
            OnConnected = () => { };
        }

        public Action OnConnected;

        public async Task Handle(HttpContext httpContext)
        {
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            var connectionId = Guid.NewGuid();
            var connection = new WebSocketConnection(socket, connectionId);
            _connections.Add(connectionId, connection);
            connection.ConnectionOpen += () =>
            {
                OnConnected();
            };
            connection.MessageReceived += async () =>
            {
                await connection.Send("hello hello hello");
            };
            await connection.StartMonitoring();
        }
    }
}

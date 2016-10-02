using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public abstract class WebSocketServer
    {
        private readonly IDictionary<Guid, IWebSocketConnection> _activeConnections = 
            new ConcurrentDictionary<Guid, IWebSocketConnection>();

        public abstract void OnMessageReceived(IWebSocketConnection conn, string message);

        public abstract void OnConnectionClosed(IWebSocketConnection conn);

        public abstract void OnConnectionOpened(IWebSocketConnection conn);

        private void OnCloseInternal(IWebSocketConnection connection)
        {
            _activeConnections.Remove(connection.Id);
            OnConnectionClosed(connection);
        }

        internal async Task ProcessRequest(HttpContext context)
        {
            try
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync(subProtocol: null);

                var connection = new WebSocketConnection(socket, CancellationToken.None);
                connection.OnOpen = () => OnConnectionOpened(connection);
                connection.OnMessage = (msg) => OnMessageReceived(connection, msg);
                connection.OnClose = () => OnCloseInternal(connection);

                _activeConnections.Add(connection.Id, connection);
                await connection.ProcessRequest(CancellationToken.None);
            }
            catch (Exception)
            {
                // If acceptwebsocketasync failed, do not remove from dictionary
                // If socket already added -> error
                // If processrequest failed -> error
                throw;
            }
        }
    }
}

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

        public virtual Action<IWebSocketConnection> OnOpen { get; set; }

        public virtual Action<IWebSocketConnection, string> OnMessage { get; set; }

        public virtual Action<IWebSocketConnection> OnClose { get; set; }

        private void OnCloseInternal(IWebSocketConnection connection)
        {
            _activeConnections.Remove(connection.Id);
            OnClose(connection);
        }

        internal async Task ProcessRequest(HttpContext context)
        {
            try
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync(subProtocol: null);
                var connection = new WebSocketConnection();
                _activeConnections.Add(connection.Id, connection);

                connection.OnMessage = (msg) => OnMessage?.Invoke(connection, msg);
                OnOpen?.Invoke(connection);

                await connection.ProcessRequest(socket, CancellationToken.None);
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

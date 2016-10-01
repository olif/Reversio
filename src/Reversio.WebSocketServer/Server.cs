using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSocketServer
{
    public abstract class ServerOld
    {
        private readonly IDictionary<Guid, WebSocketConnection> _activeConnections =
            new ConcurrentDictionary<Guid, WebSocketConnection>();

        protected ServerOld()
        {
        }

        protected IEnumerable<WebSocketConnection> ActiveConnections => _activeConnections.Values;

        protected abstract Task OnConnected(WebSocketConnection connection);

        protected abstract Task OnMessageReceived(WebSocketConnection connection, string msg);

        protected abstract Task OnClose(WebSocketConnection connection);

        private async Task OnCloseInternal(WebSocketConnection connection)
        {
            _activeConnections.Remove(connection.Id);
            await OnClose(connection);
        }

        protected async Task Send(Guid connectionId, string message)
        {
            await _activeConnections[connectionId].Send(message);
        }

        protected void Broadcast(string message)
        {
            foreach (var conn in _activeConnections.Values)
            {
                conn.Send(message).ConfigureAwait(false);
            }
        }

        internal async Task ProcessRequest(HttpContext httpContext)
        {
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            var connectionId = Guid.NewGuid();
            var connection = new WebSocketConnection(
                socket,
                connectionId)
            {
                OnConnected = (conn) => OnConnected(conn),
                OnMessageReceived = (conn, msg) => OnMessageReceived(conn, msg),
                OnClose = async (conn) => await OnCloseInternal(conn)
            };
            _activeConnections.Add(connectionId, connection);
            await connection.StartMonitoring();
        }
    }
}

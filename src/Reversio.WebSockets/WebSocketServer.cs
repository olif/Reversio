using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public class WebSocketServer
    {
        private IWebSocketAgentFactory _agentFactory;
        //private void OnCloseInternal(IWebSocketConnection connection)
        //{
        //    _activeConnections.Remove(connection.Id);
        //    OnConnectionClosed(connection);
        //}

        public WebSocketServer(IWebSocketAgentFactory agentFactory)
        {
            _agentFactory = agentFactory;
        }

        internal async Task ProcessRequest(HttpContext context)
        {
            try
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync(subProtocol: null);

                var connection = new WebSocketConnection(socket, CancellationToken.None);
                _agentFactory.Create(context, connection);
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

    public interface IWebSocketAgentFactory
    {
        WebSocketAgent Create(HttpContext context, IWebSocketConnection connection);
    }
}

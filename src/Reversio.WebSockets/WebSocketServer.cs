using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public class WebSocketServer
    {
        private readonly WebSocketBrokerFactory _brokerFactory;

        public WebSocketServer(WebSocketBrokerFactory agentFactory)
        {
            _brokerFactory = agentFactory;
        }

        internal async Task ProcessRequest(HttpContext context)
        {
            try
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync(subProtocol: null);

                var connection = new WebSocketConnection(socket, CancellationToken.None);
                _brokerFactory.AfterCreate(context, connection);
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

using Microsoft.AspNetCore.Http;
using Reversio.Domain;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameBrokerFactory : WebSocketBrokerFactory
    {
        public override WebSocketBroker Create(HttpContext context, IWebSocketConnection connection)
        {
            return new WebSocketGameBroker(GameServer.Instance);
        }
    }
}

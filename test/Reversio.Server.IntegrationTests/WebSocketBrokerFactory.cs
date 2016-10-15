using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Reversio.WebSockets;

namespace Reversio.Server.IntegrationTests
{
    public class WebSocketBrokerFactoryStub : WebSockets.WebSocketBrokerFactory
    {
        public static WebSocketBrokerStub Instace = new WebSocketBrokerStub();

        public override WebSocketBroker Create(HttpContext context, IWebSocketConnection connection)
        {
            return Instace;
        }
    }
}

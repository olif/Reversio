using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public abstract class WebSocketBroker
    {
        protected HttpContext Context;
        protected WebSocketBroker(HttpContext context)
        {
            Context = context;
        }

        public abstract void OnMessageReceived(IWebSocketConnection conn, string message);

        public abstract void OnConnectionClosed(IWebSocketConnection conn);

        public abstract void OnConnectionOpened(IWebSocketConnection conn);
    }
}

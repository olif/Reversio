using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.WebSockets
{
    public interface IWebSocketConnection
    {
        Guid Id { get; }

        Task Send(string message);

        Action OnOpen { get; set; }
        Action<string> OnMessage { get; set; }
        Action OnClose { get; set; }
        Action<Exception> OnError { get; set; }

        Task CloseConnection();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public interface IWebSocketConnection
    {
        Guid Id { get; }

        Task Send(string message);

        Task CloseConnection();
    }
}

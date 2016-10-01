using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Reversio.Server.IntegrationTests
{
    public class Class1
    {
        private TestServer _testServer;
        private WebSocketClient _client;
        private WebSocketServerStub _stub;
        
        public Class1()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = _testServer.CreateWebSocketClient();
        }

        [Fact]
        public async Task Test()
        {
            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
            var data = Encoding.UTF8.GetBytes("hello");
            await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

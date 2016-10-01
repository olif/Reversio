using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reversio.WebSocketServer;

namespace Reversio.Server.IntegrationTests
{
    public class TestStartup
    {
        public static SimpleWebSocketServer Server = new SimpleWebSocketServer();

        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseWebSockets();
            app.UseSimpleServer();
        }
    }
}

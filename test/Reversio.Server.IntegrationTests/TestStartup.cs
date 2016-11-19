﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reversio.WebSockets;

namespace Reversio.Server.IntegrationTests
{
    public class TestStartup
    {
        public static WebSocketServerStub Server = new WebSocketServerStub();

        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSingleton<WebSocketServer>(Server);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseWebSockets();
            app.UseSimpleServer();
        }
    }
}

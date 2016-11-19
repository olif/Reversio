using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Reversio.Server
{
    /// <summary>
    /// Simple middleware for setting the principal by using user
    /// credentials from request header
    /// </summary>
    public class SimpleAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SimpleAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
    


            await _next.Invoke(context);
        }
    }
}

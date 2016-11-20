using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Reversio.Server.Auth
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;

        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            if (!context.Request.Method.Equals("POST") ||
                !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request");
            }

            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username and/or password");
            }

            var now = DateTimeOffset.UtcNow;
            var randomGenerator = new Random((int) DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var guestSerialNr = randomGenerator.Next(100000);
            var guestUsername = $"guest-{guestSerialNr}";

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, guestUsername), 
                new Claim(ClaimTypes.Name, guestUsername),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), 
            };

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now.DateTime,
                expires: now.Add(_options.Expiration).DateTime,
                signingCredentials: _options.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int) _options.Expiration.TotalSeconds
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            }), Encoding.UTF8);
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            if (username.Equals("guest", StringComparison.OrdinalIgnoreCase) &&
                password.Equals("guest", StringComparison.OrdinalIgnoreCase))
            {
                var claims = new[]
                {
                    new Claim("reversio.claims.usertype", "guest"), 
                    new Claim(ClaimTypes.Name, "test")
                };
                var identity = new ClaimsIdentity(new GenericIdentity(username, "Token"), claims);
                return Task.FromResult(identity);
            }

            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}

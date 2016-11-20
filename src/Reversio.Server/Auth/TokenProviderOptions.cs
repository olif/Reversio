using System;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Reversio.Server.Auth
{
    public class TokenProviderOptions
    {
        public string Path { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
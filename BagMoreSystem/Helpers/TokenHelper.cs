﻿using System.IdentityModel.Tokens.Jwt;

namespace BagMoreSystem.Helpers
{
    public class TokenHelper
    {
        private const string BEARER_PREFIX = "Bearer ";
        public static JwtSecurityToken ReadToken(HttpContext httpContext)
        {
            string authorizationToken = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorizationToken) == false)
            {
                string token = authorizationToken.Substring(BEARER_PREFIX.Length);

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                return jsonToken;
            }
            return null;
        }
    }
}

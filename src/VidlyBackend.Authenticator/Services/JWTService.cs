using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using VidlyBackend.Authentication.Models;

namespace VidlyBackend.Authentication.Services
{
    public class JWTService : IAuthService
    {
        /// <summary>
        /// The secret key used to encrypt this token.
        /// </summary>
        private IAuthContainerSettings _settings { get; set; }

        public JWTService(IAuthContainerSettings authContainerModel)
        {
            _settings = authContainerModel;
        }

        /// <summary>
        /// Generates an encrypted token with the given model.
        /// </summary>
        /// <param name="model"></param>
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            if (claims is null || claims.Count() == 0)
                throw new ArgumentException("Provided arguments for token creation are invalid.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(GetSecurityKey(_settings.SecretKey), _settings.SecurityAlgorithm)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Retrieves the cliams of a given token as a string.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Provided token is null or empty.");

            var tokenParams = GetTokenValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = tokenHandler.ValidateToken(token, tokenParams, out _);
                return tokenValid.Claims;
            }
            catch { throw; }
        }

        /// <summary>
        /// Tries to validate a token. If it is not a valid token, it returns false.
        /// </summary>
        /// <param name="token"></param>
        public bool TryGetTokenClaims(string token, out IEnumerable<Claim> claims)
        {
            if (string.IsNullOrEmpty(token))
            {
                claims = null;
                return false;
            }

            var tokenParams = GetTokenValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();

            try { claims = tokenHandler.ValidateToken(token, tokenParams, out _).Claims; }
            catch { claims = null; }

            return claims != null;
        }

        public SymmetricSecurityKey GetSecurityKey(string key)
        {
            var keyData = Encoding.UTF8.GetBytes(key);
            return new SymmetricSecurityKey(keyData);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSecurityKey(_settings.SecretKey)
            };
        }
    }
}

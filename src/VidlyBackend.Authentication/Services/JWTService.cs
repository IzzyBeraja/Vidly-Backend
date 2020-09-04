using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VidlyBackend.Authentication.Models;

namespace VidlyBackend.Authentication.Services
{
    public class JWTService : IAuthService
    {
        /// <summary>
        /// The secret key used to encrypt this token.
        /// </summary>
        public string SecretKey { get; set; }

        public JWTService(string secretKey)
        {
            SecretKey = secretKey;
        }

        /// <summary>
        /// Generates an encrypted token with the given model.
        /// </summary>
        /// <param name="model"></param>
        public string GenerateToken(IAuthContainerModel model)
        {
            if (model is null || model.Claims is null || model.Claims.Length == 0)
                throw new ArgumentException("Provided arguments for token creation are invalid.");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.UtcNow.AddMinutes(model.ExpireMinutes),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
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

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return tokenValid.Claims;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Performs validation on a given token returning true when valid and false when it is not.
        /// </summary>
        /// <param name="token"></param>
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token provided is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal tokenValid;
            try
            {
                tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch
            {
                tokenValid = null;
            }
            return tokenValid != null;
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(SecretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}

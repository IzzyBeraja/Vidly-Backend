using Authenticator.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authenticator.Services
{
    public interface IAuthService
    {
        string HeaderName { get; }
        bool TryGetTokenClaims(string token, out IEnumerable<Claim> claims);
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateToken(TokenModel claims);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}

using System.Collections.Generic;
using System.Security.Claims;

namespace Authenticator.Services
{
    public interface IAuthService
    {
        bool TryGetTokenClaims(string token, out IEnumerable<Claim> claims);
        string GenerateToken(IEnumerable<Claim> claims);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}

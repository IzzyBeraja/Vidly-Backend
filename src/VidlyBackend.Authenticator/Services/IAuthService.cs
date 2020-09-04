using System.Collections.Generic;
using System.Security.Claims;
using VidlyBackend.Authentication.Models;

namespace VidlyBackend.Authentication.Services
{
    public interface IAuthService
    {
        bool TryGetTokenClaims(string token, out IEnumerable<Claim> claims);
        string GenerateToken(IEnumerable<Claim> claims);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}

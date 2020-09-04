using System.Collections.Generic;
using System.Security.Claims;
using VidlyBackend.Authentication.Models;

namespace VidlyBackend.Authentication.Services
{
    public interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}

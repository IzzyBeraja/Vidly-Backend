using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;

namespace Authenticator.Services
{
    public class JwtAuthentication : AuthenticationHandler<JwtBearerOptions>
    {
        private readonly IAuthService _authService;

        public JwtAuthentication(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthService authService)
            : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        public bool AllowMultiple { get; }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Request does not contain header: 'Authentication'");

            string token = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Authorization header contains no data");

            try
            {
                return validateToken(token);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private AuthenticateResult validateToken(string token)
        {
            var claims = _authService.GetTokenClaims(token);
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new GenericPrincipal(identity, claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToArray());
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}

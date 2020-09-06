using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Authenticator.Attributes
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public JwtAuthenticationAttribute(bool allowMultiple = false)
        {
            AllowMultiple = allowMultiple;
        }

        public bool AllowMultiple { get; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization is null || authorization.Scheme != "JWTBearer")
                return;

            if(string.IsNullOrEmpty(authorization.Parameter))
            {
                return;
            }

        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

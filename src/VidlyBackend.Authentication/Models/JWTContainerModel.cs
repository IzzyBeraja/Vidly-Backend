using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace VidlyBackend.Authentication.Models
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; } = string.Empty;
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 60;

        public Claim[] Claims { get; set; }
    }
}

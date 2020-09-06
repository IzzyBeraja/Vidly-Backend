using System;
using System.Security.Claims;

namespace Authenticator.Models
{
    public interface IAuthContainerSettings
    {
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
    }
}
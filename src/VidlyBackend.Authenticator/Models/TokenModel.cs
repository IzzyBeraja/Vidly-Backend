using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Authenticator.Models
{
    public class TokenModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsEmpty => Email is null && Name is null && Id is null;
    }
}

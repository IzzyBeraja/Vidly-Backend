using Authenticator.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authenticator.Handlers
{
    public class ClaimsBuilder
    {
        public IEnumerable<Claim> GetClaims(TokenModel tokenModel)
        {
            if (tokenModel is null || tokenModel.IsEmpty)
                throw new ArgumentException("Token model provided in null or empty");

            List<Claim> claims = new List<Claim>();
            if (BuildClaim(ClaimTypes.Email, tokenModel.Email, out Claim claim)) claims.Add(claim);
            if (BuildClaim(ClaimTypes.Name, tokenModel.Name, out claim)) claims.Add(claim);
            if (BuildClaim(ClaimTypes.NameIdentifier, tokenModel.Id, out claim)) claims.Add(claim);
            if (BuildClaim(ClaimTypes.Role, tokenModel.Role, out claim)) claims.Add(claim);
            return claims;
        }

        private bool BuildClaim(string claimType, string value, out Claim claim)
        {
            claim = null;
            if (string.IsNullOrEmpty(value) || claimType is null)
                return false;

            claim = new Claim(claimType, value);
            return true;
        }

        //public TokenModel GetTokenModel(IEnumerable<Claim> claims)
        //{
        //    foreach(var claim in claims)
        //    {
        //        claim
        //    }
        //}
    }
}

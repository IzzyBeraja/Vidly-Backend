using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authenticator.Models;
using Authenticator.Services;
using BCrypt.Net;
using DataManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Controllers
{
    [Route("/api/Auth")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private string _collectionName = "users";
        private HashType _hashType = HashType.SHA384;
        private readonly IAuthService _auth;

        public TokenController(IDatabaseContext dbContext, IAuthService auth)
        {
            _dbContext = dbContext;
            _auth = auth;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserReadDto>> AuthorizeUser(UserAuthDto userAuthDto)
        {
            var userFromRepo = await _dbContext.GetAsync<User>(_collectionName, nameof(Models.User.Email), userAuthDto.Email);
            if (userFromRepo is null || !BCrypt.Net.BCrypt.EnhancedVerify(userAuthDto.Password, userFromRepo.Password, _hashType))
                return BadRequest("Incorrect authentication credentials.");

            TokenModel tokenModel = new TokenModel { Email = userFromRepo.Email, Name = userFromRepo.Name, Id = userFromRepo.Id };
            var token = _auth.GenerateToken(tokenModel);
            Response.Headers.Add(_auth.HeaderName, token);
            Response.Headers.Add("access-control-expose-headers", _auth.HeaderName);
            return Ok("Login Successful");
        }
    }
}

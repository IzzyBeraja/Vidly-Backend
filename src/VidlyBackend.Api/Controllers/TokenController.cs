using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using DataManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Authentication.Services;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Controllers
{
    [Route("/api/Auth")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string _collectionName = "users";
        private HashType _hashType = HashType.SHA384;
        private readonly IAuthService _auth;

        public TokenController(IDatabaseContext dbContext, IMapper mapper, IAuthService auth)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _auth = auth;
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> AuthorizeUser(UserAuthDto userAuthDto)
        {
            var userFromRepo = await _dbContext.GetAsync<User>(_collectionName, nameof(Models.User.Email), userAuthDto.Email);
            if (userFromRepo is null || !BCrypt.Net.BCrypt.EnhancedVerify(userAuthDto.Password, userFromRepo.Password, _hashType))
                return BadRequest("Incorrect authentication credentials.");

            List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.Name, userAuthDto.Email) };
            var token = _auth.GenerateToken(claims);
            return Ok(token);
        }
    }
}

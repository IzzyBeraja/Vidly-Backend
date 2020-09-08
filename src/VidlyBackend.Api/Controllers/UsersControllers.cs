using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;
using BCrypt.Net;
using DataManager.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Authenticator.Services;
using System.Security.Claims;
using Authenticator.Models;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string _collectionName = "users";
        private HashType _hashType = HashType.SHA384;
        private readonly IAuthService _auth;

        public UsersController(IDatabaseContext dbContext, IMapper mapper, IAuthService auth)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _auth = auth;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> Get()
        {
            var users = await _dbContext.GetAsync<User>(_collectionName);
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserReadDto>> GetUserById(string id)
        {
            var user = await _dbContext.GetAsync<User>(_collectionName, id);
            if (user is null)
                return NotFound();

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var userFromRepo = await _dbContext.GetAsync<User>(_collectionName, nameof(Models.User.Email), userCreateDto.Email);
            if (userFromRepo != null)
                return BadRequest("User is already registered.");

            var user = _mapper.Map<User>(userCreateDto);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, _hashType);
            await _dbContext.CreateAsync(_collectionName, user);

            var userReadDto = _mapper.Map<UserReadDto>(user);

            TokenModel token = new TokenModel { Email = userReadDto.Email, Name = userReadDto.Name, Id = userReadDto.Id };
            Response.Headers.Add(_auth.HeaderName, _auth.GenerateToken(token));
            Response.Headers.Add("access-control-expose-headers", _auth.HeaderName);

            return CreatedAtRoute(nameof(GetUserById), new { id = user.Id.ToString() }, userReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser<User>(string id)
        {
            var user = await _dbContext.GetAsync<User>(_collectionName, id);
            if (user is null)
                return NotFound();

            await _dbContext.RemoveAsync<User>(_collectionName, id);
            return NoContent();
        }
    }
}

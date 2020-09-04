using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;
using BCrypt.Net;
using DataManager.Services;
using System.Threading.Tasks;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string _collectionName = "users";
        private HashType _hashType = HashType.SHA384;

        public UsersController(IDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var userFromRepo = await _dbContext.GetAsync<User>(_collectionName, nameof(Models.User.Email), userCreateDto.Email);
            if (userFromRepo != null)
                return BadRequest("User is already registered.");

            var user = _mapper.Map<User>(userCreateDto);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, _hashType);
            await _dbContext.CreateAsync(_collectionName, user);

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return CreatedAtRoute(nameof(GetUserById), new { id = user.Id.ToString() }, userReadDto);
        }

        [Route("~/api/Auth")]
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> AuthorizeUser(UserAuthDto userAuthDto)
        {
            var userFromRepo = await _dbContext.GetAsync<User>(_collectionName, nameof(Models.User.Email), userAuthDto.Email);
            if (userFromRepo is null || !BCrypt.Net.BCrypt.EnhancedVerify(userAuthDto.Password, userFromRepo.Password, _hashType))
                return BadRequest("Incorrect authentication credentials.");

            // Works but needs to generate a JWT
            return Ok("Nice");
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

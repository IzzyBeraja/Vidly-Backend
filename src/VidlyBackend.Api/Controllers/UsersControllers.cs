using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;
using BCrypt.Net;
using DataManager.Services;

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
        public ActionResult<IEnumerable<UserReadDto>> Get()
        {
            var users = _dbContext.Get<User>(_collectionName);
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(string id)
        {
            if (!_dbContext.Get(_collectionName, id, out User user))
                return NotFound();

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPost]
        public ActionResult<UserReadDto> CreateUser(UserCreateDto userCreateDto)
        {
            var userFromRepo = _dbContext.Get<User>(_collectionName, nameof(VidlyBackend.Models.User.Email), userCreateDto.Email);
            if (userFromRepo != null)
                return BadRequest("User is already registered.");

            var user = _mapper.Map<User>(userCreateDto);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, _hashType);
            _dbContext.Create(_collectionName, user);

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return CreatedAtRoute(nameof(GetUserById), new { id = user.Id.ToString() }, userReadDto);
        }

        [Route("~/api/Auth")]
        [HttpPost]
        public ActionResult<UserReadDto> AuthorizeUser(UserAuthDto userAuthDto)
        {
            var userFromRepo = _dbContext.Get<User>(_collectionName, nameof(VidlyBackend.Models.User.Email), userAuthDto.Email);
            if (userFromRepo is null || !BCrypt.Net.BCrypt.EnhancedVerify(userAuthDto.Password, userFromRepo.Password, _hashType))
                return BadRequest("Incorrect authentication credentials.");

            // Works but needs to generate a JWT
            return Ok("Nice");
        }

        // [HttpPut("{id}")]
        // public ActionResult UpdateUser(string id, UserUpdateDto userUpdateDto)
        // {
        //     if (!GetFromDatabase(id, out User userFromRepo))
        //         return NotFound();

        //     var user = _mapper.Map(userUpdateDto, userFromRepo);

        //     _dbContext.Update(collectionName, id, user);
        //     return NoContent();
        // }

        // [HttpPatch("{id}")]
        // public ActionResult PatchUser(string id, JsonPatchDocument<UserUpdateDto> patchDocument)
        // {
        //     if (!GetFromDatabase(id, out User userFromRepo))
        //         return NotFound();

        //     var userToPatch = _mapper.Map<UserUpdateDto>(userFromRepo);
        //     patchDocument.ApplyTo(userToPatch, ModelState);

        //     if (!TryValidateModel(userToPatch))
        //         return ValidationProblem(ModelState);

        //     var user = _mapper.Map(userToPatch, userFromRepo);

        //     _dbContext.Update(collectionName, id, user);
        //     return NoContent();
        // }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser<User>(string id)
        {
            if (!_dbContext.Get<User>(_collectionName, id, out _))
                return NotFound();

            _dbContext.Remove<User>(_collectionName, id);
            return NoContent();
        }
    }
}

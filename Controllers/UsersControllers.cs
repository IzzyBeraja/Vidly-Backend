using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using VidlyBackend.Dto;
using VidlyBackend.Models;
using VidlyBackend.Services;
using BCrypt.Net;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string _collectionName = "users";

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
        public ActionResult<UserReadDto> Create(UserCreateDto userCreateDto)
        {
            var userFromRepo = _dbContext.Get<User>(_collectionName, nameof(VidlyBackend.Models.User.Email), userCreateDto.Email);
            if (userFromRepo != null)
                return BadRequest("User is already registered.");

            var user = _mapper.Map<User>(userCreateDto);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, HashType.SHA384);
            _dbContext.Create(_collectionName, user);

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return CreatedAtRoute(nameof(GetUserById), new { id = user.Id.ToString() }, userReadDto);
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

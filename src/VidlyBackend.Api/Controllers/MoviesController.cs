using System.Collections.Generic;
using System.Threading.Tasks;
using Authenticator.Services;
using AutoMapper;
using DataManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthService _auth;
        private string _collectionName = "movies"; // appSettings

        public MoviesController(IDatabaseContext dbContext, IMapper mapper, IAuthService auth)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _auth = auth;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MovieReadDto>>> Get()
        {
            var movies = await _dbContext.GetAsync<Movie>(_collectionName);
            return Ok(_mapper.Map<IEnumerable<MovieReadDto>>(movies));
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        [AllowAnonymous]
        public async Task<ActionResult<MovieReadDto>> GetMovieById(string id)
        {
            var movie = await _dbContext.GetAsync<Movie>(_collectionName, id);
            if (movie is null)
                return NotFound();

            return Ok(_mapper.Map<MovieReadDto>(movie));
        }

        [HttpPost]
        public async Task<ActionResult<MovieReadDto>> Create(MovieCreateDto movieCreateDto)
        {
            var movie = _mapper.Map<Movie>(movieCreateDto);
            await _dbContext.CreateAsync(_collectionName, movie);

            var movieReadDto = _mapper.Map<MovieReadDto>(movie);

            return CreatedAtRoute(nameof(GetMovieById), new { id = movie.Id.ToString() }, movieReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(string id, MovieUpdateDto movieUpdateDto)
        {
            var movieFromRepo = await _dbContext.GetAsync<Movie>(_collectionName, id);
            if (movieFromRepo is null)
                return NotFound();

            var movie = _mapper.Map(movieUpdateDto, movieFromRepo);

            await _dbContext.UpdateAsync(_collectionName, id, movie);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchMovie(string id, JsonPatchDocument<MovieUpdateDto> patchDocument)
        {
            var movieFromRepo = await _dbContext.GetAsync<Movie>(_collectionName, id);
            if (movieFromRepo is null)
                return NotFound();

            var movieToPatch = _mapper.Map<MovieUpdateDto>(movieFromRepo);
            patchDocument.ApplyTo(movieToPatch, ModelState);

            if (!TryValidateModel(movieToPatch))
                return ValidationProblem(ModelState);

            var movie = _mapper.Map(movieToPatch, movieFromRepo);

            await _dbContext.UpdateAsync(_collectionName, id, movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie1<Movie>(string id)
        {
            var movieFromRepo = await _dbContext.GetAsync<Movie>(_collectionName, id);
            if (movieFromRepo is null)
                return NotFound();

            await _dbContext.RemoveAsync<Movie>(_collectionName, id);
            return NoContent();
        }
    }
}

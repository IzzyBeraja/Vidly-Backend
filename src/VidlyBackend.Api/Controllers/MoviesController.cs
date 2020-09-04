using System.Collections.Generic;
using AutoMapper;
using DataManager.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string _collectionName = "movies"; // appSettings

        public MoviesController(IDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MovieReadDto>> Get()
        {
            var movies = _dbContext.Get<Movie>(_collectionName);
            return Ok(_mapper.Map<IEnumerable<MovieReadDto>>(movies));
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        public ActionResult<MovieReadDto> GetMovieById(string id)
        {
            if (!_dbContext.Get(_collectionName, id, out Movie movie))
                return NotFound();

            return Ok(_mapper.Map<MovieReadDto>(movie));
        }

        [HttpPost]
        public ActionResult<MovieReadDto> Create(MovieCreateDto movieCreateDto)
        {
            var movie = _mapper.Map<Movie>(movieCreateDto);
            _dbContext.Create(_collectionName, movie);

            var movieReadDto = _mapper.Map<MovieReadDto>(movie);

            return CreatedAtRoute(nameof(GetMovieById), new { id = movie.Id.ToString() }, movieReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateMovie(string id, MovieUpdateDto movieUpdateDto)
        {
            if (!_dbContext.Get(_collectionName, id, out Movie movieFromRepo))
                return NotFound();

            var movie = _mapper.Map(movieUpdateDto, movieFromRepo);

            _dbContext.Update(_collectionName, id, movie);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchMovie(string id, JsonPatchDocument<MovieUpdateDto> patchDocument)
        {
            if (!_dbContext.Get(_collectionName, id, out Movie movieFromRepo))
                return NotFound();

            var movieToPatch = _mapper.Map<MovieUpdateDto>(movieFromRepo);
            patchDocument.ApplyTo(movieToPatch, ModelState);

            if (!TryValidateModel(movieToPatch))
                return ValidationProblem(ModelState);

            var movie = _mapper.Map(movieToPatch, movieFromRepo);

            _dbContext.Update(_collectionName, id, movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMovie<Movie>(string id)
        {
            if (!_dbContext.Get<Movie>(_collectionName, id, out _))
                return NotFound();

            _dbContext.Remove<Movie>(_collectionName, id);
            return NoContent();
        }
    }
}

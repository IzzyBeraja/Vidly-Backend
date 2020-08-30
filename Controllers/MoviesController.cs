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

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IDatabaseContext<Movie> _movieService;
        private readonly IMapper _mapper;
        private string collectionName = "movies"; // appSettings

        public MoviesController(IDatabaseContext<Movie> movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MovieReadDto>> Get()
        {
            var movies = _movieService.Get(collectionName);
            return Ok(_mapper.Map<IEnumerable<MovieReadDto>>(movies));
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        public ActionResult<MovieReadDto> GetMovieById(string id)
        {
            if (!GetFromDatabase(id, out Movie movie))
                return NotFound();

            return Ok(_mapper.Map<MovieReadDto>(movie));
        }

        [HttpPost]
        public ActionResult<MovieReadDto> Create(MovieCreateDto movieCreateDto)
        {
            var movie = _mapper.Map<Movie>(movieCreateDto);
            _movieService.Create(collectionName, movie);

            var movieReadDto = _mapper.Map<MovieReadDto>(movie);

            return CreatedAtRoute(nameof(GetMovieById), new { id = movie.Id.ToString() }, movieReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateMovie(string id, MovieUpdateDto movieUpdateDto)
        {
            if (!GetFromDatabase(id, out Movie movieFromRepo))
                return NotFound();

            var movie = _mapper.Map(movieUpdateDto, movieFromRepo);

            _movieService.Update(collectionName, id, movie);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchMovie(string id, JsonPatchDocument<MovieUpdateDto> patchDocument)
        {
            if (!GetFromDatabase(id, out Movie movieFromRepo))
                return NotFound();

            var movieToPatch = _mapper.Map<MovieUpdateDto>(movieFromRepo);
            patchDocument.ApplyTo(movieToPatch, ModelState);

            if (!TryValidateModel(movieToPatch))
                return ValidationProblem(ModelState);

            var movie = _mapper.Map(movieToPatch, movieFromRepo);

            _movieService.Update(collectionName, id, movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(string id)
        {
            if (!GetFromDatabase(id, out _))
                return NotFound();

            _movieService.Remove(collectionName, id);
            return NoContent();
        }

        /// <summary>
        /// Helper function to get document from collection in database
        /// </summary>
        private bool GetFromDatabase(string id, out Movie movie)
        {
            movie = _movieService.Get(collectionName, id);
            return movie != null;
        }
    }
}

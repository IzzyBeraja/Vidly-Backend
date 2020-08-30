using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private string collectionName = "movies";

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

        [HttpGet("{id:length(24)}", Name = "GetMovieById")]
        public ActionResult<MovieReadDto> GetMovieById(string id)
        {
            var movie = _movieService.Get(collectionName, id);
            
            if (movie is null)
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

        [HttpPut("{id:length(24)}")]
        public ActionResult UpdateMovie(string id, MovieUpdateDto movieUpdateDto)
        {
            var movieFromRepo = _movieService.Get(collectionName, id);

            if (movieFromRepo is null)
                return NotFound();

            var movie = _mapper.Map(movieUpdateDto, movieFromRepo);

            _movieService.Update(collectionName, id, movie);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult DeleteMovie(string id)
        {
            var movie = _movieService.Get(collectionName, id);

            if (movie is null)
                return NotFound();

            _movieService.Remove(collectionName, id);
            return NoContent();
        }
    }
}

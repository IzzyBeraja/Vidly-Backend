using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;
using VidlyBackend.Services;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id:length(24)}", Name = "GetMovie")]
        public ActionResult<MovieReadDto> Get(string id)
        {
            var movie = _movieService.Get(collectionName, id);
            
            if (movie is null)
                return NotFound();

            return Ok(_mapper.Map<MovieReadDto>(movie));
        }

        [HttpPost]
        public ActionResult<Movie> Create(Movie movie)
        {
            _movieService.Create(collectionName, movie);
            return CreatedAtRoute("GetMovie", new { id = movie.Id.ToString() }, movie);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(string id, Movie movieIn)
        {
            var movie = _movieService.Get(collectionName, id);

            if (movie is null)
                return NotFound();

            _movieService.Update(collectionName, id, movieIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            var movie = _movieService.Get(collectionName, id);

            if (movie is null)
                return NotFound();

            _movieService.Remove(collectionName, id);
            return NoContent();
        }
    }
}

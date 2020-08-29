using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using VidlyBackend.Models;
using VidlyBackend.Services;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMongoCRUD<MovieModel> _movieService;
        private string collectionName = "movies";

        public MovieController(IMongoCRUD<MovieModel> movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IEnumerable<MovieModel> Get() =>
            _movieService.Get(collectionName);

        [HttpGet("{id:length(24)}", Name = "GetMovie")]
        public ActionResult<IEnumerable<MovieModel>> Get(string id)
        {
            var movie = _movieService.Get(collectionName, ObjectId.Parse(id));

            if (movie is null)
                return NotFound();

            return movie;
        }

        [HttpPost]
        public ActionResult<MovieModel> Create(MovieModel movie)
        {
            _movieService.Create(collectionName, movie);
            return CreatedAtRoute("GetMovie", new { id = movie.Id.ToString() }, movie);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, MovieModel movieIn)
        {
            var movie = _movieService.Get(collectionName, ObjectId.Parse(id));

            if (movie is null)
                return NotFound();

            _movieService.Update(collectionName, ObjectId.Parse(id), movieIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var movie = _movieService.Get(collectionName, ObjectId.Parse(id));

            if (movie is null)
                return NotFound();

            _movieService.Remove(collectionName, ObjectId.Parse(id));
            return NoContent();
        }
    }
}

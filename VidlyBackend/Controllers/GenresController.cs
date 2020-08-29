using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VidlyBackend.Models;
using VidlyBackend.Services;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IDatabaseContext<Genre> _genreService;
        private string collectionName = "genres";

        public GenresController(IDatabaseContext<Genre> genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public IEnumerable<Genre> Get() =>
            _genreService.Get(collectionName);

        [HttpGet("{id:length(24)}", Name = "GetGenre")]
        public ActionResult<Genre> Get(string id)
        {
            var genre = _genreService.Get(collectionName, id);

            if (genre is null)
                return NotFound();

            return genre;
        }

        [HttpPost]
        public ActionResult<Genre> Create(Genre genre)
        {
            _genreService.Create(collectionName, genre);
            return CreatedAtRoute("GetGenre", new { id = genre.Id.ToString() }, genre);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Genre genreIn)
        {
            var genre = _genreService.Get(collectionName, id);

            if (genre is null)
                return NotFound();

            _genreService.Update(collectionName, id, genreIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var genre = _genreService.Get(collectionName, id);

            if (genre is null)
                return NotFound();

            _genreService.Remove(collectionName, id);
            return NoContent();
        }
    }
}

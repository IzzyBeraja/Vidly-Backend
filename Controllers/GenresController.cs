using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VidlyBackend.Dto;
using VidlyBackend.Models;
using VidlyBackend.Services;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Genres")]
    public class GenresController : ControllerBase
    {
        private readonly IDatabaseContext<Genre> _genreService;
        private readonly IMapper _mapper;
        private string collectionName = "genres";

        public GenresController(IDatabaseContext<Genre> genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GenreReadDto>> Get()
        {
            var genres = _genreService.Get(collectionName);
            return Ok(_mapper.Map<IEnumerable<GenreReadDto>>(genres));
        }

        [HttpGet("{id}", Name = "GetGenreById")]
        public ActionResult<GenreReadDto> GetGenreById(string id)
        {
            if (!GetFromDatabase(id, out Genre genre))
                return NotFound();

            return Ok(_mapper.Map<GenreReadDto>(genre));
        }

        [HttpPost]
        public ActionResult<GenreReadDto> Create(GenreCreateDto genreCreateDto)
        {
            var genre = _mapper.Map<Genre>(genreCreateDto);
            _genreService.Create(collectionName, genre);

            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return CreatedAtRoute(nameof(GetGenreById), new { id = genre.Id.ToString() }, genreReadDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre(string id, GenreUpdateDto genreUpdateDto)
        {
            if (!GetFromDatabase(id, out Genre genreFromRepo))
                return NotFound();

            var genre = _mapper.Map(genreUpdateDto, genreFromRepo);

            _genreService.Update(collectionName, id, genre);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchGenre(string id, JsonPatchDocument<GenreUpdateDto> patchDocument)
        {
            if (!GetFromDatabase(id, out Genre genreFromRepo))
                return NotFound();

            var genreToPatch = _mapper.Map<GenreUpdateDto>(genreFromRepo);
            patchDocument.ApplyTo(genreToPatch, ModelState);

            if (!TryValidateModel(genreToPatch))
                return ValidationProblem(ModelState);

            var genre = _mapper.Map(genreToPatch, genreFromRepo);

            _genreService.Update(collectionName, id, genre);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var genre = _genreService.Get(collectionName, id);

            if (genre is null)
                return NotFound();

            _genreService.Remove(collectionName, id);
            return NoContent();
        }

        /// <summary>
        /// Helper function to get document from collection in database
        /// </summary>
        private bool GetFromDatabase(string id, out Genre genre)
        {
            genre = _genreService.Get(collectionName, id);
            return genre != null;
        }
    }
}

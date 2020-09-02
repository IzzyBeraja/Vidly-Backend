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
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string collectionName = "genres";

        public GenresController(IDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GenreReadDto>> Get()
        {
            var genres = _dbContext.Get<Genre>(collectionName);
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
            _dbContext.Create(collectionName, genre);

            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return CreatedAtRoute(nameof(GetGenreById), new { id = genre.Id.ToString() }, genreReadDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre(string id, GenreUpdateDto genreUpdateDto)
        {
            if (!GetFromDatabase(id, out Genre genreFromRepo))
                return NotFound();

            var genre = _mapper.Map(genreUpdateDto, genreFromRepo);

            _dbContext.Update(collectionName, id, genre);
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

            _dbContext.Update(collectionName, id, genre);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var genre = _dbContext.Get<Genre>(collectionName, id);

            if (genre is null)
                return NotFound();

            _dbContext.Remove<Genre>(collectionName, id);
            return NoContent();
        }

        /// <summary>
        /// Helper function to get document from collection in database
        /// </summary>
        private bool GetFromDatabase<T>(string id, out T documentOut)
        {
            documentOut = _dbContext.Get<T>(collectionName, id);
            return documentOut != null;
        }
    }
}

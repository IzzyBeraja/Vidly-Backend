using System.Collections.Generic;
using AutoMapper;
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

        [HttpGet("{id:length(24)}", Name = "GetGenre")]
        public ActionResult<GenreReadDto> GetGenreById(string id)
        {
            var genre = _genreService.Get(collectionName, id);

            if (genre is null)
                return NotFound();

            return Ok(_mapper.Map<GenreReadDto>(genre));
        }

        [HttpPost]
        public ActionResult<GenreReadDto> Create(Genre genreCreateDto)
        {
            var genre = _mapper.Map<Genre>(genreCreateDto);
            _genreService.Create(collectionName, genre);
            return CreatedAtRoute(nameof(GetGenreById), new { id = genre.Id.ToString() }, genre);
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

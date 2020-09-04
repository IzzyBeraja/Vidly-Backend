using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataManager.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Controllers
{
    [ApiController]
    [Route("api/Genres")]
    public class GenresController : ControllerBase
    {
        private readonly IDatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private string _collectionName = "genres";

        public GenresController(IDatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreReadDto>>> Get()
        {
            var genres = await _dbContext.GetAsync<Genre>(_collectionName);
            return Ok(_mapper.Map<IEnumerable<GenreReadDto>>(genres));
        }

        [HttpGet("{id}", Name = "GetGenreById")]
        public async Task<ActionResult<GenreReadDto>> GetGenreById(string id)
        {
            var genre = await _dbContext.GetAsync<Genre>(_collectionName, id);
            if (genre is null)
                return NotFound();

            return Ok(_mapper.Map<GenreReadDto>(genre));
        }

        [HttpPost]
        public async Task<ActionResult<GenreReadDto>> Create(GenreCreateDto genreCreateDto)
        {
            var genre = _mapper.Map<Genre>(genreCreateDto);
            await _dbContext.CreateAsync(_collectionName, genre);

            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return CreatedAtRoute(nameof(GetGenreById), new { id = genre.Id.ToString() }, genreReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGenre(string id, GenreUpdateDto genreUpdateDto)
        {
            var genreFromRepo = await _dbContext.GetAsync<Genre>(_collectionName, id);
            if (genreFromRepo is null)
                return NotFound();

            var genre = _mapper.Map(genreUpdateDto, genreFromRepo);

            await _dbContext.UpdateAsync(_collectionName, id, genre);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchGenre(string id, JsonPatchDocument<GenreUpdateDto> patchDocument)
        {
            var genreFromRepo = await _dbContext.GetAsync<Genre>(_collectionName, id);
            if (genreFromRepo is null)
                return NotFound();

            var genreToPatch = _mapper.Map<GenreUpdateDto>(genreFromRepo);
            patchDocument.ApplyTo(genreToPatch, ModelState);

            if (!TryValidateModel(genreToPatch))
                return ValidationProblem(ModelState);

            var genre = _mapper.Map(genreToPatch, genreFromRepo);

            await _dbContext.UpdateAsync(_collectionName, id, genre);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGenre(string id)
        {
            var genreFromRepo = await _dbContext.GetAsync<Genre>(_collectionName, id);
            if (genreFromRepo is null)
                return NotFound();

            await _dbContext.RemoveAsync<Genre>(_collectionName, id);
            return NoContent();
        }
    }
}

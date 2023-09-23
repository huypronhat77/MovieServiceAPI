using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieServiceWebAPI.Model;
using MovieServiceWebAPI.Services;
using Serilog;

namespace MovieServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private IGenreRepository _genreRepository;
        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;             
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            return Ok(_genreRepository.GetAll());
        }

        [HttpGet("id")]
        public IActionResult GetGenreById(string id) 
        {
            var selectedGenre = _genreRepository.GetById(id);

            if (selectedGenre != null)
            {
                return Ok(selectedGenre);
            }

            return NotFound("Invalid Genre Id");
        }

        [HttpPost]
        public IActionResult PostGenre(GenreVM genreVM)
        {
            try
            {
                var newGenre = _genreRepository.Add(genreVM);
                return Ok(newGenre);
            }
            catch (Exception e)
            {
                Log.Error($"Create Genre Failed {e.Message}, statck trace: {e.StackTrace}");
                return BadRequest($"Fail to add new Genre with error {e.Message}, statck trace: {e.StackTrace}");
            }
        }

        [HttpPut("id")]
        public IActionResult PutGenre(string id, GenreVM genreVM)
        {
            try
            {
                var isSuccess = false;
                _genreRepository.Update(id, genreVM, ref isSuccess);

                if (isSuccess)
                {
                    return NoContent();
                }

                return BadRequest($"Fail to update Genre with id: {id}");
                
            }
            catch (Exception e)
            {
                Log.Error($"Update Genre Error {e.Message}, statck trace: {e.StackTrace}");
                return BadRequest($"Fail to update new Genre with error {e.Message}, statck trace: {e.StackTrace}");
            }
        }

        [HttpDelete("id")]
        public IActionResult DeleteGenre(string id)
        {
            try
            {
                var isSuccess = false;
                _genreRepository.DeleteById(id, ref isSuccess);

                if (isSuccess) { 
                    return NoContent();               
                }

                return BadRequest($"Fail to delete Genre with id: {id}");
            }
            catch (Exception e)
            {
                Log.Error($"Delete Genre Error {e.Message}, statck trace: {e.StackTrace}");
                return BadRequest($"Fail to delete new Genre with error {e.Message}, statck trace: {e.StackTrace}");
            }
        }
    }
}

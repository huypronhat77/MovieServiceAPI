using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieServiceWebAPI.Model;
using MovieServiceWebAPI.Services;
using Serilog;

namespace MovieServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            return Ok(_movieRepository.GetAll());
        }

        [HttpGet("id")]
        public IActionResult GetMovieById(string id)
        {
            var selectedMovie = _movieRepository.GetById(id);

            if (selectedMovie != null)
            {
                return Ok(selectedMovie);
            }

            return NotFound("Invalid Movie Id");
        }

        [HttpPost]
        public IActionResult PostMovie(MovieVM movieVM)
        {
            try
            {
                var newMovie = _movieRepository.Add(movieVM);
                return Ok(newMovie);
            }
            catch (Exception e)
            {
                Log.Error($"Create Movie Failed {e.Message}, statck trace: {e.StackTrace}");
                return BadRequest($"Fail to add new Movie with error {e.Message}, statck trace: {e.StackTrace}");
            }
        }

        [HttpPut("id")]
        public IActionResult PutMovie(string id, MovieVM movieVM)
        {
            try
            {
                var isSuccess = false;
                _movieRepository.Update(id, movieVM, ref isSuccess);

                if (isSuccess)
                {
                    return NoContent();
                }

                return BadRequest($"Fail to update Movie with id: {id}");

            }
            catch (Exception e)
            {
                Log.Error($"Update Movie Error {e.Message}, statck trace: {e.StackTrace}");
                return BadRequest($"Fail to update Movie with error {e.Message}, statck trace: {e.StackTrace}");
            }
        }

        [HttpDelete("id")]
        public IActionResult DeleteMovie(string id)
        {
            try
            {
                var isSuccess = false;
                _movieRepository.DeleteById(id, ref isSuccess);

                if (isSuccess)
                {
                    return NoContent();
                }

                return BadRequest($"Fail to delete Movie with id: {id}");
            }
            catch (Exception e)
            {
                Log.Error($"Delete Movie Error {e.Message}, statck trace: {e.StackTrace}");
                return BadRequest($"Fail to delete Movie with error {e.Message}, statck trace: {e.StackTrace}");
            }
        }
    }
}

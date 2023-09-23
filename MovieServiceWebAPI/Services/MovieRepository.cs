using Microsoft.EntityFrameworkCore;
using MovieServiceWebAPI.Data;
using MovieServiceWebAPI.Model;

namespace MovieServiceWebAPI.Services
{
    public class MovieRepository : IMovieRepository
    {
        private MyDBContext _dbContext;
        public MovieRepository(MyDBContext dBContext)
        {
              _dbContext = dBContext;
        }
        public MovieVM Add(MovieVM entity)
        {
            if (entity == null) throw new ArgumentNullException();

            var validGenreReturn = new List<string>();

            Movie newMovie = new Movie()
            {
                Description = entity.Description,
                ReleaseYear = entity.ReleaseYear,
                Status = entity.Status,
                Title = entity.Title,
            };

            _dbContext.Movies.Add(newMovie);

            if (entity.SelectedGenres.Any())
            {
                foreach (var item  in entity.SelectedGenres)
                {
                    var selectedGenre = _dbContext.Genres.FirstOrDefault(x =>  x.Name.Equals(item));

                    if (selectedGenre != null)
                    {
                        MovieGenre movieGenre = new MovieGenre()
                        {
                            Genre = selectedGenre,
                            Movie = newMovie
                        };

                        validGenreReturn.Add(item);
                        _dbContext.MovieGenres.Add(movieGenre);
                    }
                }
            }

            _dbContext.SaveChanges();

            return new MovieVM() {
                Description = newMovie.Description,
                ReleaseYear = newMovie.ReleaseYear,
                Status = newMovie.Status,
                Title = newMovie.Title,
                SelectedGenres = validGenreReturn
            };
        }

        public void DeleteById(string id, ref bool isSuccess)
        {
            var selectedMovie = GetMovieById(id);
            if (selectedMovie != null) 
            {
                var movieGenreRemove = _dbContext.MovieGenres.Where(mg => mg.MovieId == selectedMovie.Id).ToList();
                _dbContext.MovieGenres.RemoveRange(movieGenreRemove);

                _dbContext.Movies.Remove(selectedMovie);
                _dbContext.SaveChanges();
                isSuccess = true;
            }
        }

        public List<MovieVM> GetAll()
        {
            var result = new List<MovieVM>();
            var movieList = _dbContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .ToList();

            foreach ( var movie in movieList )
            {
                var movieVM = new MovieVM()
                {
                    Description = movie.Description,
                    ReleaseYear = movie.ReleaseYear,
                    Status = movie.Status,
                    Title = movie.Title,
                    SelectedGenres = movie.MovieGenres.Select(x => x.Genre.Name).ToList()
                };

                result.Add(movieVM);
            }

            return result;
        }

        public MovieVM GetById(string id)
        {
            var selectedMovie = GetMovieById(id);

            if (selectedMovie != null)
            {
                return new MovieVM()
                {
                    Description = selectedMovie.Description,
                    ReleaseYear = selectedMovie.ReleaseYear,
                    Status = selectedMovie.Status,
                    Title = selectedMovie.Title,
                    SelectedGenres = selectedMovie.MovieGenres.Select(x => x.Genre.Name).ToList()
                };
            }

            return null;
        }

        public void Update(string id, MovieVM entity, ref bool isSuccess)
        {
            var selectedMovie = GetMovieById(id);

            if (selectedMovie != null)
            {
                selectedMovie.Description = string.IsNullOrEmpty(entity.Description) ? selectedMovie.Description : entity.Description;
                selectedMovie.ReleaseYear = entity.ReleaseYear;
                selectedMovie.Status = string.IsNullOrEmpty(entity.Status) ? selectedMovie.Status : entity.Status;
                selectedMovie.Title = string.IsNullOrEmpty(entity.Title) ? selectedMovie.Title : entity.Title;


                if (entity.SelectedGenres.Any())
                {
                    var selectedGenreNames = entity.SelectedGenres;

                    // Fetch all genres in one query and create a dictionary for easy lookup
                    var genres = _dbContext.Genres.Where(g => selectedGenreNames.Contains(g.Name))
                                                   .ToDictionary(g => g.Name, g => g);

                    if (genres.Any())
                    {
                        // Fetch existing MovieGenres records for the selected movie
                        var existingMovieGenres = _dbContext.MovieGenres
                            .Where(mg => mg.MovieId == selectedMovie.Id)
                            .ToList();

                        // Remove existing MovieGenres records
                        _dbContext.MovieGenres.RemoveRange(existingMovieGenres);

                        // Create and add new MovieGenre entities
                        var newMovieGenres = selectedGenreNames
                            .Select(genreName => new MovieGenre
                            {
                                MovieId = selectedMovie.Id,
                                GenreId = genres[genreName].Id
                            })
                            .ToList();

                        _dbContext.MovieGenres.AddRange(newMovieGenres);
                    }
                }

                _dbContext.SaveChanges();
                isSuccess = true;
            }
        }

        private Movie GetMovieById(string id)
        {
            if (int.TryParse(id, out int validMovieId))
            {
                return _dbContext.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefault(x => x.Id.Equals(validMovieId));
            }

            return null;
        }
    }
}

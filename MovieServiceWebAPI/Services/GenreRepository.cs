using MovieServiceWebAPI.Data;
using MovieServiceWebAPI.Model;

namespace MovieServiceWebAPI.Services
{
    public class GenreRepository : IGenreRepository
    {
        private MyDBContext _dbContext;
        public GenreRepository(MyDBContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public GenreVM Add(GenreVM entity)
        {
            Genre genre = new Genre() 
            {
                Name = entity.Name,
                Description = entity.Description,
            };

            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();

            return new GenreVM() 
            {
                Name = entity.Name,
                Description = entity.Description,
            };

        }

        public void DeleteById(string id, ref bool isSuccess)
        {
            var selectedGenre = GetGenreById(id);

            if (selectedGenre != null)
            {
                _dbContext.Genres.Remove(selectedGenre);
                _dbContext.SaveChanges();
                isSuccess = true;
            }
        }

        public List<GenreVM> GetAll()
        {
            return _dbContext.Genres.Select(x => new GenreVM() 
            { 
                Name = x.Name, 
                Description = x.Description 
            }).ToList();
        }

        public GenreVM GetById(string id)
        {
            var selectedGenre = GetGenreById(id);

            if (selectedGenre != null)
            {
                return new GenreVM()
                {
                    Name = selectedGenre.Name,
                    Description = selectedGenre.Description
                };
            }

            return null;
        }

        public void Update(string id, GenreVM entity, ref bool isSuccess)
        {
            var selectedGenre = GetGenreById(id);

            if (selectedGenre != null)
            {
                selectedGenre.Name = entity.Name;
                selectedGenre.Description = string.IsNullOrEmpty(entity.Description) ? selectedGenre.Description : entity.Description;
                _dbContext.SaveChanges();
                isSuccess = true;
            }
        }

        private Genre GetGenreById(string id)
        {
            if (int.TryParse(id, out int validGenreId))
            {
                return _dbContext.Genres.FirstOrDefault(x => x.Id.Equals(validGenreId));
            }

            return null;
        }
    }
}

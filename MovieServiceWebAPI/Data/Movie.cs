using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieServiceWebAPI.Data
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public string Status { get; set; }
        [Range(1900, 2026)]
        public int ReleaseYear { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}

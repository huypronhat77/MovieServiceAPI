using System.ComponentModel.DataAnnotations;

namespace MovieServiceWebAPI.Data
{
    public class Genre
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}

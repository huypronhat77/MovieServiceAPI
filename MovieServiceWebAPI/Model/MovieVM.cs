using System.ComponentModel.DataAnnotations;
using MovieServiceWebAPI.Data;

namespace MovieServiceWebAPI.Model
{
    public class MovieVM
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public string Status { get; set; }
        [Range(1900, 2026)]
        public int ReleaseYear { get; set; }
        public List<string> SelectedGenres { get; set; } = new List<string>();
    }
}

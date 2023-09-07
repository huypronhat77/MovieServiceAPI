using MovieServiceWebAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace MovieServiceWebAPI.Model
{
    public class GenreVM
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; } = "";
    }
}

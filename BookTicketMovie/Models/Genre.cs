using System.ComponentModel.DataAnnotations;

namespace BookTicketMovie.Models
{
    public class Genre
    {
       
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 5)]
        [Display (Name = "Tên thể loại")]
        public string name { get; set; }
        public List<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();



    }
}

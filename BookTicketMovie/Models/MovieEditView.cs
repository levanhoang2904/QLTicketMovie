using System.ComponentModel.DataAnnotations;

namespace BookTicketMovie.Models
{
    public class MovieEditView
    {
        public Movie Movie { get; set; }

        public List<Genre>? Genres { get; set; }

        [Display(Name = "Thể loại")]
        public List<int>? idGenres { get; set; }

       
    }
}

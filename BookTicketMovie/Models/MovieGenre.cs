namespace BookTicketMovie.Models
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int GenreId;

        public Genre Genre { get; set; }
    }
}

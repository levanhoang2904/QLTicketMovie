using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Models
{
    public class Movie
    {
       
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Tên Phim")]
        public string Title { get; set; }
        
        [Display(Name = "Ngày phát hành")]
        public DateTime ReleaseDate { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Display(Name = "Thể loại")]
        public  List<MovieGenre>? MovieGenres { get; set; }

        [Display(Name = "Thời lượng")]
        public int Time { get; set; }

        [Display(Name ="Ảnh")]
        public string? Photo {  get; set; }

        public decimal? Revenue { get; set; }
        public ICollection<Showtime>? Showtimes { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();


    }
}

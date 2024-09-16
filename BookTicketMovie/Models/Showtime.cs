using System.ComponentModel.DataAnnotations;

namespace BookTicketMovie.Models
{
    public class Showtime
    {
        public int Id { get; set; }

        [Display(Name ="Ngày chiếu"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        [Display(Name = "Tên phim")]
        public int? MovieId { get; set; }

        [Display(Name = "Phòng")]
        public int? RoomId { get; set; }

        public Movie? Movie { get; set; }
        public Room? Room { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}

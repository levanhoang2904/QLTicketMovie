using System.ComponentModel.DataAnnotations;

namespace BookTicketMovie.Models
{
    public class ShowTimeView
    {
        public List<Movie>? Movies { get; set; }

        public int? MovieId { get; set; }

        public List<Room>? Rooms { get; set; }
        [Display(Name = "Phòng")]
        public int? RoomId { get; set; }

        public Showtime? showTime {  get; set; }

        public List<DateTime>? ListDateTimes { get; set; }
    }
}

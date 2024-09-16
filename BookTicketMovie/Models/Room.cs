using System.ComponentModel.DataAnnotations;

namespace BookTicketMovie.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Phòng số")]
        public int NumberRoom { get; set; }
        [Display(Name = "Trạng thái")]
        public bool status { get; set; }

        [Display(Name = "Số lượng ghế")]
        [Required]
        public int AmountChair { get; set; }

        public ICollection<Showtime>? Showtimes { get; set; }
    }
}

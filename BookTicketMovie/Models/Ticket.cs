using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTicketMovie.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Display(Name = "Suất chiếu")]
        public int? ShowtimeId { get; set; }
        [Display(Name = "Suất chiếu")]

        public Showtime? Showtime { get; set; }

        public char ChairId { get; set; }
        [Display(Name ="Loại ghế")]
        public Chair? Chair { get; set; }

        public int MovieId { get; set; }
        [Display(Name = "Tên phim")]
        public Movie? Movie {  get; set; }

        [Display(Name = "Số ghế")]
        public string SeatNumber { get; set; }

        [Display(Name = "Ngày mua vé"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }

        [Range(0, 100000), DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:#,##0} ₫", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

    }
}

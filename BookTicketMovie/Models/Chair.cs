using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace BookTicketMovie.Models
{
    public class Chair
    {
        [Display(Name ="Mã ghế")]
        public char Id {  get; set; }

        [Display(Name = "Tên ghế")]

        public string? NameChair { get; set; }

        [Range(0, 100000), DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:#,##0} ₫", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }

    public class ChairValidator : AbstractValidator<Chair>
    {
        public ChairValidator()
        {
            RuleFor(customer => customer.NameChair).NotNull().WithMessage("Trường này bắt buộc");
        }
    }
}

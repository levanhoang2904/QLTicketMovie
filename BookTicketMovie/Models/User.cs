using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookTicketMovie.Models
{
    public class User 
    {
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string? Name { get; set; }

        public string? Email { get; set; }
        [Display(Name = "Mật khẩu")]

        public string? Password { get; set; }
        [Display(Name = "Vai trò")]
        public string? Role { get; set; } = "";
    }
}

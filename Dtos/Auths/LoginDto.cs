using System.ComponentModel.DataAnnotations;

namespace Mini_E_Commerce.Dtos.Auths
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}

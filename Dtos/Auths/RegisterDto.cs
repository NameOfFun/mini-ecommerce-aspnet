using System.ComponentModel.DataAnnotations;

namespace Mini_E_Commerce.Dtos.Auths
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}

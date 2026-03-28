namespace Mini_E_Commerce.Dtos.Auths
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}

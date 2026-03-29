using Mini_E_Commerce.Dtos.Auths;

namespace Mini_E_Commerce.Services.Interface
{
    public interface IAuthService
    {
        //Task<AuthResponseDto?> Register(RegisterDto registerDto);
        Task<(AuthResponseDto? Dto, IEnumerable<string> Errors)> RegisterError(RegisterDto registerDto);
        Task<AuthResponseDto?> Login(LoginDto loginDto);
    }
}

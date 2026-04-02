using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mini_E_Commerce.Dtos.Auths;
using Mini_E_Commerce.Models;
using Mini_E_Commerce.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mini_E_Commerce.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        public AuthService(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid) return null;

            return GenerationToken(user);
        }

        //public async Task<AuthResponseDto?> Register(RegisterDto registerDto)
        //{
        //    var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        //    if (existingUser != null) return null;

        //    var user = new AppUser
        //    {
        //        UserName = registerDto.Email,
        //        Email = registerDto.Email,
        //        FullName = registerDto.FullName,
        //    };

        //    var result = await _userManager.CreateAsync(user, registerDto.Password);

        //    if (!result.Succeeded) return null;

        //    return GenerationToken(user);
        //}

        public async Task<(AuthResponseDto? Dto, IEnumerable<string> Errors)> RegisterError(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return (null, ["Email already exists."]);

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return (null, result.Errors.Select(e => e.Description)); // ← lỗi thật

            // Gán role mặc định là "User"
            await _userManager.AddToRoleAsync(user, "User");

            return (GenerationToken(user), []);
        }

        public async Task<(AuthResponseDto? Dto, string? Error)> ResetPassWord(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded) return (null, result.Errors.First().Description);

            return (GenerationToken(user), null);
        }
        private AuthResponseDto GenerationToken(AppUser user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var expiration = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpirationHours"]!));

            // lấy roles của user
            var roles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.FullName ?? ""),
            };

            // thêm claims cho roles
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email!,
                FullName = user.FullName ?? "",
                Expiration = expiration
            };
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_E_Commerce.Dtos.Auths;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var (response, errors) = await _authService.RegisterError(dto);
            if (response == null)
                return BadRequest(errors);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.Login(dto);
            if(result == null) 
            {
                return Unauthorized("Login failed. Invalid email or password.");
            }

            return Ok(result);
        }
    }
}

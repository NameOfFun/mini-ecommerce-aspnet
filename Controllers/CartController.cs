using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_E_Commerce.Dtos.Cart;
using Mini_E_Commerce.Services.Interface;
using System.Security.Claims;

namespace Mini_E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCart(userId);
            return Ok(cart);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = GetUserId();
            var (success, message) = await _cartService.AddToCart(userId, dto);
            if (!success) return BadRequest(message);
            return Ok(message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            var userId = GetUserId();
            var (success, message) = await _cartService.UpdateCartItem(userId, dto);
            if (!success) return BadRequest(message);
            return Ok(message);
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = GetUserId();
            var (success, message) = await _cartService.RemoveFromCart(userId, cartItemId);
            if (!success) return NotFound(message);
            return Ok(message);
        }

        [HttpGet("validate-stock")]
        public async Task<IActionResult> ValidateCart()
        {
            var warnings = await _cartService.ValidateCartStock(GetUserId());
            if (!warnings.Any())
            {
                return Ok(new { IsValid = true, Message = "Cart is ready for checkout." });
            }
            return Ok(new { IsValid = false, Warnings = warnings });
        }
    }
}

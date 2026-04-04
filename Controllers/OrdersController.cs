using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_E_Commerce.Dtos.Order;
using Mini_E_Commerce.Services.Interface;
using System.Security.Claims;

namespace Mini_E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value!;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            var (order, error) = await _orderService.Checkout(GetUserId(), dto);
            if (error != null)
            {
                return BadRequest(new { message = error });
            }
            return CreatedAtAction(nameof(GetById), new {id = order.OrderId}, order);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok();
    }
}

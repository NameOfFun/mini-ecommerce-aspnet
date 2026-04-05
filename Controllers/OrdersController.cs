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
        public async Task<IActionResult> GetById(int id)
        {
            var (order, error) = await _orderService.GetOrderByIdForUser(GetUserId(), id);
            if (order == null) return NotFound(new {message = error});
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var list = await _orderService.GetMyOrders(GetUserId());
            return Ok(list);
        }
    }
}

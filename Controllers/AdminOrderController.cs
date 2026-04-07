using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_E_Commerce.Dtos.Order;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderQueryDto query)
        {
            var result = await _orderService.GetAllOrdersForAdmin(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (order, error) = await _orderService.GetOrderByIdForAdmin(id);
            if (order == null) return NotFound(new { message = error });
            return Ok(order);
        }


    }
}

using Mini_E_Commerce.Dtos.Order;

namespace Mini_E_Commerce.Services.Interface
{
    public interface IOrderService
    {
        Task<(OrderResponseDto? Order, string? Error)> Checkout(string userId, CheckoutDto dto);
    }
}

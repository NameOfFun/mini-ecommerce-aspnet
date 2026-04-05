using Mini_E_Commerce.Dtos.Common;
using Mini_E_Commerce.Dtos.Order;

namespace Mini_E_Commerce.Services.Interface
{
    public interface IOrderService
    {
        Task<(OrderResponseDto? Order, string? Error)> Checkout(string userId, CheckoutDto dto);

        // User: chỉ đơn của mình
        Task<IEnumerable<OrderSumaryDto>> GetMyOrders(string userId);
        Task<(OrderResponseDto? Order, string? Error)> GetOrderByIdForUser(string userId, int orderId);

        // Admin: tất cả đơn + chi tiết bất kỳ
        Task<PagedResultDto<OrderSumaryDto>> GetAllOrdersForAdmin(OrderQueryDto query);
        Task<(OrderResponseDto? Order, string? Error)> GetOrderByIdForAdmin(int orderId);
    }
}

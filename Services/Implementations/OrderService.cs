using Microsoft.EntityFrameworkCore;
using Mini_E_Commerce.Dtos.Order;
using Mini_E_Commerce.Models;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly EcommerceMiniContext _context;
        public const int PendingStatusId = 1; // Assuming 1 is the ID for "Pending" status in OrderStatuses table
        public OrderService(EcommerceMiniContext context)
        {
            _context = context;
        }

        public async Task<(OrderResponseDto? Order, string? Error)> Checkout(string userId, CheckoutDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems.Any())
                {
                    await transaction.RollbackAsync();
                    return (null, "Cart is empty.");
                }

                foreach (var item in cart.CartItems)
                {
                    if (item.Product == null)
                    {
                        await transaction.RollbackAsync();
                        return (null, $"Product with ID {item.ProductId} not found.");
                    }

                    if (item.Quantity > item.Product.Stock)
                    {
                        await transaction.RollbackAsync();
                        return (null, $"Not enough stock for product {item.Product.ProductName}. Available: {item.Product.Stock}, Requested: {item.Quantity}");
                    }
                }

                var snapshot = cart.CartItems.ToList();

                var order = new Order
                {
                    UserId = userId,
                    FullName = dto.FullName,
                    Address = dto.Address,
                    PaymentMethod = dto.PaymentMethod,
                    ShippingMethod = dto.ShippingMethod,
                    ShippingFee = dto.ShippingFee,
                    StatusId = PendingStatusId,
                    Note = dto.Note,
                    OrderDate = DateTime.UtcNow,
                    RequiredDate = DateTime.UtcNow,
                    EmployeeId = null
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                double itemsTotal = 0;
                foreach (var item in snapshot)
                {
                    var p = item.Product;
                    var lineDiscount = p.Discount;

                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = lineDiscount
                    };

                    _context.OrderDetails.Add(orderDetail);

                    p.Stock -= item.Quantity;
                    itemsTotal += (item.UnitPrice * item.Quantity) * (1 - lineDiscount);
                }

                _context.CartItems.RemoveRange(cart.CartItems);
                cart.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var status = await _context.OrderStatuses.AsNoTracking().FirstOrDefaultAsync(s => s.StatusId == order.StatusId);

                var response = new OrderResponseDto
                {
                    OrderId = order.OrderId,
                    StatusName = status?.StatusName ?? "Unknown",
                    OrderDate = order.OrderDate,
                    FullName = order.FullName,
                    Address = order.Address,
                    PaymentMethod = order.PaymentMethod,
                    ShippingFee = order.ShippingFee,
                    ItemsTotal = itemsTotal,
                    GrandTotal = itemsTotal + order.ShippingFee,
                    Items = cart.CartItems.Select(ci => new OrderItemResponseDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.ProductName,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.UnitPrice,
                        Discount = ci.Product?.Discount ?? 0
                    }).ToList()
                };

                return (response, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

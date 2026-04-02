using Microsoft.EntityFrameworkCore;
using Mini_E_Commerce.Dtos.Cart;
using Mini_E_Commerce.Models;
using Mini_E_Commerce.Services.Interface;

namespace Mini_E_Commerce.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly EcommerceMiniContext _context;
        public CartService(EcommerceMiniContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> AddToCart(string userId, AddToCartDto dto)
        {
            return (true, "Add Product successfully");
        }

        public async Task<CartResponseDto> GetCart(string userId)
        {
            var cart = await _context.Carts.
                AsNoTracking().
                Include(i => i.CartItems).
                    ThenInclude(ci => ci.Product).
                    FirstOrDefaultAsync(u => u.UserId == userId);

            if(cart == null)
            {
                return new CartResponseDto();
            }

            return new CartResponseDto
            {
                CartId = cart.CartId,
                Items = cart.CartItems.Select(ci => new CartItemResponseDto
                {
                    CartItemId = ci.ProductId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    Image = ci.Product.Image,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                }).ToList()
            };
        }

        public Task<(bool Success, string Message)> RemoveFromCart(string userId, int cartItemId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string Message)> UpdateCartItem(string userId, UpdateCartItemDto dto)
        {
            throw new NotImplementedException();
        }
    }
}

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
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);
            if (product == null)
            {
                return (false, "Product not found");
            }

            if (product.Stock < dto.Quantity)
            {
                return (false, $"Not enough stock. Available: {product.Stock}");
            }

            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (existingCartItem != null)
            {
                int newQty = existingCartItem.Quantity + dto.Quantity;
                if (product.Stock < newQty)
                {
                    return (false, $"Not enough stock to update quantity. Available: {product.Stock}");
                }
                existingCartItem.Quantity = newQty;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.UnitPrice ?? 0,
                    AddedAt = DateTime.UtcNow,
                });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return (true, "Add Product successfully");
        }

        public async Task<CartResponseDto> GetCart(string userId)
        {
            var cart = await _context.Carts.
                AsNoTracking().
                Include(i => i.CartItems).
                    ThenInclude(ci => ci.Product).
                    FirstOrDefaultAsync(u => u.UserId == userId);

            if (cart == null)
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
                    StockAvailable = ci.Product.Stock
                }).ToList()
            };
        }

        public async Task<(bool Success, string Message)> RemoveFromCart(string userId, int cartItemId)
        {
            var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return (false, "Cart not found");
            }

            var items = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (items == null)
            {
                return (false, "Cart item not found");
            }

            cart.CartItems.Remove(items);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return (true, "Items removed");
        }

        public async Task<(bool Success, string Message)> UpdateCartItem(string userId, UpdateCartItemDto dto)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return (false, "Cart not found");
            }

            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == dto.CartItemId);
            if (item == null)
            {
                return (false, "Cart item not found");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
            if (product == null || product.Stock < dto.Quantity)
            {
                return (false, "Not enough stock");
            }

            item.Quantity = dto.Quantity;
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return (true, "Cart item updated successfully");
        }

        public async Task<List<string>> ValidateCartStock(string userId)
        {
            var warning = new List<string>();

            var cart = await _context.Carts.AsNoTracking().Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return warning;
            }

            foreach (var item in cart.CartItems)
            {
                if (item.Product == null)
                {
                    warning.Add($"Product ID {item.ProductId} no longer exists.");
                    continue;
                }
                if (item.Product.Stock == 0)
                {
                    warning.Add($"'{item.Product.ProductName}' is out of stock");
                }
                else if (item.Quantity > item.Product.Stock)
                {
                    warning.Add($"'{item.Product.ProductName}' has only {item.Product.Stock} items left. Please adjust your quantity.");
                }
            }
            return warning;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return false;
            }

            _context.CartItems.RemoveRange(cart.CartItems);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

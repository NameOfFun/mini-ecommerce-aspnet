using Mini_E_Commerce.Dtos.Cart;

namespace Mini_E_Commerce.Services.Interface
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCart(string userId);
        Task<(bool Success, string Message)> AddToCart(string userId, AddToCartDto dto);
        Task<(bool Success, string Message)> UpdateCartItem(string userId, UpdateCartItemDto dto);
        Task<(bool Success, string Message)> RemoveFromCart(string userId, int cartItemId);
    }
}

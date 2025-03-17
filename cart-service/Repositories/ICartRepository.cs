using cart_service.Models;

namespace cart_service.Repositories;

public interface ICartRepository
{
    Task<Cart> GetCartAsync(string username);
    Task AddOrUpdateItemAsync(string username, CartItem item);
    Task RemoveItemAsync(string username, int productId);
    Task ClearCartAsync(string username);
    Task<Product> GetProductByIdAsync(int productId);
    Task<CartItem> GetCartItemAsync(string username, int productId);
}
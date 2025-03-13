using cart_service.Models;

namespace cart_service.Repositories;

public interface ICartRepository
{
    Task<Cart> GetCartAsync(string userEmail);
    Task AddOrUpdateItemAsync(string userEmail, CartItem item);
    Task RemoveItemAsync(string userEmail, int productId);
    Task ClearCartAsync(string userEmail);
    Task<Product> GetProductByIdAsync(int productId);
    Task<CartItem> GetCartItemAsync(string userEmail, int productId);
}
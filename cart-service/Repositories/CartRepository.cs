using cart_service.Data;
using cart_service.Models;
using cart_service.Repositories;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartAsync(string userEmail)
    {
        var items = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserEmail == userEmail)
            .ToListAsync();
        
        if (items == null || !items.Any()) return null;
        
        return new Cart { UserEmail = userEmail, Items = items };
    }

    public async Task AddOrUpdateItemAsync(string userEmail, CartItem item)
    {
        var existingItem = await _context.CartItems.FindAsync(userEmail, item.ProductId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            item.UserEmail = userEmail;
            await _context.CartItems.AddAsync(item);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(string userEmail, int productId)
    {
        var item = await _context.CartItems.FindAsync(userEmail, productId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string userEmail)
    {
        var items = await _context.CartItems
            .Where(ci => ci.UserEmail == userEmail)
            .ToListAsync();
            
        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }

    public async Task<CartItem> GetCartItemAsync(string userEmail, int productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserEmail == userEmail && ci.ProductId == productId);
    }
}
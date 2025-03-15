using cart_service.Data;
using cart_service.Models;
using Microsoft.EntityFrameworkCore;

namespace cart_service.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();
    public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);
    public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);
    public async Task UpdateAsync(Product product) => _context.Products.Update(product);
    public async Task DeleteAsync(int id) => _context.Products.Remove(await GetByIdAsync(id));
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public async Task UpdateStockQuantityAsync(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) throw new Exception("Product not found");

        product.StockQuantity = quantity;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
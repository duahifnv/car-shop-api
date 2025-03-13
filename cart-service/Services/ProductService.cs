using cart_service.Data;
using cart_service.Dto;
using cart_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace cart_service.Services;

public class ProductService
{
    private readonly AppDbContext _context;
    public ProductService(AppDbContext context) => _context = context;

    public async Task<List<Product>> GetAllProductsAsync() =>
        await _context.Products.ToListAsync();

    public async Task<Product> GetProductByIdAsync(int id) => 
        await _context.Products.FindAsync(id);

    public async Task<EntityEntry<Product>> CreateProductAsync(ProductDto productDto)
    {
        var product = new Product()
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity,
            CategoryId = productDto.CategoryId
        };
        return await _context.Products.AddAsync(product);
    }
    
    public async Task UpdateProductAsync(Product product) => 
        _context.Products.Update(product);

    public async Task DeleteProductAsync(int id) => 
        _context.Products.Remove(await GetProductByIdAsync(id));
}
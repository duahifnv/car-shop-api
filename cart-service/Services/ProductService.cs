using cart_service.Data;
using cart_service.Models;
using cart_service.Repositories;

namespace cart_service.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository) => 
        _productRepository = productRepository;

    public async Task<List<Product>> GetAllProductsAsync() => 
        await _productRepository.GetAllAsync();

    public async Task<Product> GetProductByIdAsync(int id) => 
        await _productRepository.GetByIdAsync(id);
    
    public async Task CreateProductAsync(Product product) =>
        await _productRepository.AddAsync(product);
    
    public async Task UpdateProductAsync(Product product) => 
        await _productRepository.UpdateAsync(product);

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
    } 
}
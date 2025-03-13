using AutoMapper;
using cart_service.Dto;
using cart_service.Models;
using cart_service.Repositories;

public class ProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) throw new Exception("Product not found");

        _mapper.Map(updateProductDto, product);
        await _repository.UpdateAsync(product);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}
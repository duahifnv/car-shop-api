using AutoMapper;
using cart_service.Dto;
using cart_service.Models;
using cart_service.Repositories;

namespace cart_service.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Category> CreateCategoryAsync(CategoryRequest categoryRequest)
    {
        var category = _mapper.Map<Category>(categoryRequest);
        _logger.LogInformation("Created new category: {category}", category.Name);
        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Category {category} saved to database", category.Name);
        return category;
    }

    public async Task UpdateCategoryAsync(int id, CategoryRequest categoryRequest)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) throw new Exception("Category not found");
        
        _mapper.Map(categoryRequest, category);
        await _repository.UpdateAsync(category);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Category {category} updated in database", category.Name);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) throw new Exception("Category not found");
        
        _logger.LogInformation("Category {category} deleted from database", category.Name);
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}
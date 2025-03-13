using AutoMapper;
using cart_service.Dto;
using cart_service.Models;
using cart_service.Repositories;

namespace cart_service.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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
        await _repository.AddAsync(category);
        return category;
    }

    public async Task UpdateCategoryAsync(int id, CategoryRequest categoryRequest)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) throw new Exception("Category not found");

        _mapper.Map(categoryRequest, category);
        await _repository.UpdateAsync(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) throw new Exception("Category not found");

        await _repository.DeleteAsync(id);
    }
}
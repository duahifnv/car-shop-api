using cart_service.Models;

namespace cart_service.Repositories;

public interface ICategoryRepository
{
    Task<Category> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}
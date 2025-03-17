using desktop_client.Services;

public class AdminViewModel
{
    private readonly ApiService _apiService;

    public AdminViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<CartResponse>> GetCartsAsync()
    {
        return await _apiService.GetCartsAsync();
    }

    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        return await _apiService.GetProductsAsync();
    }

    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        return await _apiService.GetCategoriesAsync();
    }
}
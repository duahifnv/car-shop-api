using desktop_client.Services;

namespace desktop_client.ViewModels;

public class UserViewModel
{
    private readonly ApiService _apiService;
    private readonly string _userEmail;

    public UserViewModel(ApiService apiService, string userEmail)
    {
        _apiService = apiService;
        _userEmail = userEmail;
    }

    public async Task<CartResponse> GetMyCartAsync()
    {
        return await _apiService.GetCartAsync(_userEmail);
    }

    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        return await _apiService.GetProductsAsync();
    }
}
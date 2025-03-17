using desktop_client.Services;

namespace desktop_client.ViewModels;

public class LoginViewModel
{
    private readonly ApiService _apiService;

    public LoginViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var token = await _apiService.AuthenticateAsync(email, password);
        return token;
    }

    public async Task<string> RegisterAsync(string email, string password, string role)
    {
        var token = await _apiService.RegisterAsync(email, password, role);
        return token;
    }
}
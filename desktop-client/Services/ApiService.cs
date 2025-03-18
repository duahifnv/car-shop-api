using System.Net.Http;
using System.Net.Http.Json;

namespace desktop_client.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:9000") };
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/auth/login", new { email, password });
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadAsStringAsync();
        return token;
    }

    public async Task<string> RegisterAsync(string email, string password, string role)
    {
        var response = await _httpClient.PostAsJsonAsync("/auth/register", new { email, password, role });
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadAsStringAsync();
        return token;
    }
    
    // Метод для получения всех корзин
    public async Task<List<CartResponse>> GetCartsAsync()
    {
        var response = await _httpClient.GetAsync("/api/cart");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CartResponse>>();
    }

    // Метод для получения всех продуктов
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("/api/products");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
    }

    // Метод для получения всех категорий
    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("/api/categories");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
    }

    // Метод для получения корзины пользователя
    public async Task<CartResponse> GetCartAsync(string userEmail)
    {
        var response = await _httpClient.GetAsync($"/api/cart/{userEmail}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CartResponse>();
    }

    // Метод для добавления товара в корзину
    public async Task AddToCartAsync(string userEmail, CartItemResponse item)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/cart/{userEmail}/items", item);
        response.EnsureSuccessStatusCode();
    }

    // Метод для удаления товара из корзины
    public async Task RemoveFromCartAsync(string userEmail, int productId)
    {
        var response = await _httpClient.DeleteAsync($"/api/cart/{userEmail}/items/{productId}");
        response.EnsureSuccessStatusCode();
    }

    // Метод для очистки корзины
    public async Task ClearCartAsync(string userEmail)
    {
        var response = await _httpClient.DeleteAsync($"/api/cart/{userEmail}");
        response.EnsureSuccessStatusCode();
    }
}
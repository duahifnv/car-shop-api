using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;
using System.Windows.Controls;
using desktop_client.Services;
using desktop_client.ViewModels;

namespace desktop_client.Views;

public partial class LoginView : UserControl
{
    private readonly MainWindow _mainWindow;
    private readonly LoginViewModel _viewModel;

    public LoginView(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _viewModel = new LoginViewModel(new ApiService());
        DataContext = _viewModel;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var email = EmailTextBox.Text;
        var password = PasswordBox.Password;

        try
        {
            var token = await _viewModel.LoginAsync(email, password);

            // Проверяем роль пользователя
            var role = GetRoleFromToken(token);

            if (role == "Admin")
            {
                _mainWindow.ShowAdminView(token);
            }
            else
            {
                _mainWindow.ShowUserView(token, email);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        var email = EmailTextBox.Text;
        var password = PasswordBox.Password;
        var role = "User"; // По умолчанию регистрируем как пользователя

        try
        {
            var token = await _viewModel.RegisterAsync(email, password, role);
            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private static string GetRoleFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        // Проверяем, может ли токен быть прочитан
        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid token");
        }
        // Читаем токен
        var jwtToken = handler.ReadJwtToken(token);
        // Извлекаем роль из токена
        var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");
        if (roleClaim == null)
        {
            throw new ArgumentException("Role claim not found in token");
        }
        return roleClaim.Value;
    }
}
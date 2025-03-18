using System.Windows;

namespace desktop_client.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowLoginView(); // По умолчанию показываем форму входа
    }

    // Метод для отображения формы входа
    public void ShowLoginView()
    {
        var loginView = new LoginView(this);
        MainContent.Content = loginView;
    }
    
    // Метод для отображения панели администратора
    public void ShowAdminView(string token)
    {
        var adminView = new AdminView(token);
        MainContent.Content = adminView;
    }
    
    // Метод для отображения панели пользователя
    public void ShowUserView(string token, string userEmail)
    {
        var userView = new UserView(token, userEmail);
        MainContent.Content = userView;
    }
}
using System.Windows;
using System.Windows.Controls;
using desktop_client.Services;
using desktop_client.ViewModels;

namespace desktop_client.Views;

public partial class UserView : UserControl
{
    private readonly UserViewModel _viewModel;

    public UserView(string token, string userEmail)
    {
        InitializeComponent();
        _viewModel = new UserViewModel(new ApiService(), userEmail);
        DataContext = _viewModel;
        LoadData();
    }

    private async void LoadData()
    {
        var cart = await _viewModel.GetMyCartAsync();
        var products = await _viewModel.GetProductsAsync();

        // Привязка данных к DataGrid
        CartDataGrid.ItemsSource = cart.Items;
        ProductsDataGrid.ItemsSource = products;
    }
}
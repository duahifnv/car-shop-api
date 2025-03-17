using System.Windows;
using System.Windows.Controls;
using desktop_client.Services;

public partial class AdminView
{
    private readonly AdminViewModel _viewModel;

    public AdminView(string token)
    {
        InitializeComponent();
        _viewModel = new AdminViewModel(new ApiService());
        DataContext = _viewModel;
        LoadData();
    }

    private async void LoadData()
    {
        var carts = await _viewModel.GetCartsAsync();
        var products = await _viewModel.GetProductsAsync();
        var categories = await _viewModel.GetCategoriesAsync();

        // Привязка данных к DataGrid
        CartsDataGrid.ItemsSource = carts;
        ProductsDataGrid.ItemsSource = products;
        CategoriesDataGrid.ItemsSource = categories;
    }
}
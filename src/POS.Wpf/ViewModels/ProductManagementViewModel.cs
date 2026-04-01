using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Abstractions;
using POS.Application.Models;
using POS.Wpf.Windows;

namespace POS.Wpf.ViewModels;

public partial class ProductManagementViewModel : ObservableObject
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ProductManagementViewModel(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    [ObservableProperty]
    private ObservableCollection<CategoryDto> _categories = new();

    [ObservableProperty]
    private ObservableCollection<ProductListItemDto> _products = new();

    [ObservableProperty]
    private ProductListItemDto? _selectedProduct;

    [ObservableProperty]
    private string _newCategoryName = "";

    public async Task LoadAsync()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var catalog = scope.ServiceProvider.GetRequiredService<IProductCatalogService>();
        var cats = await catalog.GetCategoriesAsync();
        Categories = new ObservableCollection<CategoryDto>(cats);
        var list = await catalog.SearchProductsAsync(null);
        Products = new ObservableCollection<ProductListItemDto>(list);
    }

    [RelayCommand]
    private async Task AddCategoryAsync()
    {
        if (string.IsNullOrWhiteSpace(NewCategoryName))
            return;

        await using var scope = _scopeFactory.CreateAsyncScope();
        var catalog = scope.ServiceProvider.GetRequiredService<IProductCatalogService>();
        try
        {
            var c = await catalog.CreateCategoryAsync(NewCategoryName.Trim());
            Categories.Add(c);
            NewCategoryName = "";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    [RelayCommand]
    private async Task AddProductAsync()
    {
        if (Categories.Count == 0)
        {
            MessageBox.Show("Create a category first.", "POS", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var edit = new ProductEditDto
        {
            CategoryId = Categories[0].Id,
            InitialStock = 0,
            Price = 0,
            Cost = 0
        };
        var dlg = new Windows.ProductEditWindow(edit, Categories.ToList()) { Owner = System.Windows.Application.Current.MainWindow };
        if (dlg.ShowDialog() != true)
            return;

        await using var scope = _scopeFactory.CreateAsyncScope();
        var catalog = scope.ServiceProvider.GetRequiredService<IProductCatalogService>();
        try
        {
            await catalog.CreateProductAsync(dlg.Result!);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    [RelayCommand]
    private async Task EditProductAsync()
    {
        if (SelectedProduct is null)
            return;

        await using var scope = _scopeFactory.CreateAsyncScope();
        var catalog = scope.ServiceProvider.GetRequiredService<IProductCatalogService>();
        var existing = await catalog.GetProductForEditAsync(SelectedProduct.Id);
        if (existing is null)
        {
            MessageBox.Show("Product not found.", "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dlg = new ProductEditWindow(existing, Categories.ToList()) { Owner = System.Windows.Application.Current.MainWindow };
        if (dlg.ShowDialog() != true)
            return;

        try
        {
            await catalog.UpdateProductAsync(dlg.Result!);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    [RelayCommand]
    private async Task DeleteProductAsync()
    {
        if (SelectedProduct is null)
            return;

        if (MessageBox.Show($"Delete '{SelectedProduct.Name}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        await using var scope = _scopeFactory.CreateAsyncScope();
        var catalog = scope.ServiceProvider.GetRequiredService<IProductCatalogService>();
        try
        {
            await catalog.DeleteProductAsync(SelectedProduct.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

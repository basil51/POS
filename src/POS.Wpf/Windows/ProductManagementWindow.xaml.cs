using System.Windows;
using POS.Wpf.ViewModels;

namespace POS.Wpf.Windows;

public partial class ProductManagementWindow : Window
{
    public ProductManagementWindow(ProductManagementViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        Loaded += async (_, _) => await viewModel.LoadAsync();
    }
}

using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using POS.Application.Models;
using POS.Wpf.Converters;

namespace POS.Wpf.Windows;

public partial class ProductEditWindow : Window
{
    private readonly ProductEditDto _model;
    private static readonly FilePathToImageConverter _imgConverter = new();

    public ProductEditWindow(ProductEditDto model, IReadOnlyList<CategoryDto> categories)
    {
        InitializeComponent();
        _model = model;
        Title = model.Id == Guid.Empty ? "Add Product" : "Edit Product";

        // Bind simple fields
        NameBox.Text    = model.Name;
        BarcodeBox.Text = model.Barcode ?? "";
        PriceBox.Text   = model.Price.ToString("N2", CultureInfo.InvariantCulture);
        CostBox.Text    = model.Cost.ToString("N2", CultureInfo.InvariantCulture);
        StockBox.Text   = model.InitialStock.ToString("N2", CultureInfo.InvariantCulture);
        ActiveCheck.IsChecked = model.IsActive;

        // Categories
        CategoryBox.ItemsSource    = categories;
        CategoryBox.SelectedValue  = model.CategoryId;

        // Image
        if (!string.IsNullOrWhiteSpace(model.ImagePath) && File.Exists(model.ImagePath))
            ProductImagePreview.Source =
                (BitmapImage?)_imgConverter.Convert(model.ImagePath, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
    }

    public ProductEditDto? Result { get; private set; }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            MessageBox.Show("Product name is required.", "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (CategoryBox.SelectedValue is not Guid catId)
        {
            MessageBox.Show("Select a category.", "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!decimal.TryParse(PriceBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) || price < 0)
        {
            MessageBox.Show("Enter a valid price.", "POS", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        decimal.TryParse(CostBox.Text,  NumberStyles.Any, CultureInfo.InvariantCulture, out var cost);
        decimal.TryParse(StockBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var stock);

        Result = new ProductEditDto
        {
            Id           = _model.Id,
            Name         = NameBox.Text.Trim(),
            Barcode      = string.IsNullOrWhiteSpace(BarcodeBox.Text) ? null : BarcodeBox.Text.Trim(),
            CategoryId   = catId,
            Price        = price,
            Cost         = cost,
            InitialStock = stock < 0 ? 0 : stock,
            ImagePath    = _model.ImagePath,
            IsActive     = ActiveCheck.IsChecked == true
        };
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) =>
        DialogResult = false;

    private void BrowseImage_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            Title  = "Select product image",
            Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp|All files|*.*"
        };
        if (dlg.ShowDialog() != true) return;

        _model.ImagePath = dlg.FileName;
        ProductImagePreview.Source =
            (BitmapImage?)_imgConverter.Convert(dlg.FileName, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
    }

    private void ClearImage_Click(object sender, RoutedEventArgs e)
    {
        _model.ImagePath = null;
        ProductImagePreview.Source = null;
    }
}

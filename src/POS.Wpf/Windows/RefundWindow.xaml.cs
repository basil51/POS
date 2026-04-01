using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Core.Enums;
using POS.Infrastructure.Data;

namespace POS.Wpf.Windows;

public partial class RefundWindow : Window
{
    private readonly IServiceScopeFactory _scopeFactory;

    public Guid? SelectedInvoiceId { get; private set; }

    public RefundWindow(IServiceScopeFactory scopeFactory)
    {
        InitializeComponent();
        _scopeFactory = scopeFactory;
        Loaded += async (_, _) => await LoadRecentInvoicesAsync();
    }

    private async Task LoadRecentInvoicesAsync()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<PosDbContext>();

        var invoices = await db.Invoices
            .AsNoTracking()
            .Where(i => i.Status == InvoiceStatus.Paid && !i.IsDeleted)
            .OrderByDescending(i => i.UpdatedAt)
            .Take(50)
            .Select(i => new RefundInvoiceRow
            {
                InvoiceId     = i.Id,
                InvoiceNumber = i.Id.ToString("N").Substring(0, 12).ToUpperInvariant(),
                PaidAt        = i.UpdatedAt.ToLocalTime().ToString("yyyy-MM-dd  HH:mm"),
                Total         = i.TotalAmount
            })
            .ToListAsync();

        InvoiceList.ItemsSource = invoices;
    }

    private void InvoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefundButton.IsEnabled = InvoiceList.SelectedItem is RefundInvoiceRow;
    }

    private void Refund_Click(object sender, RoutedEventArgs e)
    {
        if (InvoiceList.SelectedItem is not RefundInvoiceRow row) return;

        var confirm = MessageBox.Show(
            $"Refund invoice {row.InvoiceNumber}?\n\nTotal: {row.Total:N2}\nPaid: {row.PaidAt}\n\nStock will be restored.",
            "Confirm Refund", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes) return;

        SelectedInvoiceId = row.InvoiceId;
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) =>
        DialogResult = false;
}

internal sealed class RefundInvoiceRow
{
    public Guid   InvoiceId     { get; set; }
    public string InvoiceNumber { get; set; } = "";
    public string PaidAt        { get; set; } = "";
    public decimal Total        { get; set; }
}

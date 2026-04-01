using CommunityToolkit.Mvvm.ComponentModel;

namespace POS.Wpf.ViewModels;

/// <summary>Represents one open invoice shown as a tab in the cashier screen.</summary>
public partial class InvoiceTab : ObservableObject
{
    public Guid InvoiceId { get; set; }

    [ObservableProperty] private string _label    = "";
    [ObservableProperty] private int    _itemCount;
    [ObservableProperty] private bool   _isActive;
    [ObservableProperty] private bool   _isHeld;
}

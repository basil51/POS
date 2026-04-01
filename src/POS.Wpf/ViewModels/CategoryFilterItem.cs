using CommunityToolkit.Mvvm.ComponentModel;

namespace POS.Wpf.ViewModels;

public partial class CategoryFilterItem : ObservableObject
{
    public Guid? CategoryId { get; set; }
    public string Name { get; set; } = "";

    [ObservableProperty]
    private bool _isSelected;
}

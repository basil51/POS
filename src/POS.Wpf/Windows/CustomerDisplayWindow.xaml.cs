using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using POS.Application.Models;

namespace POS.Wpf.Windows;

public partial class CustomerDisplayWindow : Window
{
    private readonly DispatcherTimer _clock = new() { Interval = TimeSpan.FromSeconds(1) };

    public CustomerDisplayWindow()
    {
        InitializeComponent();
        _clock.Tick += (_, _) => TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
        _clock.Start();
        TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Called by MainViewModel whenever CartLines or Total changes.
    /// Thread-safe — dispatches to UI thread.
    /// </summary>
    public void Update(IReadOnlyList<CartLineDto> lines, decimal total)
    {
        Dispatcher.BeginInvoke(() =>
        {
            if (lines.Count == 0)
            {
                IdlePanel.Visibility  = Visibility.Visible;
                LinesList.Visibility  = Visibility.Collapsed;
                TotalText.Text = "0.00";
            }
            else
            {
                IdlePanel.Visibility  = Visibility.Collapsed;
                LinesList.Visibility  = Visibility.Visible;
                LinesList.ItemsSource = new ObservableCollection<CartLineDto>(lines);
                TotalText.Text = total.ToString("N2");
            }
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        _clock.Stop();
        base.OnClosed(e);
    }
}

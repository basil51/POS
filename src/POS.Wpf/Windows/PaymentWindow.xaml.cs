using System.Globalization;
using System.Windows;

namespace POS.Wpf.Windows;

public partial class PaymentWindow : Window
{
    private readonly decimal _totalDue;

    public decimal CashTendered { get; private set; }

    public PaymentWindow(decimal totalDue)
    {
        InitializeComponent();
        _totalDue = totalDue;
        TotalLabel.Text  = totalDue.ToString("N2", CultureInfo.CurrentCulture);
        ChangeLabel.Text = "0.00";
        Loaded += (_, _) => CashBox.Focus();
    }

    private void CashBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (decimal.TryParse(CashBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out var received))
        {
            var change = received - _totalDue;
            ChangeLabel.Text = change >= 0
                ? change.ToString("N2", CultureInfo.CurrentCulture)
                : "—";
        }
        else
        {
            ChangeLabel.Text = "—";
        }
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(CashBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out var received)
            || received < _totalDue)
        {
            MessageBox.Show("Cash received must be ≥ total due.", "POS",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        CashTendered = received;
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) =>
        DialogResult = false;
}

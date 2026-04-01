using System.Windows;
using POS.Wpf.ViewModels;

namespace POS.Wpf.Windows;

public partial class LoginWindow : Window
{
    public LoginWindow(LoginViewModel viewModel)
    {
        DataContext = viewModel;
        viewModel.Owner = this;
        InitializeComponent();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.Password = PasswordBox.Password;
    }
}

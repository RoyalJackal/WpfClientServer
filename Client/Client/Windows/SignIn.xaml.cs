using System;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Windows;

public partial class SignIn : Window
{
    public SignIn()
    {
        InitializeComponent();
    }
    
    private async void SignIn_OnLoaded(object sender, RoutedEventArgs e)
    {
        var viewModel = new SignInViewModel();
        await viewModel.LoadCompaniesAsync();
        DataContext = viewModel;
    }

    private async void Company_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
        await ((SignInViewModel) DataContext).LoadCompanyUsersAsync();

    private async void SignInButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (await ((SignInViewModel) DataContext).SignInAsync(false))
        {
            DialogResult = true;
            Close();
        }
    }
    
    private async void CompanySignInButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (await ((SignInViewModel) DataContext).SignInAsync(true))
        {
            DialogResult = true;
            Close();
        }
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.Source is TabControl)
        {
            ((SignInViewModel) DataContext)?.Clear();
        }
    }
}
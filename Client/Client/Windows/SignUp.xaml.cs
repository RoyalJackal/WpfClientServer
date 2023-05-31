using System;
using System.Windows;
using Client.ViewModels;

namespace Client.Windows;

public partial class SignUp : Window
{
    public SignUp()
    {
        InitializeComponent();
    }

    private async void SignUp_OnLoaded(object sender, RoutedEventArgs e)
     {
         var viewModel = new SignUpViewModel();
         await viewModel.LoadCompaniesAsync();
         DataContext = viewModel;
     }

    private async void SignUpButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (await ((SignUpViewModel) DataContext).SignUpAsync())
        {
            DialogResult = true;
            Close();
        }
    }

    
}
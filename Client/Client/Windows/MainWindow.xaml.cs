using System;
using System.Windows;
using Client.ViewModels;

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowViewModel();
            SetSignedIn(await viewModel.UpdateUserInfoAsync());
            DataContext = viewModel;
        }

        private async void SignIn_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SignIn();
            if (dialog.ShowDialog() == true)
                SetSignedIn(await ((MainWindowViewModel) DataContext).UpdateUserInfoAsync());
        }

        private async void SignUp_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SignUp();
            if (dialog.ShowDialog() == true)
                SetSignedIn(await ((MainWindowViewModel) DataContext).UpdateUserInfoAsync());
        }

        private async void SignOut_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowViewModel) DataContext;
            viewModel.SignOut();
            SetSignedIn(await viewModel.UpdateUserInfoAsync());
        }

        private void SetSignedIn(bool isSignedIn)
        {
            if (isSignedIn)
            {
                SignIn.Visibility = Visibility.Collapsed;
                SignUp.Visibility = Visibility.Collapsed;
                SignOut.Visibility = Visibility.Visible;
            }
            else
            {
                SignIn.Visibility = Visibility.Visible;
                SignUp.Visibility = Visibility.Visible;
                SignOut.Visibility = Visibility.Collapsed;
            }
        }
    }
}
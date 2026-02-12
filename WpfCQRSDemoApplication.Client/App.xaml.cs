using System.Windows;
using WpfCQRSDemo.ViewModels;
using WpfCQRSDemo.Views;

namespace WpfCQRSDemo
{
    public partial class App : Application
    {
        private System.IServiceProvider _serviceProvider;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new Bootstrapper();
            _serviceProvider = bootstrapper.ConfigureServices();
            var mainWindow = new MainWindow();
            var viewModel = (TicTacToeViewModel)_serviceProvider.GetService(typeof(TicTacToeViewModel));
            var view = new TicTacToeView();
            view.DataContext = viewModel;
            mainWindow.Content = view;
            mainWindow.Title = "WPF CQRS Demo - Tic Tac Toe";
            mainWindow.Height = 380;
            mainWindow.Width = 340;
            mainWindow.Show();
            await viewModel.InitializeAsync();
        }
    }
}

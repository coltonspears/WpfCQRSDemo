using System.Windows;
using System.Threading.Tasks;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowMessageAsync(string message)
        {
            MessageBox.Show(message);
            return Task.FromResult(true);
        }

        public Task<bool> ShowConfirmationAsync(string message, string title = "Confirm")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return Task.FromResult(result == MessageBoxResult.Yes);
        }
    }
}

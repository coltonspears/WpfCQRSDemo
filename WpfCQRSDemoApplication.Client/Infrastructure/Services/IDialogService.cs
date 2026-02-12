using System.Threading.Tasks;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message);
        Task ShowErrorAsync(string title, string message);
        Task<bool> ShowConfirmationAsync(string message, string title = "Confirm");
    }
}

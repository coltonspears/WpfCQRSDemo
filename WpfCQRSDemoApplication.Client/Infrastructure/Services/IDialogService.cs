using System.Threading.Tasks;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message);
        Task<bool> ShowConfirmationAsync(string message, string title = "Confirm");
    }
}

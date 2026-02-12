using System.Threading.Tasks;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string viewName, object parameter = null);
        void GoBack();
    }
}

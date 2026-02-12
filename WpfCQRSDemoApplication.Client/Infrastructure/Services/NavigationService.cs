using System.Threading.Tasks;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public class NavigationService : INavigationService
    {
        public Task NavigateToAsync(string viewName, object parameter = null)
        {
            return Task.CompletedTask;
        }

        public void GoBack()
        {
        }
    }
}

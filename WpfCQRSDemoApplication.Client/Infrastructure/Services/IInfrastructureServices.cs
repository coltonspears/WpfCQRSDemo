using WpfCQRSDemo.Infrastructure.CQRS;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface IInfrastructureServices
    {
        ILogger Logger { get; }
        IErrorService ErrorService { get; }
        IDialogService DialogService { get; }
        INavigationService NavigationService { get; }
        ICommandQueryDispatcher Dispatcher { get; }
    }
}

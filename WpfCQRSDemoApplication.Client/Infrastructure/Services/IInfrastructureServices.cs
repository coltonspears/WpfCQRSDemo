using WpfCQRSDemo.Infrastructure.CQRS;
using WpfCQRSDemoApplication.Client.Infrastructure.SignalR;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface IInfrastructureServices
    {
        ILogger Logger { get; }
        IErrorService ErrorService { get; }
        IDialogService DialogService { get; }
        INavigationService NavigationService { get; }
        ICommandQueryDispatcher Dispatcher { get; }
        ISignalRService SignalR { get; }
    }
}

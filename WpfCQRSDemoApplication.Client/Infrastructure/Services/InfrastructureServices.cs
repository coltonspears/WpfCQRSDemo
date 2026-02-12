using WpfCQRSDemo.Infrastructure.CQRS;
using WpfCQRSDemoApplication.Client.Infrastructure.SignalR;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public class InfrastructureServices : IInfrastructureServices
    {
        public ILogger Logger { get; }
        public IErrorService ErrorService { get; }
        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }
        public ICommandQueryDispatcher Dispatcher { get; }
        public ISignalRService SignalR { get; }

        public InfrastructureServices(
            ILogger logger,
            IErrorService errorService,
            IDialogService dialogService,
            INavigationService navigationService,
            ICommandQueryDispatcher dispatcher,
            ISignalRService signalR)
        {
            Logger = logger;
            ErrorService = errorService;
            DialogService = dialogService;
            NavigationService = navigationService;
            Dispatcher = dispatcher;
            SignalR = signalR;
        }
    }
}

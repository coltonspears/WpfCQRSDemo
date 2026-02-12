using WpfCQRSDemo.Infrastructure.CQRS;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public class InfrastructureServices : IInfrastructureServices
    {
        public ILogger Logger { get; }
        public IErrorService ErrorService { get; }
        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }
        public ICommandQueryDispatcher Dispatcher { get; }

        public InfrastructureServices(
            ILogger logger,
            IErrorService errorService,
            IDialogService dialogService,
            INavigationService navigationService,
            ICommandQueryDispatcher dispatcher)
        {
            Logger = logger;
            ErrorService = errorService;
            DialogService = dialogService;
            NavigationService = navigationService;
            Dispatcher = dispatcher;
        }
    }
}

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WpfCQRSDemo.Infrastructure.CQRS;
using WpfCQRSDemo.Infrastructure.Remote;
using WpfCQRSDemo.Infrastructure.Services;
using WpfCQRSDemo.ViewModels;
using WpfCQRSDemo.ViewModels.Base;
using WpfCQRSDemoApplication.Client.Infrastructure.SignalR;

namespace WpfCQRSDemo
{
    public class Bootstrapper
    {
        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var serverUrl = "http://localhost:5175";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(serverUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
            services.AddSingleton(httpClient);

            services.AddSingleton<ISignalRService>(new SignalRService(serverUrl));
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IErrorService, ErrorService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IRemoteCommandExecutor, RemoteCommandExecutor>();
            services.AddSingleton<ICommandQueryDispatcher, HybridCommandQueryDispatcher>();
            services.AddSingleton<IInfrastructureServices, InfrastructureServices>();

            services.AddTransient<TicTacToeViewModel>();
            services.AddTransient<LobbyViewModel>();
            services.AddTransient<LeaderboardViewModel>();

            return services.BuildServiceProvider();
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Server.Handlers.Commands.Customers;
using WpfCQRSDemoApplication.Server.Infrastructure.Logging;
using WpfCQRSDemoApplication.Server.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Server.Handlers.Commands.TicTacToe;
using WpfCQRSDemoApplication.Server.Handlers.Queries.Customers;
using WpfCQRSDemoApplication.Server.Handlers.Queries.TicTacToe;
using WpfCQRSDemoApplication.Server.Infrastructure.Execution;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.Lobby;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.Game;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.Contracts.Queries.Lobby;
using WpfCQRSDemoApplication.Shared.Contracts.Queries.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Server;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            });

        var connectionString = Configuration.GetConnectionString("DefaultConnection")
            ?? "Server=.;Database=WpfCQRSDemo;Integrated Security=true;";
        services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(connectionString));

        // Logging (use project ILogger to avoid conflict with Microsoft.Extensions.Logging.ILogger)
        services.AddSingleton<WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger, Logger>();

        // Command Executor
        services.AddScoped<ICommandExecutor, CommandExecutor>();

        // SignalR Connection Manager
        services.AddSingleton<IConnectionManager, ConnectionManager>();

        // Tic Tac Toe (legacy - kept for backwards compatibility)
        services.AddSingleton<ITicTacToeBoardStore, TicTacToeBoardStore>();
        services.AddScoped<IQueryHandler<GetBoardQuery, BoardStatusDto>, GetBoardQueryHandler>();
        services.AddScoped<IQueryHandler<GetSquareQuery, string>, GetSquareQueryHandler>();
        services.AddScoped<ICommandHandler<Shared.Contracts.Commands.TicTacToe.PutSquareCommand, BoardStatusDto>, PutSquareCommandHandler>();
        services.AddScoped<ICommandHandler<Shared.Contracts.Commands.TicTacToe.ResetBoardCommand>, ResetBoardCommandHandler>();

        // Game Store (new multi-game support)
        services.AddSingleton<IGameStore, GameStore>();

        // Game Handlers (new multi-game support)
        services.AddScoped<ICommandHandler<Shared.Contracts.Commands.Game.PutSquareCommand, PutSquareResultDto>, 
            WpfCQRSDemoApplication.Server.Handlers.Game.GameHandlers>();
        services.AddScoped<ICommandHandler<ResetGameCommand>, 
            WpfCQRSDemoApplication.Server.Handlers.Game.GameHandlers>();
        services.AddScoped<ICommandHandler<LeaveGameCommand>, 
            WpfCQRSDemoApplication.Server.Handlers.Game.GameHandlers>();
        services.AddScoped<IQueryHandler<GetGameStateQuery, GameStateDto>, 
            WpfCQRSDemoApplication.Server.Handlers.Game.GameHandlers>();

        // Lobby
        services.AddSingleton<WpfCQRSDemoApplication.Server.Domain.Lobby.ILobbyStore, WpfCQRSDemoApplication.Server.Domain.Lobby.LobbyStore>();
        services.AddScoped<ICommandHandler<CreateLobbyCommand, CreateLobbyResultDto>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();
        services.AddScoped<ICommandHandler<JoinLobbyCommand, JoinLobbyResultDto>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();
        services.AddScoped<ICommandHandler<LeaveLobbyCommand>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();
        services.AddScoped<ICommandHandler<SetPlayerReadyCommand>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();
        services.AddScoped<ICommandHandler<StartGameCommand, GameStartedDto>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();
        services.AddScoped<IQueryHandler<GetLobbyStatusQuery, LobbyDto>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();
        services.AddScoped<IQueryHandler<GetLobbyListQuery, LobbyListDto>, WpfCQRSDemoApplication.Server.Handlers.Lobby.LobbyHandlers>();

        // Leaderboard
        services.AddScoped<IQueryHandler<WpfCQRSDemoApplication.Shared.Contracts.Queries.Leaderboard.GetLeaderboardQuery, WpfCQRSDemoApplication.Shared.DTOs.Leaderboard.LeaderboardDto>, WpfCQRSDemoApplication.Server.Handlers.Leaderboard.LeaderboardHandlers>();

        // Query Handlers (Customers - require DB)
        services.AddScoped<IQueryHandler<GetAllCustomersQuery, List<CustomerListDto>>,
            GetAllCustomersQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerDto>,
            GetCustomerByIdQueryHandler>();

        // Command Handlers (Customers - require DB)
        services.AddScoped<ICommandHandler<CreateCustomerCommand, int>,
            CreateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerCommand>,
            UpdateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCustomerCommand>,
            DeleteCustomerCommandHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<LiveUpdateHub>("/hubs/live");
        });
    }
}

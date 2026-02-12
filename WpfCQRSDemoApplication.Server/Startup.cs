using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Server.Handlers.Commands.Customers;
using WpfCQRSDemoApplication.Server.Infrastructure.Logging;
using WpfCQRSDemoApplication.Server.Handlers.Commands.TicTacToe;
using WpfCQRSDemoApplication.Server.Handlers.Queries.Customers;
using WpfCQRSDemoApplication.Server.Handlers.Queries.TicTacToe;
using WpfCQRSDemoApplication.Server.Infrastructure.Execution;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;
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

        // Tic Tac Toe
        services.AddSingleton<ITicTacToeBoardStore, TicTacToeBoardStore>();
        services.AddScoped<IQueryHandler<GetBoardQuery, BoardStatusDto>, GetBoardQueryHandler>();
        services.AddScoped<IQueryHandler<GetSquareQuery, string>, GetSquareQueryHandler>();
        services.AddScoped<ICommandHandler<PutSquareCommand, BoardStatusDto>, PutSquareCommandHandler>();
        services.AddScoped<ICommandHandler<ResetBoardCommand>, ResetBoardCommandHandler>();

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
        });
    }
}
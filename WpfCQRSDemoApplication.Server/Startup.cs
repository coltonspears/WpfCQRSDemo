using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using WpfCQRSDemoApplication.Server.Handlers.Commands.Customers;
using WpfCQRSDemoApplication.Server.Handlers.Queries.Customers;
using WpfCQRSDemoApplication.Server.Infrastructure.Execution;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;

namespace WpfCQRSDemoApplication.Server;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            });

        // Database
        services.AddScoped<IDbConnection>(sp => 
            new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

        // Logging
        services.AddSingleton<ILogger, Logger>();

        // Command Executor
        services.AddScoped<ICommandExecutor, CommandExecutor>();

        // Query Handlers
        services.AddScoped<IQueryHandler<GetAllCustomersQuery, List<CustomerListDto>>, 
            GetAllCustomersQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerDto>, 
            GetCustomerByIdQueryHandler>();

        // Command Handlers
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
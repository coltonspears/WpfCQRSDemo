using System.Data;
using Dapper;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.Customers;

public class CreateCustomerCommandHandler 
    : ICommandHandler<CreateCustomerCommand, int>
{
    private readonly IDbConnection _dbConnection;
    private readonly AppLogger _logger;

    public CreateCustomerCommandHandler(IDbConnection dbConnection, AppLogger logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public async Task<int> HandleAsync(CreateCustomerCommand command)
    {
        _logger.Info($"Creating customer: {command.Name}");

        var sql = @"
            INSERT INTO Customers (Name, Email, Phone, IsActive, CreatedDate)
            VALUES (@Name, @Email, @Phone, 1, GETUTCDATE());
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var customerId = await _dbConnection.ExecuteScalarAsync<int>(sql, new
        {
            command.Name,
            command.Email,
            command.Phone
        });

        _logger.Info($"Customer created with ID: {customerId}");
        return customerId;
    }
}
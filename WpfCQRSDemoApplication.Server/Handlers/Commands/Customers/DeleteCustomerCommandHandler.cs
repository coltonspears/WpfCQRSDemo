using System.Data;
using Dapper;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.Customers;

public class DeleteCustomerCommandHandler 
    : ICommandHandler<DeleteCustomerCommand>
{
    private readonly IDbConnection _dbConnection;
    private readonly AppLogger _logger;

    public DeleteCustomerCommandHandler(IDbConnection dbConnection, AppLogger logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteCustomerCommand command)
    {
        _logger.Info($"Deleting customer ID: {command.CustomerId}");

        // Soft delete
        var sql = @"
            UPDATE Customers
            SET 
                IsActive = 0,
                ModifiedDate = GETUTCDATE()
            WHERE Id = @CustomerId";

        var rowsAffected = await _dbConnection.ExecuteAsync(
            sql, 
            new { CustomerId = command.CustomerId });

        if (rowsAffected == 0)
            throw new InvalidOperationException(
                $"Customer {command.CustomerId} not found");

        _logger.Info($"Customer {command.CustomerId} deleted");
    }
}

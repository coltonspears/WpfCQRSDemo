using System.Data;
using Dapper;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.Customers;

public class UpdateCustomerCommandHandler 
    : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IDbConnection _dbConnection;
    private readonly AppLogger _logger;

    public UpdateCustomerCommandHandler(IDbConnection dbConnection, AppLogger logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public async Task HandleAsync(UpdateCustomerCommand command)
    {
        _logger.Info($"Updating customer ID: {command.CustomerId}");

        var sql = @"
            UPDATE Customers
            SET 
                Name = @Name,
                Email = @Email,
                Phone = @Phone,
                ModifiedDate = GETUTCDATE()
            WHERE Id = @CustomerId";

        var rowsAffected = await _dbConnection.ExecuteAsync(sql, new
        {
            command.CustomerId,
            command.Name,
            command.Email,
            command.Phone
        });

        if (rowsAffected == 0)
            throw new InvalidOperationException(
                $"Customer {command.CustomerId} not found");

        _logger.Info($"Customer {command.CustomerId} updated");
    }
}
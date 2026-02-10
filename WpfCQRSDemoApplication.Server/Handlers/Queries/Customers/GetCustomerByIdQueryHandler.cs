using System.Data;
using Dapper;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;

namespace WpfCQRSDemoApplication.Server.Handlers.Queries.Customers;

public class GetCustomerByIdQueryHandler 
    : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IDbConnection _dbConnection;

    public GetCustomerByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CustomerDto> HandleAsync(GetCustomerByIdQuery query)
    {
        var sql = @"
            SELECT 
                Id, 
                Name, 
                Email, 
                Phone,
                TotalOrders
            FROM Customers
            WHERE Id = @CustomerId";

        return await _dbConnection.QuerySingleOrDefaultAsync<CustomerDto>(
            sql, 
            new { CustomerId = query.CustomerId });
    }
}
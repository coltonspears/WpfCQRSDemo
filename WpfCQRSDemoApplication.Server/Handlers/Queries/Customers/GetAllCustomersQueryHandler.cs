using System.Data;
using Dapper;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Queries.Customers;

public class GetAllCustomersQueryHandler 
    : IQueryHandler<GetAllCustomersQuery, List<CustomerListDto>>
{
    private readonly IDbConnection _dbConnection;
    private readonly AppLogger _logger;

    public GetAllCustomersQueryHandler(IDbConnection dbConnection, AppLogger logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public async Task<List<CustomerListDto>> HandleAsync(GetAllCustomersQuery query)
    {
        _logger.Info($"Fetching customers from database. Search: {query.SearchTerm}");

        var sql = @"
            SELECT 
                Id, 
                Name, 
                Email, 
                TotalOrders
            FROM Customers
            WHERE (@SearchTerm IS NULL OR Name LIKE @SearchPattern)
                AND (@IncludeInactive = 1 OR IsActive = 1)
            ORDER BY Name";

        var customers = await _dbConnection.QueryAsync<CustomerListDto>(sql, new
        {
            SearchTerm = query.SearchTerm,
            SearchPattern = $"%{query.SearchTerm}%",
            IncludeInactive = query.IncludeInactive
        });

        return customers.ToList();
    }
}
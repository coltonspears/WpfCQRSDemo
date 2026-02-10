```
Solution: MyApplication
│
├── MyApplication.Shared/                          # Shared contracts between client and server
│   ├── Properties/
│   │   └── AssemblyInfo.cs
│   │
│   ├── Contracts/
│   │   ├── Commands/
│   │   │   ├── ICommand.cs
│   │   │   ├── ICommandHandler.cs
│   │   │   ├── Customers/
│   │   │   │   ├── CreateCustomerCommand.cs
│   │   │   │   ├── UpdateCustomerCommand.cs
│   │   │   │   └── DeleteCustomerCommand.cs
│   │   │   └── Orders/
│   │   │       ├── CreateOrderCommand.cs
│   │   │       └── UpdateOrderCommand.cs
│   │   │
│   │   ├── Queries/
│   │   │   ├── IQuery.cs
│   │   │   ├── IQueryHandler.cs
│   │   │   ├── Customers/
│   │   │   │   ├── GetAllCustomersQuery.cs
│   │   │   │   └── GetCustomerByIdQuery.cs
│   │   │   └── Orders/
│   │   │       ├── GetOrdersByCustomerQuery.cs
│   │   │       └── GetOrderByIdQuery.cs
│   │   │
│   │   └── Protocol/
│   │       ├── RemoteRequest.cs
│   │       ├── RemoteResponse.cs
│   │       └── RemoteException.cs
│   │
│   ├── DTOs/
│   │   ├── Customers/
│   │   │   ├── CustomerDto.cs
│   │   │   └── CustomerListDto.cs
│   │   └── Orders/
│   │       ├── OrderDto.cs
│   │       └── OrderListDto.cs
│   │
│   └── MyApplication.Shared.csproj
│
│
├── MyApplication.Client/                          # WPF Client Application
│   ├── Properties/
│   │   ├── AssemblyInfo.cs
│   │   └── Resources.resx
│   │
│   ├── App.xaml
│   ├── App.xaml.cs
│   │
│   ├── Views/
│   │   ├── MainWindow.xaml
│   │   ├── MainWindow.xaml.cs
│   │   │
│   │   ├── Base/
│   │   │   ├── ViewBase.cs
│   │   │   └── UserControlBase.cs
│   │   │
│   │   ├── Customers/
│   │   │   ├── CustomerListView.xaml
│   │   │   ├── CustomerListView.xaml.cs
│   │   │   ├── CustomerEditView.xaml
│   │   │   └── CustomerEditView.xaml.cs
│   │   │
│   │   └── Orders/
│   │       ├── OrderListView.xaml
│   │       ├── OrderListView.xaml.cs
│   │       ├── OrderEditView.xaml
│   │       └── OrderEditView.xaml.cs
│   │
│   ├── ViewModels/
│   │   ├── Base/
│   │   │   └── ViewModelBase.cs
│   │   │
│   │   ├── Customers/
│   │   │   ├── CustomerListViewModel.cs
│   │   │   └── CustomerEditViewModel.cs
│   │   │
│   │   └── Orders/
│   │       ├── OrderListViewModel.cs
│   │       └── OrderEditViewModel.cs
│   │
│   ├── Infrastructure/
│   │   ├── Services/
│   │   │   ├── IInfrastructureServices.cs
│   │   │   ├── InfrastructureServices.cs
│   │   │   ├── IDialogService.cs
│   │   │   ├── DialogService.cs
│   │   │   ├── INavigationService.cs
│   │   │   ├── NavigationService.cs
│   │   │   ├── IErrorService.cs
│   │   │   ├── ErrorService.cs
│   │   │   ├── ILogger.cs
│   │   │   └── Logger.cs
│   │   │
│   │   ├── CQRS/
│   │   │   ├── ICommandQueryDispatcher.cs
│   │   │   └── HybridCommandQueryDispatcher.cs
│   │   │
│   │   └── Remote/
│   │       ├── IRemoteCommandExecutor.cs
│   │       └── RemoteCommandExecutor.cs
│   │
│   ├── Handlers/                                  # Client-side only handlers
│   │   └── Commands/
│   │       └── ShowCustomerDialogCommand.cs
│   │       └── ShowCustomerDialogCommandHandler.cs
│   │
│   ├── Helpers/
│   │   ├── RelayCommand.cs
│   │   ├── AsyncRelayCommand.cs
│   │   └── TypeExtensions.cs
│   │
│   ├── Bootstrapper.cs
│   │
│   └── MyApplication.Client.csproj
│
│
├── MyApplication.Server/                          # ASP.NET Core Web API Server
│   ├── Properties/
│   │   └── launchSettings.json
│   │
│   ├── Controllers/
│   │   ├── CommandController.cs
│   │   └── HealthController.cs
│   │
│   ├── Infrastructure/
│   │   ├── Execution/
│   │   │   ├── ICommandExecutor.cs
│   │   │   └── CommandExecutor.cs
│   │   │
│   │   ├── Database/
│   │   │   ├── IDbConnectionFactory.cs
│   │   │   └── DbConnectionFactory.cs
│   │   │
│   │   └── Logging/
│   │       ├── ILogger.cs
│   │       └── Logger.cs
│   │
│   ├── Handlers/
│   │   ├── Queries/
│   │   │   ├── Customers/
│   │   │   │   ├── GetAllCustomersQueryHandler.cs
│   │   │   │   └── GetCustomerByIdQueryHandler.cs
│   │   │   └── Orders/
│   │   │       ├── GetOrdersByCustomerQueryHandler.cs
│   │   │       └── GetOrderByIdQueryHandler.cs
│   │   │
│   │   └── Commands/
│   │       ├── Customers/
│   │       │   ├── CreateCustomerCommandHandler.cs
│   │       │   ├── UpdateCustomerCommandHandler.cs
│   │       │   └── DeleteCustomerCommandHandler.cs
│   │       └── Orders/
│   │           ├── CreateOrderCommandHandler.cs
│   │           └── UpdateOrderCommandHandler.cs
│   │
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── Customer.cs
│   │   │   └── Order.cs
│   │   │
│   │   └── Repositories/
│   │       ├── ICustomerRepository.cs
│   │       ├── CustomerRepository.cs
│   │       ├── IOrderRepository.cs
│   │       └── OrderRepository.cs
│   │
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Startup.cs
│   ├── Program.cs
│   │
│   └── MyApplication.Server.csproj
│
│
├── MyApplication.Tests/                           # Unit and Integration Tests
│   ├── Client/
│   │   ├── ViewModels/
│   │   │   ├── CustomerListViewModelTests.cs
│   │   │   └── CustomerEditViewModelTests.cs
│   │   │
│   │   └── Handlers/
│   │       └── ShowCustomerDialogCommandHandlerTests.cs
│   │
│   ├── Server/
│   │   ├── Handlers/
│   │   │   ├── Queries/
│   │   │   │   └── GetAllCustomersQueryHandlerTests.cs
│   │   │   └── Commands/
│   │   │       └── CreateCustomerCommandHandlerTests.cs
│   │   │
│   │   └── Integration/
│   │       └── CommandControllerTests.cs
│   │
│   ├── Shared/
│   │   └── DTOs/
│   │       └── CustomerDtoTests.cs
│   │
│   └── MyApplication.Tests.csproj
│
│
└── MyApplication.sln
```

## MyApplication.Shared Project Files
```aiignore
MyApplication.Shared/
├── Contracts/Commands/ICommand.cs
├── Contracts/Commands/ICommandHandler.cs
├── Contracts/Commands/Customers/CreateCustomerCommand.cs
├── Contracts/Commands/Customers/UpdateCustomerCommand.cs
├── Contracts/Commands/Customers/DeleteCustomerCommand.cs

├── Contracts/Queries/IQuery.cs
├── Contracts/Queries/IQueryHandler.cs
├── Contracts/Queries/Customers/GetAllCustomersQuery.cs
├── Contracts/Queries/Customers/GetCustomerByIdQuery.cs

├── Contracts/Protocol/RemoteRequest.cs
├── Contracts/Protocol/RemoteResponse.cs
├── Contracts/Protocol/RemoteException.cs

├── DTOs/Customers/CustomerDto.cs
├── DTOs/Customers/CustomerListDto.cs
```

## MyApplication.Client Project Files
```aiignore
MyApplication.Client/
├── App.xaml
├── App.xaml.cs
├── Bootstrapper.cs

├── Views/MainWindow.xaml
├── Views/MainWindow.xaml.cs
├── Views/Base/ViewBase.cs
├── Views/Base/UserControlBase.cs
├── Views/Customers/CustomerListView.xaml
├── Views/Customers/CustomerListView.xaml.cs
├── Views/Customers/CustomerEditView.xaml
├── Views/Customers/CustomerEditView.xaml.cs

├── ViewModels/Base/ViewModelBase.cs
├── ViewModels/Customers/CustomerListViewModel.cs
├── ViewModels/Customers/CustomerEditViewModel.cs

├── Infrastructure/Services/IInfrastructureServices.cs
├── Infrastructure/Services/InfrastructureServices.cs
├── Infrastructure/Services/IDialogService.cs
├── Infrastructure/Services/DialogService.cs
├── Infrastructure/Services/INavigationService.cs
├── Infrastructure/Services/NavigationService.cs
├── Infrastructure/Services/IErrorService.cs
├── Infrastructure/Services/ErrorService.cs
├── Infrastructure/Services/ILogger.cs
├── Infrastructure/Services/Logger.cs

├── Infrastructure/CQRS/ICommandQueryDispatcher.cs
├── Infrastructure/CQRS/HybridCommandQueryDispatcher.cs

├── Infrastructure/Remote/IRemoteCommandExecutor.cs
├── Infrastructure/Remote/RemoteCommandExecutor.cs

├── Handlers/Commands/ShowCustomerDialogCommand.cs
├── Handlers/Commands/ShowCustomerDialogCommandHandler.cs

├── Helpers/RelayCommand.cs
├── Helpers/AsyncRelayCommand.cs
├── Helpers/TypeExtensions.cs
```

## MyApplication.Server Project Files
```aiignore
MyApplication.Server/
├── Program.cs
├── Startup.cs
├── appsettings.json
├── appsettings.Development.json

├── Controllers/CommandController.cs
├── Controllers/HealthController.cs

├── Infrastructure/Execution/ICommandExecutor.cs
├── Infrastructure/Execution/CommandExecutor.cs
├── Infrastructure/Database/IDbConnectionFactory.cs
├── Infrastructure/Database/DbConnectionFactory.cs
├── Infrastructure/Logging/ILogger.cs
├── Infrastructure/Logging/Logger.cs

├── Handlers/Queries/Customers/GetAllCustomersQueryHandler.cs
├── Handlers/Queries/Customers/GetCustomerByIdQueryHandler.cs
├── Handlers/Commands/Customers/CreateCustomerCommandHandler.cs
├── Handlers/Commands/Customers/UpdateCustomerCommandHandler.cs
├── Handlers/Commands/Customers/DeleteCustomerCommandHandler.cs

├── Domain/Entities/Customer.cs
├── Domain/Repositories/ICustomerRepository.cs
├── Domain/Repositories/CustomerRepository.cs
```


// ============================================================================
// 1. Shared Contracts (Shared between Client and Server)
// ============================================================================

// Shared.Contracts/Commands/IRemoteCommand.cs
public interface IRemoteCommand
{
bool ExecuteOnServer { get; } // Marker to indicate server execution
}

public interface IRemoteCommand<TResult> : IRemoteCommand
{
}

// Extend existing interfaces
public interface ICommand : IRemoteCommand { }
public interface ICommand<TResult> : IRemoteCommand<TResult> { }

// Shared.Contracts/DTOs/
public class CustomerDto
{
public int Id { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
public decimal TotalOrders { get; set; }
}

public class CustomerListDto
{
public int Id { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public decimal TotalOrders { get; set; }
}

// Shared.Contracts/Queries/
public class GetAllCustomersQuery : IQuery<List<CustomerListDto>>
{
public string SearchTerm { get; set; }
public bool IncludeInactive { get; set; }

    public bool ExecuteOnServer => true; // Execute on server
}

public class GetCustomerByIdQuery : IQuery<CustomerDto>
{
public int CustomerId { get; set; }

    public bool ExecuteOnServer => true;
}

// Shared.Contracts/Commands/
public class CreateCustomerCommand : ICommand<int>
{
public string Name { get; set; }
public string Email { get; set; }
public string Phone { get; set; }

    public bool ExecuteOnServer => true;
}

public class UpdateCustomerCommand : ICommand
{
public int CustomerId { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public string Phone { get; set; }

    public bool ExecuteOnServer => true;
}

public class DeleteCustomerCommand : ICommand
{
public int CustomerId { get; set; }

    public bool ExecuteOnServer => true;
}

// Client-side only command (doesn't execute on server)
public class ShowCustomerDialogCommand : ICommand
{
public int CustomerId { get; set; }

    public bool ExecuteOnServer => false; // Execute locally
}

// ============================================================================
// 2. Remote Communication Protocol
// ============================================================================

// Shared.Contracts/Protocol/
public class RemoteRequest
{
public Guid RequestId { get; set; }
public string CommandType { get; set; }
public string SerializedCommand { get; set; }
public DateTime Timestamp { get; set; }
}

public class RemoteResponse
{
public Guid RequestId { get; set; }
public bool Success { get; set; }
public string ResultType { get; set; }
public string SerializedResult { get; set; }
public string ErrorMessage { get; set; }
public string StackTrace { get; set; }
}

public class RemoteException : Exception
{
public string ServerStackTrace { get; set; }
    
    public RemoteException(string message, string serverStackTrace) 
        : base(message)
    {
        ServerStackTrace = serverStackTrace;
    }
}

// ============================================================================
// 3. Client-Side Remote Proxy
// ============================================================================

// Client.Infrastructure/Remote/
public interface IRemoteCommandExecutor
{
Task ExecuteAsync(ICommand command);
Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
}

public class RemoteCommandExecutor : IRemoteCommandExecutor
{
private readonly HttpClient _httpClient;
private readonly ILogger _logger;
private readonly JsonSerializerSettings _jsonSettings;

    public RemoteCommandExecutor(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    public async Task ExecuteAsync(ICommand command)
    {
        var request = CreateRequest(command);
        var response = await SendRequestAsync(request);
        
        if (!response.Success)
        {
            throw new RemoteException(response.ErrorMessage, response.StackTrace);
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
    {
        var request = CreateRequest(command);
        var response = await SendRequestAsync(request);
        
        if (!response.Success)
        {
            throw new RemoteException(response.ErrorMessage, response.StackTrace);
        }

        return DeserializeResult<TResult>(response);
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        var request = CreateRequest(query);
        var response = await SendRequestAsync(request);
        
        if (!response.Success)
        {
            throw new RemoteException(response.ErrorMessage, response.StackTrace);
        }

        return DeserializeResult<TResult>(response);
    }

    private RemoteRequest CreateRequest(object command)
    {
        return new RemoteRequest
        {
            RequestId = Guid.NewGuid(),
            CommandType = command.GetType().AssemblyQualifiedName,
            SerializedCommand = JsonConvert.SerializeObject(command, _jsonSettings),
            Timestamp = DateTime.UtcNow
        };
    }

    private async Task<RemoteResponse> SendRequestAsync(RemoteRequest request)
    {
        _logger.Info($"Sending remote request: {request.CommandType}");

        var json = JsonConvert.SerializeObject(request, _jsonSettings);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpResponse = await _httpClient.PostAsync("/api/commands/execute", content);
        httpResponse.EnsureSuccessStatusCode();

        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RemoteResponse>(responseJson, _jsonSettings);
    }

    private TResult DeserializeResult<TResult>(RemoteResponse response)
    {
        if (string.IsNullOrEmpty(response.SerializedResult))
            return default;

        return JsonConvert.DeserializeObject<TResult>(
            response.SerializedResult, 
            _jsonSettings);
    }
}

// ============================================================================
// 4. Enhanced Client Dispatcher (Routes to Server or Local)
// ============================================================================

// Client.Infrastructure/CQRS/
public class HybridCommandQueryDispatcher : ICommandQueryDispatcher
{
private readonly IServiceProvider _serviceProvider;
private readonly IRemoteCommandExecutor _remoteExecutor;
private readonly ILogger _logger;

    public HybridCommandQueryDispatcher(
        IServiceProvider serviceProvider,
        IRemoteCommandExecutor remoteExecutor,
        ILogger logger)
    {
        _serviceProvider = serviceProvider;
        _remoteExecutor = remoteExecutor;
        _logger = logger;
    }

    public async Task ExecuteAsync(ICommand command)
    {
        if (command.ExecuteOnServer)
        {
            _logger.Info($"Executing {command.GetType().Name} on server");
            await _remoteExecutor.ExecuteAsync(command);
        }
        else
        {
            _logger.Info($"Executing {command.GetType().Name} locally");
            await ExecuteLocalCommandAsync(command);
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
    {
        if (command.ExecuteOnServer)
        {
            _logger.Info($"Executing {command.GetType().Name} on server");
            return await _remoteExecutor.ExecuteAsync(command);
        }
        else
        {
            _logger.Info($"Executing {command.GetType().Name} locally");
            return await ExecuteLocalCommandAsync<TResult>(command);
        }
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        if (query.ExecuteOnServer)
        {
            _logger.Info($"Executing {query.GetType().Name} on server");
            return await _remoteExecutor.QueryAsync(query);
        }
        else
        {
            _logger.Info($"Executing {query.GetType().Name} locally");
            return await ExecuteLocalQueryAsync(query);
        }
    }

    private async Task ExecuteLocalCommandAsync(ICommand command)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException(
                $"No local handler found for {command.GetType().Name}");
        
        await handler.HandleAsync((dynamic)command);
    }

    private async Task<TResult> ExecuteLocalCommandAsync<TResult>(ICommand<TResult> command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(commandType, typeof(TResult));
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException(
                $"No local handler found for {commandType.Name}");
        
        return await handler.HandleAsync((dynamic)command);
    }

    private async Task<TResult> ExecuteLocalQueryAsync<TResult>(IQuery<TResult> query)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(queryType, typeof(TResult));
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException(
                $"No local handler found for {queryType.Name}");
        
        return await handler.HandleAsync((dynamic)query);
    }
}

// ============================================================================
// 5. Server-Side Implementation
// ============================================================================

// Server/Controllers/CommandController.cs
[ApiController]
[Route("api/commands")]
public class CommandController : ControllerBase
{
private readonly ICommandExecutor _commandExecutor;
private readonly ILogger _logger;

    public CommandController(ICommandExecutor commandExecutor, ILogger logger)
    {
        _commandExecutor = commandExecutor;
        _logger = logger;
    }

    [HttpPost("execute")]
    public async Task<ActionResult<RemoteResponse>> Execute([FromBody] RemoteRequest request)
    {
        _logger.Info($"Received request {request.RequestId}: {request.CommandType}");

        try
        {
            var result = await _commandExecutor.ExecuteAsync(request);
            
            return Ok(new RemoteResponse
            {
                RequestId = request.RequestId,
                Success = true,
                ResultType = result?.GetType().AssemblyQualifiedName,
                SerializedResult = result != null 
                    ? JsonConvert.SerializeObject(result) 
                    : null
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error executing request {request.RequestId}");
            
            return Ok(new RemoteResponse
            {
                RequestId = request.RequestId,
                Success = false,
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
}

// Server/Infrastructure/CommandExecutor.cs
public interface ICommandExecutor
{
Task<object> ExecuteAsync(RemoteRequest request);
}

public class CommandExecutor : ICommandExecutor
{
private readonly IServiceProvider _serviceProvider;
private readonly ILogger _logger;
private readonly JsonSerializerSettings _jsonSettings;

    public CommandExecutor(IServiceProvider serviceProvider, ILogger logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        
        _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    public async Task<object> ExecuteAsync(RemoteRequest request)
    {
        // Deserialize the command
        var commandType = Type.GetType(request.CommandType);
        if (commandType == null)
            throw new InvalidOperationException($"Unknown command type: {request.CommandType}");

        var command = JsonConvert.DeserializeObject(
            request.SerializedCommand, 
            commandType, 
            _jsonSettings);

        // Determine if it's a command or query and execute
        if (typeof(IQuery<>).IsAssignableFromGeneric(commandType))
        {
            return await ExecuteQueryAsync((dynamic)command, commandType);
        }
        else if (typeof(ICommand<>).IsAssignableFromGeneric(commandType))
        {
            return await ExecuteCommandWithResultAsync((dynamic)command, commandType);
        }
        else if (typeof(ICommand).IsAssignableFrom(commandType))
        {
            await ExecuteCommandAsync((dynamic)command);
            return null;
        }
        else
        {
            throw new InvalidOperationException(
                $"Type {commandType.Name} is not a valid command or query");
        }
    }

    private async Task ExecuteCommandAsync(ICommand command)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException(
                $"No handler registered for {command.GetType().Name}");
        
        await handler.HandleAsync((dynamic)command);
    }

    private async Task<object> ExecuteCommandWithResultAsync(
        dynamic command, 
        Type commandType)
    {
        var resultType = commandType.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
            .GetGenericArguments()[0];

        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(commandType, resultType);
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException(
                $"No handler registered for {commandType.Name}");
        
        return await handler.HandleAsync(command);
    }

    private async Task<object> ExecuteQueryAsync(dynamic query, Type queryType)
    {
        var resultType = queryType.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>))
            .GetGenericArguments()[0];

        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(queryType, resultType);
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException(
                $"No handler registered for {queryType.Name}");
        
        return await handler.HandleAsync(query);
    }
}

// Helper extension
public static class TypeExtensions
{
public static bool IsAssignableFromGeneric(this Type genericType, Type type)
{
return type.GetInterfaces().Any(i =>
i.IsGenericType && i.GetGenericTypeDefinition() == genericType);
}
}

// ============================================================================
// 6. Server-Side Handlers (with Database Access)
// ============================================================================

// Server/Handlers/Queries/
public class GetAllCustomersQueryHandler
: IQueryHandler<GetAllCustomersQuery, List<CustomerListDto>>
{
private readonly IDbConnection _dbConnection;
private readonly ILogger _logger;

    public GetAllCustomersQueryHandler(IDbConnection dbConnection, ILogger logger)
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

// Server/Handlers/Commands/
public class CreateCustomerCommandHandler
: ICommandHandler<CreateCustomerCommand, int>
{
private readonly IDbConnection _dbConnection;
private readonly ILogger _logger;

    public CreateCustomerCommandHandler(IDbConnection dbConnection, ILogger logger)
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

public class UpdateCustomerCommandHandler
: ICommandHandler<UpdateCustomerCommand>
{
private readonly IDbConnection _dbConnection;
private readonly ILogger _logger;

    public UpdateCustomerCommandHandler(IDbConnection dbConnection, ILogger logger)
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

public class DeleteCustomerCommandHandler
: ICommandHandler<DeleteCustomerCommand>
{
private readonly IDbConnection _dbConnection;
private readonly ILogger _logger;

    public DeleteCustomerCommandHandler(IDbConnection dbConnection, ILogger logger)
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

// ============================================================================
// 7. Client-Side Local Handler Example
// ============================================================================

// Client/Handlers/Commands/
public class ShowCustomerDialogCommandHandler
: ICommandHandler<ShowCustomerDialogCommand>
{
private readonly IDialogService _dialogService;
private readonly ICommandQueryDispatcher _dispatcher;

    public ShowCustomerDialogCommandHandler(
        IDialogService dialogService,
        ICommandQueryDispatcher dispatcher)
    {
        _dialogService = dialogService;
        _dispatcher = dispatcher;
    }

    public async Task HandleAsync(ShowCustomerDialogCommand command)
    {
        // This executes locally - fetches data from server, shows dialog locally
        var query = new GetCustomerByIdQuery { CustomerId = command.CustomerId };
        var customer = await _dispatcher.QueryAsync(query);

        if (customer != null)
        {
            await _dialogService.ShowMessageAsync(
                $"Customer: {customer.Name}\nEmail: {customer.Email}");
        }
    }
}

// ============================================================================
// 8. Server Startup Configuration
// ============================================================================

// Server/Startup.cs
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

// Server/Program.cs
public class Program
{
public static void Main(string[] args)
{
CreateHostBuilder(args).Build().Run();
}

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://localhost:5000");
            });
}

// ============================================================================
// 9. Client Startup Configuration (Updated)
// ============================================================================

// Client/Bootstrapper.cs
public class Bootstrapper
{
public IServiceProvider ConfigureServices()
{
var services = new ServiceCollection();

        // HttpClient for server communication
        services.AddHttpClient<IRemoteCommandExecutor, RemoteCommandExecutor>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Infrastructure Services
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<IErrorService, ErrorService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INavigationService>(sp => 
            new NavigationService(sp, App.Current.MainWindow.GetFrame()));

        // CQRS - Use Hybrid Dispatcher
        services.AddSingleton<ICommandQueryDispatcher, HybridCommandQueryDispatcher>();

        // Facade
        services.AddSingleton<IInfrastructureServices, InfrastructureServices>();

        // Local Command Handlers (client-side only)
        services.AddTransient<ICommandHandler<ShowCustomerDialogCommand>, 
            ShowCustomerDialogCommandHandler>();

        // ViewModels
        services.AddTransient<CustomerListViewModel>();
        services.AddTransient<CustomerEditViewModel>();

        return services.BuildServiceProvider();
    }
}

// ============================================================================
// 10. ViewModel Usage (Unchanged - just works!)
// ============================================================================

public class CustomerListViewModel : ViewModelBase
{
// No changes needed! The ViewModel doesn't know if commands execute
// locally or remotely - it's all handled by the dispatcher
    
    public CustomerListViewModel(IInfrastructureServices infrastructure)
        : base(infrastructure)
    {
        LoadCustomersCommand = new AsyncRelayCommand(LoadCustomersAsync);
        DeleteCustomerCommand = new AsyncRelayCommand<CustomerListDto>(DeleteCustomerAsync);
        ShowDetailsCommand = new AsyncRelayCommand<CustomerListDto>(ShowDetailsAsync);
    }

    private async Task LoadCustomersAsync()
    {
        // This query executes on the server
        var query = new GetAllCustomersQuery
        {
            SearchTerm = SearchTerm,
            IncludeInactive = false
        };

        var customers = await ExecuteQueryAsync(
            query,
            errorMessage: "Failed to load customers",
            busyMessage: "Loading customers...");

        if (customers != null)
        {
            Customers = new ObservableCollection<CustomerListDto>(customers);
        }
    }

    private async Task DeleteCustomerAsync(CustomerListDto customer)
    {
        if (customer == null) return;

        var confirmed = await DialogService.ShowConfirmationAsync(
            $"Are you sure you want to delete {customer.Name}?",
            "Confirm Delete");

        if (!confirmed) return;

        // This command executes on the server
        var command = new DeleteCustomerCommand { CustomerId = customer.Id };
        await ExecuteCommandAsync(command, "Failed to delete customer");

        await LoadCustomersAsync();
    }

    private async Task ShowDetailsAsync(CustomerListDto customer)
    {
        if (customer == null) return;

        // This command executes locally (shows dialog)
        var command = new ShowCustomerDialogCommand { CustomerId = customer.Id };
        await ExecuteCommandAsync(command);
    }

    // ... rest of ViewModel unchanged
}
```

**Key Architecture Benefits:**

1. **Transparent Remoting**: ViewModels don't know/care if commands run locally or remotely
2. **Clean Separation**: Server has database access, client has UI logic
3. **Flexible Execution**: Flag `ExecuteOnServer` determines routing
4. **Shared Contracts**: Commands/Queries/DTOs shared between client and server
5. **Type Safety**: Full compile-time type checking maintained
6. **Error Handling**: Server exceptions propagated cleanly to client
7. **Easy Testing**: Can test handlers in isolation on both sides
8. **Maintainable**: Adding new commands is straightforward

**Execution Flow:**
```
Client ViewModel
→ Dispatcher.ExecuteAsync(command)
→ Checks command.ExecuteOnServer
→ If true: RemoteExecutor sends HTTP request
→ Server receives request
→ Server CommandExecutor deserializes
→ Server Handler executes (with DB access)
→ Result serialized and returned
→ Client receives and deserializes
→ If false: Local handler executes
→ Result returned to ViewModel


// ============================================================================
// 1. CQRS Infrastructure
// ============================================================================

// Command/Query Interfaces
public interface ICommand { }
public interface ICommand<TResult> { }
public interface IQuery<TResult> { }

// Handler Interfaces
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
Task HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
Task<TResult> HandleAsync(TCommand command);
}

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
Task<TResult> HandleAsync(TQuery query);
}

// Mediator/Dispatcher
public interface ICommandQueryDispatcher
{
Task ExecuteAsync(ICommand command);
Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
}

public class CommandQueryDispatcher : ICommandQueryDispatcher
{
private readonly IServiceProvider _serviceProvider;

    public CommandQueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync(ICommand command)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"No handler found for {command.GetType().Name}");
        
        await handler.HandleAsync((dynamic)command);
    }

    public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"No handler found for {commandType.Name}");
        
        return await handler.HandleAsync((dynamic)command);
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
        dynamic handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"No handler found for {queryType.Name}");
        
        return await handler.HandleAsync((dynamic)query);
    }
}

// ============================================================================
// 2. Infrastructure Facade (Enhanced with CQRS)
// ============================================================================

public interface IInfrastructureServices
{
ILogger Logger { get; }
IErrorService ErrorService { get; }
IDialogService DialogService { get; }
INavigationService NavigationService { get; }
ICommandQueryDispatcher Dispatcher { get; }
}

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

// ============================================================================
// 3. Base ViewModel (Enhanced with CQRS helpers)
// ============================================================================

public abstract class ViewModelBase : INotifyPropertyChanged
{
protected ILogger Logger { get; }
protected IErrorService ErrorService { get; }
protected IDialogService DialogService { get; }
protected INavigationService NavigationService { get; }
protected ICommandQueryDispatcher Dispatcher { get; }

    private bool _isBusy;
    private string _busyMessage;

    protected ViewModelBase(IInfrastructureServices infrastructure)
    {
        Logger = infrastructure.Logger;
        ErrorService = infrastructure.ErrorService;
        DialogService = infrastructure.DialogService;
        NavigationService = infrastructure.NavigationService;
        Dispatcher = infrastructure.Dispatcher;
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string BusyMessage
    {
        get => _busyMessage;
        set => SetProperty(ref _busyMessage, value);
    }

    // Helper for executing commands with error handling
    protected async Task ExecuteCommandAsync(ICommand command, string errorMessage = null)
    {
        try
        {
            IsBusy = true;
            await Dispatcher.ExecuteAsync(command);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, errorMessage ?? $"Error executing {command.GetType().Name}");
            ErrorService.HandleError(ex);
            await DialogService.ShowMessageAsync(errorMessage ?? "An error occurred");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task<TResult> ExecuteCommandAsync<TResult>(
        ICommand<TResult> command, 
        string errorMessage = null)
    {
        try
        {
            IsBusy = true;
            return await Dispatcher.ExecuteAsync(command);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, errorMessage ?? $"Error executing {command.GetType().Name}");
            ErrorService.HandleError(ex);
            await DialogService.ShowMessageAsync(errorMessage ?? "An error occurred");
            return default;
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task<TResult> ExecuteQueryAsync<TResult>(
        IQuery<TResult> query, 
        string errorMessage = null,
        string busyMessage = null)
    {
        try
        {
            IsBusy = true;
            BusyMessage = busyMessage;
            return await Dispatcher.QueryAsync(query);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, errorMessage ?? $"Error executing {query.GetType().Name}");
            ErrorService.HandleError(ex);
            await DialogService.ShowMessageAsync(errorMessage ?? "An error occurred");
            return default;
        }
        finally
        {
            IsBusy = false;
            BusyMessage = null;
        }
    }

    // Lifecycle methods
    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnNavigatedToAsync(object parameter)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

// ============================================================================
// 4. Domain Models
// ============================================================================

public class Customer
{
public int Id { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
public decimal TotalOrders { get; set; }
}

public class CustomerListDto
{
public int Id { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public decimal TotalOrders { get; set; }
}

// ============================================================================
// 5. Queries
// ============================================================================

public class GetAllCustomersQuery : IQuery<List<CustomerListDto>>
{
public string SearchTerm { get; set; }
public bool IncludeInactive { get; set; }
}

public class GetCustomerByIdQuery : IQuery<Customer>
{
public int CustomerId { get; set; }
}

// Query Handlers
public class GetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, List<CustomerListDto>>
{
private readonly ICustomerRepository _repository;
private readonly ILogger _logger;

    public GetAllCustomersQueryHandler(ICustomerRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<CustomerListDto>> HandleAsync(GetAllCustomersQuery query)
    {
        _logger.Info($"Fetching customers with search term: {query.SearchTerm}");
        
        var customers = await _repository.GetAllAsync(query.SearchTerm, query.IncludeInactive);
        
        return customers.Select(c => new CustomerListDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            TotalOrders = c.TotalOrders
        }).ToList();
    }
}

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, Customer>
{
private readonly ICustomerRepository _repository;

    public GetCustomerByIdQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Customer> HandleAsync(GetCustomerByIdQuery query)
    {
        return await _repository.GetByIdAsync(query.CustomerId);
    }
}

// ============================================================================
// 6. Commands
// ============================================================================

public class CreateCustomerCommand : ICommand<int>
{
public string Name { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
}

public class UpdateCustomerCommand : ICommand
{
public int CustomerId { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
}

public class DeleteCustomerCommand : ICommand
{
public int CustomerId { get; set; }
}

// Command Handlers
public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, int>
{
private readonly ICustomerRepository _repository;
private readonly ILogger _logger;

    public CreateCustomerCommandHandler(ICustomerRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> HandleAsync(CreateCustomerCommand command)
    {
        _logger.Info($"Creating customer: {command.Name}");
        
        var customer = new Customer
        {
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone
        };

        var id = await _repository.CreateAsync(customer);
        
        _logger.Info($"Customer created with ID: {id}");
        return id;
    }
}

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
{
private readonly ICustomerRepository _repository;
private readonly ILogger _logger;

    public DeleteCustomerCommandHandler(ICustomerRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteCustomerCommand command)
    {
        _logger.Info($"Deleting customer ID: {command.CustomerId}");
        await _repository.DeleteAsync(command.CustomerId);
        _logger.Info($"Customer {command.CustomerId} deleted");
    }
}

// ============================================================================
// 7. ViewModels
// ============================================================================

public class CustomerListViewModel : ViewModelBase
{
private ObservableCollection<CustomerListDto> _customers;
private string _searchTerm;
private CustomerListDto _selectedCustomer;

    public CustomerListViewModel(IInfrastructureServices infrastructure)
        : base(infrastructure)
    {
        LoadCustomersCommand = new AsyncRelayCommand(LoadCustomersAsync);
        DeleteCustomerCommand = new AsyncRelayCommand<CustomerListDto>(DeleteCustomerAsync);
        EditCustomerCommand = new RelayCommand<CustomerListDto>(EditCustomer);
        CreateCustomerCommand = new RelayCommand(CreateCustomer);
    }

    public ObservableCollection<CustomerListDto> Customers
    {
        get => _customers;
        set => SetProperty(ref _customers, value);
    }

    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (SetProperty(ref _searchTerm, value))
            {
                // Debounce or trigger search
                _ = LoadCustomersAsync();
            }
        }
    }

    public CustomerListDto SelectedCustomer
    {
        get => _selectedCustomer;
        set => SetProperty(ref _selectedCustomer, value);
    }

    public ICommand LoadCustomersCommand { get; }
    public ICommand DeleteCustomerCommand { get; }
    public ICommand EditCustomerCommand { get; }
    public ICommand CreateCustomerCommand { get; }

    // Called when ViewModel is first initialized
    public override async Task InitializeAsync()
    {
        await LoadCustomersAsync();
    }

    private async Task LoadCustomersAsync()
    {
        var query = new GetAllCustomersQuery
        {
            SearchTerm = SearchTerm,
            IncludeInactive = false
        };

        var customers = await ExecuteQueryAsync(
            query,
            errorMessage: "Failed to load customers",
            busyMessage: "Loading customers...");

        if (customers != null)
        {
            Customers = new ObservableCollection<CustomerListDto>(customers);
        }
    }

    private async Task DeleteCustomerAsync(CustomerListDto customer)
    {
        if (customer == null) return;

        var confirmed = await DialogService.ShowConfirmationAsync(
            $"Are you sure you want to delete {customer.Name}?",
            "Confirm Delete");

        if (!confirmed) return;

        var command = new DeleteCustomerCommand { CustomerId = customer.Id };

        await ExecuteCommandAsync(command, "Failed to delete customer");

        // Refresh the list
        await LoadCustomersAsync();
    }

    private void EditCustomer(CustomerListDto customer)
    {
        if (customer == null) return;
        NavigationService.NavigateTo("CustomerEdit", customer.Id);
    }

    private void CreateCustomer()
    {
        NavigationService.NavigateTo("CustomerEdit");
    }
}

public class CustomerEditViewModel : ViewModelBase
{
private int? _customerId;
private string _name;
private string _email;
private string _phone;
private bool _isEditMode;

    public CustomerEditViewModel(IInfrastructureServices infrastructure)
        : base(infrastructure)
    {
        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        CancelCommand = new RelayCommand(Cancel);
    }

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
                ((AsyncRelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
                ((AsyncRelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }

    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    // Called when navigating to this view with a parameter
    public override async Task OnNavigatedToAsync(object parameter)
    {
        if (parameter is int customerId)
        {
            _customerId = customerId;
            IsEditMode = true;
            await LoadCustomerAsync(customerId);
        }
        else
        {
            IsEditMode = false;
            // Initialize for new customer
            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }
    }

    private async Task LoadCustomerAsync(int customerId)
    {
        var query = new GetCustomerByIdQuery { CustomerId = customerId };
        
        var customer = await ExecuteQueryAsync(
            query,
            errorMessage: "Failed to load customer",
            busyMessage: "Loading customer...");

        if (customer != null)
        {
            Name = customer.Name;
            Email = customer.Email;
            Phone = customer.Phone;
        }
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email);
    }

    private async Task SaveAsync()
    {
        if (IsEditMode)
        {
            var command = new UpdateCustomerCommand
            {
                CustomerId = _customerId.Value,
                Name = Name,
                Email = Email,
                Phone = Phone
            };

            await ExecuteCommandAsync(command, "Failed to update customer");
        }
        else
        {
            var command = new CreateCustomerCommand
            {
                Name = Name,
                Email = Email,
                Phone = Phone
            };

            var newId = await ExecuteCommandAsync(command, "Failed to create customer");
            
            if (newId > 0)
            {
                await DialogService.ShowMessageAsync("Customer created successfully");
            }
        }

        NavigationService.GoBack();
    }

    private void Cancel()
    {
        NavigationService.GoBack();
    }
}

// ============================================================================
// 8. View Initialization
// ============================================================================

// Custom Window/UserControl base for automatic ViewModel initialization
public class ViewBase : Window
{
public ViewBase()
{
Loaded += OnLoaded;
Unloaded += OnUnloaded;
}

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModelBase viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

    private async void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModelBase viewModel)
        {
            await viewModel.OnNavigatedFromAsync();
        }
    }
}

// For UserControls
public class UserControlBase : UserControl
{
public UserControlBase()
{
Loaded += OnLoaded;
Unloaded += OnUnloaded;
}

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModelBase viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

    private async void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModelBase viewModel)
        {
            await viewModel.OnNavigatedFromAsync();
        }
    }
}

// CustomerListView.xaml.cs
public partial class CustomerListView : UserControlBase
{
public CustomerListView()
{
InitializeComponent();
}
}

// Alternative: Navigation Service that handles initialization
public interface INavigationService
{
Task NavigateToAsync(string viewName, object parameter = null);
void GoBack();
}

public class NavigationService : INavigationService
{
private readonly IServiceProvider _serviceProvider;
private readonly Frame _frame;
private readonly Stack<(ViewModelBase ViewModel, object Parameter)> _navigationStack;

    public NavigationService(IServiceProvider serviceProvider, Frame frame)
    {
        _serviceProvider = serviceProvider;
        _frame = frame;
        _navigationStack = new Stack<(ViewModelBase, object)>();
    }

    public async Task NavigateToAsync(string viewName, object parameter = null)
    {
        // Resolve view and viewmodel from container
        var viewType = Type.GetType($"YourNamespace.Views.{viewName}");
        var viewModelType = Type.GetType($"YourNamespace.ViewModels.{viewName}ViewModel");

        if (viewType == null || viewModelType == null)
            throw new InvalidOperationException($"Could not resolve view or viewmodel for {viewName}");

        var view = Activator.CreateInstance(viewType) as FrameworkElement;
        var viewModel = _serviceProvider.GetService(viewModelType) as ViewModelBase;

        if (view == null || viewModel == null)
            throw new InvalidOperationException($"Could not create view or viewmodel for {viewName}");

        // Call lifecycle event on old ViewModel
        if (_frame.Content is FrameworkElement oldView && 
            oldView.DataContext is ViewModelBase oldViewModel)
        {
            await oldViewModel.OnNavigatedFromAsync();
        }

        // Set DataContext
        view.DataContext = viewModel;

        // Navigate
        _frame.Navigate(view);

        // Call lifecycle events on new ViewModel
        await viewModel.InitializeAsync();
        await viewModel.OnNavigatedToAsync(parameter);

        // Track navigation
        _navigationStack.Push((viewModel, parameter));
    }

    public void GoBack()
    {
        if (_navigationStack.Count > 1)
        {
            _navigationStack.Pop();
            var (viewModel, parameter) = _navigationStack.Peek();
            
            _frame.GoBack();
            
            // Re-initialize if needed
            _ = viewModel.OnNavigatedToAsync(parameter);
        }
    }
}

// ============================================================================
// 9. DI Container Setup
// ============================================================================

public class Bootstrapper
{
public IServiceProvider ConfigureServices()
{
var services = new ServiceCollection();

        // Infrastructure Services
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<IErrorService, ErrorService>();
        services.AddSingleton<IDialogService, DialogService>();
        
        // Navigation (register Frame from MainWindow)
        services.AddSingleton<INavigationService>(sp => 
            new NavigationService(sp, App.Current.MainWindow.GetFrame()));

        // CQRS
        services.AddSingleton<ICommandQueryDispatcher, CommandQueryDispatcher>();

        // Facade
        services.AddSingleton<IInfrastructureServices, InfrastructureServices>();

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Query Handlers
        services.AddTransient<IQueryHandler<GetAllCustomersQuery, List<CustomerListDto>>, 
            GetAllCustomersQueryHandler>();
        services.AddTransient<IQueryHandler<GetCustomerByIdQuery, Customer>, 
            GetCustomerByIdQueryHandler>();

        // Command Handlers
        services.AddTransient<ICommandHandler<CreateCustomerCommand, int>, 
            CreateCustomerCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteCustomerCommand>, 
            DeleteCustomerCommandHandler>();

        // ViewModels
        services.AddTransient<CustomerListViewModel>();
        services.AddTransient<CustomerEditViewModel>();

        return services.BuildServiceProvider();
    }
}

// ============================================================================
// 10. App.xaml.cs - Application Startup
// ============================================================================

public partial class App : Application
{
private IServiceProvider _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var bootstrapper = new Bootstrapper();
        _serviceProvider = bootstrapper.ConfigureServices();

        var mainWindow = new MainWindow();
        
        // Set initial view
        var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
        _ = navigationService.NavigateToAsync("CustomerList");
        
        mainWindow.Show();
    }
}


// 1. Infrastructure Facade - bundles common infrastructure services
public interface IInfrastructureServices
{
ILogger Logger { get; }
IErrorService ErrorService { get; }
IDialogService DialogService { get; }
INavigationService NavigationService { get; }
}

public class InfrastructureServices : IInfrastructureServices
{
public ILogger Logger { get; }
public IErrorService ErrorService { get; }
public IDialogService DialogService { get; }
public INavigationService NavigationService { get; }

    public InfrastructureServices(
        ILogger logger,
        IErrorService errorService,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        Logger = logger;
        ErrorService = errorService;
        DialogService = dialogService;
        NavigationService = navigationService;
    }
}

// 2. Register the facade in your DI container
public class Bootstrapper
{
public void ConfigureServices(IServiceCollection services)
{
// Register individual services
services.AddSingleton<ILogger, Logger>();
services.AddSingleton<IErrorService, ErrorService>();
services.AddSingleton<IDialogService, DialogService>();
services.AddSingleton<INavigationService, NavigationService>();

        // Register the facade
        services.AddSingleton<IInfrastructureServices, InfrastructureServices>();
        
        // Register ViewModels
        services.AddTransient<CustomerViewModel>();
        services.AddTransient<OrderViewModel>();
    }
}

// 3. Base ViewModel that accepts the facade
public abstract class ViewModelBase : INotifyPropertyChanged
{
protected ILogger Logger { get; }
protected IErrorService ErrorService { get; }
protected IDialogService DialogService { get; }
protected INavigationService NavigationService { get; }

    protected ViewModelBase(IInfrastructureServices infrastructure)
    {
        Logger = infrastructure.Logger;
        ErrorService = infrastructure.ErrorService;
        DialogService = infrastructure.DialogService;
        NavigationService = infrastructure.NavigationService;
    }

    // Common helper methods available to all ViewModels
    protected async Task ExecuteAsync(Func<Task> action, string errorMessage = null)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, errorMessage ?? "An error occurred");
            ErrorService.HandleError(ex);
        }
    }

    protected bool Confirm(string message, string title = "Confirm")
    {
        return DialogService.ShowConfirmation(message, title);
    }

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

// 4. Concrete ViewModels - now much cleaner!
public class CustomerViewModel : ViewModelBase
{
private readonly ICustomerRepository _customerRepository;
private readonly IEmailService _emailService;
private ObservableCollection<Customer> _customers;

    // Before: 10+ dependencies
    // After: Just 3 - the facade + domain-specific services
    public CustomerViewModel(
        IInfrastructureServices infrastructure,
        ICustomerRepository customerRepository,
        IEmailService emailService) 
        : base(infrastructure)
    {
        _customerRepository = customerRepository;
        _emailService = emailService;
        
        LoadCustomersCommand = new RelayCommand(async () => await LoadCustomersAsync());
        DeleteCustomerCommand = new RelayCommand<Customer>(async (c) => await DeleteCustomerAsync(c));
    }

    public ObservableCollection<Customer> Customers
    {
        get => _customers;
        set => SetProperty(ref _customers, value);
    }

    public ICommand LoadCustomersCommand { get; }
    public ICommand DeleteCustomerCommand { get; }

    private async Task LoadCustomersAsync()
    {
        await ExecuteAsync(async () =>
        {
            Logger.Info("Loading customers...");
            var customers = await _customerRepository.GetAllAsync();
            Customers = new ObservableCollection<Customer>(customers);
            Logger.Info($"Loaded {customers.Count} customers");
        }, "Failed to load customers");
    }

    private async Task DeleteCustomerAsync(Customer customer)
    {
        if (!Confirm($"Are you sure you want to delete {customer.Name}?"))
            return;

        await ExecuteAsync(async () =>
        {
            await _customerRepository.DeleteAsync(customer.Id);
            Customers.Remove(customer);
            Logger.Info($"Deleted customer: {customer.Name}");
        }, $"Failed to delete customer {customer.Name}");
    }
}

// 5. Another ViewModel example
public class OrderViewModel : ViewModelBase
{
private readonly IOrderRepository _orderRepository;
private readonly IInventoryService _inventoryService;
private readonly IPricingService _pricingService;

    public OrderViewModel(
        IInfrastructureServices infrastructure,
        IOrderRepository orderRepository,
        IInventoryService inventoryService,
        IPricingService pricingService)
        : base(infrastructure)
    {
        _orderRepository = orderRepository;
        _inventoryService = inventoryService;
        _pricingService = pricingService;
        
        SubmitOrderCommand = new RelayCommand(async () => await SubmitOrderAsync());
    }

    public ICommand SubmitOrderCommand { get; }

    private async Task SubmitOrderAsync()
    {
        await ExecuteAsync(async () =>
        {
            Logger.Info("Submitting order...");
            
            // Check inventory
            if (!await _inventoryService.CheckAvailabilityAsync(CurrentOrder))
            {
                await DialogService.ShowMessageAsync("Some items are out of stock");
                return;
            }

            // Calculate pricing
            var total = await _pricingService.CalculateTotalAsync(CurrentOrder);
            
            if (!Confirm($"Submit order for {total:C}?"))
                return;

            await _orderRepository.SaveAsync(CurrentOrder);
            
            await DialogService.ShowMessageAsync("Order submitted successfully");
            NavigationService.NavigateTo("OrderConfirmation");
            
        }, "Failed to submit order");
    }

    // ... other properties and methods
    private Order _currentOrder;
    public Order CurrentOrder
    {
        get => _currentOrder;
        set => SetProperty(ref _currentOrder, value);
    }
}
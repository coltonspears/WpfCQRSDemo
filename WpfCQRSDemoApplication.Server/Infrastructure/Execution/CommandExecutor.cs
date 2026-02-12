using Newtonsoft.Json;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Protocol;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Infrastructure.Execution;

public class CommandExecutor : ICommandExecutor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AppLogger _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    public CommandExecutor(IServiceProvider serviceProvider, AppLogger logger)
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
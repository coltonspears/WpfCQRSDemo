using System;
using System.Threading.Tasks;
using WpfCQRSDemo.Infrastructure.Remote;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemo.Infrastructure.CQRS
{
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
}
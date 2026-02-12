using System;
using System.Threading.Tasks;
using WpfCQRSDemo.Infrastructure.Remote;
using WpfCQRSDemo.Infrastructure.Services;
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
                _logger.Info(string.Format("Executing {0} on server", command.GetType().Name));
                await _remoteExecutor.ExecuteAsync(command);
            }
            else
            {
                _logger.Info(string.Format("Executing {0} locally", command.GetType().Name));
                await ExecuteLocalCommandAsync(command);
            }
        }

        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            if (command.ExecuteOnServer)
            {
                _logger.Info(string.Format("Executing {0} on server", command.GetType().Name));
                return await _remoteExecutor.ExecuteAsync(command);
            }
            else
            {
                _logger.Info(string.Format("Executing {0} locally", command.GetType().Name));
                return await ExecuteLocalCommandAsync<TResult>(command);
            }
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            if (query.ExecuteOnServer)
            {
                _logger.Info(string.Format("Executing {0} on server", query.GetType().Name));
                return await _remoteExecutor.QueryAsync(query);
            }
            else
            {
                _logger.Info(string.Format("Executing {0} locally", query.GetType().Name));
                return await ExecuteLocalQueryAsync(query);
            }
        }

        private async Task ExecuteLocalCommandAsync(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new InvalidOperationException(
                    string.Format("No local handler found for {0}", command.GetType().Name));

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
                    string.Format("No local handler found for {0}", commandType.Name));

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
                    string.Format("No local handler found for {0}", queryType.Name));

            return await handler.HandleAsync((dynamic)query);
        }
    }
}

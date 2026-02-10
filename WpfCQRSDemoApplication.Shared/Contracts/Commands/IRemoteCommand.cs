using System.Threading.Tasks;

namespace WpfCQRSDemoApplication.Shared.Contracts.Commands
{
    public interface IRemoteCommand
    {
        bool ExecuteOnServer { get; }
    }

    public interface IRemoteCommand<TResult> : IRemoteCommand
    {
    }

    public interface ICommand<TResult> : IRemoteCommand<TResult>
    {
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
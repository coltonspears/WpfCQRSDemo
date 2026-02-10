using System.Threading.Tasks;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemo.Infrastructure.Remote
{
    public interface IRemoteCommandExecutor
    {
        Task ExecuteAsync(ICommand command);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
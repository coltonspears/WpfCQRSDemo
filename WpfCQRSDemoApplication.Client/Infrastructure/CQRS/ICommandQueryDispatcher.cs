using System.Threading.Tasks;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemo.Infrastructure.CQRS
{
    public interface ICommandQueryDispatcher
    {
        Task ExecuteAsync(ICommand command);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}

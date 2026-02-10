using System.Threading.Tasks;

namespace WpfCQRSDemoApplication.Shared.Contracts.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
namespace WpfCQRSDemoApplication.Shared.Contracts.Commands
{
    public interface IQuery<TResult>
    {
        bool ExecuteOnServer { get; }
    }
}
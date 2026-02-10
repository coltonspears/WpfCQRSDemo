namespace WpfCQRSDemoApplication.Server.Infrastructure.Execution;

public interface ICommandExecutor
{
    Task<object> ExecuteAsync(RemoteRequest request);
}
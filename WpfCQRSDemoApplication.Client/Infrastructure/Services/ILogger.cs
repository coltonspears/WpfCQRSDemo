namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface ILogger
    {
        void Info(string message);
        void Error(System.Exception ex, string message);
    }
}

using System;
using System.Diagnostics;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public class ErrorService : IErrorService
    {
        private readonly ILogger _logger;

        public ErrorService(ILogger logger)
        {
            _logger = logger;
        }

        public void HandleError(Exception ex)
        {
            if (ex != null)
                _logger.Error(ex, ex.Message);
            Debug.WriteLine(ex != null ? ex.ToString() : "Unknown error");
        }
    }
}

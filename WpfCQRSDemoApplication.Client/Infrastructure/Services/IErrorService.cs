using System;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public interface IErrorService
    {
        void HandleError(Exception ex);
    }
}

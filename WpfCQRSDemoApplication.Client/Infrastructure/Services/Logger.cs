using System;
using System.Diagnostics;

namespace WpfCQRSDemo.Infrastructure.Services
{
    public class Logger : ILogger
    {
        public void Info(string message)
        {
            Debug.WriteLine("[INFO] " + message);
        }

        public void Error(Exception ex, string message)
        {
            Debug.WriteLine("[ERROR] " + message + ": " + (ex != null ? ex.Message : ""));
        }
    }
}

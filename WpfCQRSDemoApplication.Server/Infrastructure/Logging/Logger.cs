using System;

namespace WpfCQRSDemoApplication.Server.Infrastructure.Logging;

public class Logger : ILogger
{
    public void Info(string message)
    {
        System.Diagnostics.Debug.WriteLine($"[INFO] {message}");
        Console.WriteLine($"[INFO] {message}");
    }

    public void Error(Exception ex, string message)
    {
        var text = $"[ERROR] {message}: {ex?.Message}";
        System.Diagnostics.Debug.WriteLine(text);
        Console.WriteLine(text);
    }
}
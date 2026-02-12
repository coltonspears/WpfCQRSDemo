using Aspire.Hosting;

SetDefaultEnv("ASPNETCORE_URLS", "http://localhost:5179");
SetDefaultEnv("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL", "http://localhost:5176");
SetDefaultEnv("DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL", "http://localhost:5177");
SetDefaultEnv("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.WpfCQRSDemoApplication_Server>("server");

builder.Build().Run();

static void SetDefaultEnv(string name, string value)
{
    if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
    {
        Environment.SetEnvironmentVariable(name, value);
    }
}

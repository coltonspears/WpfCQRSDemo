using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WpfCQRSDemoApplication.Server.Infrastructure.Execution;
using WpfCQRSDemoApplication.Shared.Contracts.Protocol;

namespace WpfCQRSDemoApplication.Server.Controllers;

[ApiController]
[Route("api/commands")]
public class CommandController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly ILogger _logger;

    public CommandController(ICommandExecutor commandExecutor, ILogger logger)
    {
        _commandExecutor = commandExecutor;
        _logger = logger;
    }

    [HttpPost("execute")]
    public async Task<ActionResult<RemoteResponse>> Execute([FromBody] RemoteRequest request)
    {
        _logger.Info($"Received request {request.RequestId}: {request.CommandType}");

        try
        {
            var result = await _commandExecutor.ExecuteAsync(request);
            
            return Ok(new RemoteResponse
            {
                RequestId = request.RequestId,
                Success = true,
                ResultType = result?.GetType().AssemblyQualifiedName,
                SerializedResult = result != null 
                    ? JsonConvert.SerializeObject(result) 
                    : null
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error executing request {request.RequestId}");
            
            return Ok(new RemoteResponse
            {
                RequestId = request.RequestId,
                Success = false,
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
}
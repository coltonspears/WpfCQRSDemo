using System;
using System.Text;
using System.Threading.Tasks;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Protocol;

namespace WpfCQRSDemo.Infrastructure.Remote
{
    public class RemoteCommandExecutor : IRemoteCommandExecutor
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _jsonSettings;

        public RemoteCommandExecutor(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        
            _jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task ExecuteAsync(ICommand command)
        {
            var request = CreateRequest(command);
            var response = await SendRequestAsync(request);
        
            if (!response.Success)
            {
                throw new RemoteException(response.ErrorMessage, response.StackTrace);
            }
        }

        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var request = CreateRequest(command);
            var response = await SendRequestAsync(request);
        
            if (!response.Success)
            {
                throw new RemoteException(response.ErrorMessage, response.StackTrace);
            }

            return DeserializeResult<TResult>(response);
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            var request = CreateRequest(query);
            var response = await SendRequestAsync(request);
        
            if (!response.Success)
            {
                throw new RemoteException(response.ErrorMessage, response.StackTrace);
            }

            return DeserializeResult<TResult>(response);
        }

        private RemoteRequest CreateRequest(object command)
        {
            return new RemoteRequest
            {
                RequestId = Guid.NewGuid(),
                CommandType = command.GetType().AssemblyQualifiedName,
                SerializedCommand = JsonConvert.SerializeObject(command, _jsonSettings),
                Timestamp = DateTime.UtcNow
            };
        }

        private async Task<RemoteResponse> SendRequestAsync(RemoteRequest request)
        {
            _logger.Info($"Sending remote request: {request.CommandType}");

            var json = JsonConvert.SerializeObject(request, _jsonSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync("/api/commands/execute", content);
            httpResponse.EnsureSuccessStatusCode();

            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RemoteResponse>(responseJson, _jsonSettings);
        }

        private TResult DeserializeResult<TResult>(RemoteResponse response)
        {
            if (string.IsNullOrEmpty(response.SerializedResult))
                return default;

            return JsonConvert.DeserializeObject<TResult>(
                response.SerializedResult, 
                _jsonSettings);
        }
    }
}
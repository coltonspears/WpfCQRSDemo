using System;

namespace WpfCQRSDemoApplication.Shared.Contracts.Protocol
{
    public class RemoteResponse
    {
        public Guid RequestId { get; set; }
        public bool Success { get; set; }
        public string ResultType { get; set; }
        public string SerializedResult { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
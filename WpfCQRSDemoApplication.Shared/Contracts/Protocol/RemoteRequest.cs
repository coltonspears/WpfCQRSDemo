using System;

namespace WpfCQRSDemoApplication.Shared.Contracts.Protocol
{
    public class RemoteRequest
    {
        public Guid RequestId { get; set; }
        public string CommandType { get; set; }
        public string SerializedCommand { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
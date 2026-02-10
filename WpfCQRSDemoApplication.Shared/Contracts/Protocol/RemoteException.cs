using System;

namespace WpfCQRSDemoApplication.Shared.Contracts.Protocol
{
    public class RemoteException : Exception
    {
        public string ServerStackTrace { get; set; }
    
        public RemoteException(string message, string serverStackTrace) 
            : base(message)
        {
            ServerStackTrace = serverStackTrace;
        }
    }
}
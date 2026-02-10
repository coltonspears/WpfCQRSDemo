namespace WpfCQRSDemoApplication.Shared.Contracts.Commands
{
    public class DeleteCustomerCommand : ICommand
    {
        public int CustomerId { get; set; }
    
        public bool ExecuteOnServer => true;
    }
}
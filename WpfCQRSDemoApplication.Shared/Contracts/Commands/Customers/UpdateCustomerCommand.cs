namespace WpfCQRSDemoApplication.Shared.Contracts.Commands
{
    public class UpdateCustomerCommand : ICommand
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    
        public bool ExecuteOnServer => true;
    }
}
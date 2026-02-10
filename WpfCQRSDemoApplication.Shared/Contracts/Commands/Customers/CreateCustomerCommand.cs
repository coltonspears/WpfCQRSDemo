namespace WpfCQRSDemoApplication.Shared.Contracts.Commands
{
    public class CreateCustomerCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    
        public bool ExecuteOnServer => true;
    }
}
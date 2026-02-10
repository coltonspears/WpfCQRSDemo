using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries
{
    public class GetCustomerByIdQuery : IQuery<CustomerDto>
    {
        public int CustomerId { get; set; }
    
        public bool ExecuteOnServer => true;
    }
}
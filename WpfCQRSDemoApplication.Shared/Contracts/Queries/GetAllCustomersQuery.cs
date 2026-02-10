using System.Collections.Generic;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.DTOs.Customers;

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries
{
    public class GetAllCustomersQuery : IQuery<List<CustomerListDto>>
    {
        public string SearchTerm { get; set; }
        public bool IncludeInactive { get; set; }
    
        public bool ExecuteOnServer => true;
    }
}
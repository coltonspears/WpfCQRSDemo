namespace WpfCQRSDemoApplication.Shared.DTOs.Customers
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public decimal TotalOrders { get; set; }
    }
}
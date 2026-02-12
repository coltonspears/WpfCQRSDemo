using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries
{
    public class GetSquareQuery : IQuery<string>
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool ExecuteOnServer => true;
    }
}

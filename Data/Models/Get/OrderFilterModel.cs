using Utility.Enums;

namespace Data.Models.Get
{
    public class OrderFilterModel
    {
        public string? Name { get; set; }
        public OrderStatus? Status { get; set; }
    }
}

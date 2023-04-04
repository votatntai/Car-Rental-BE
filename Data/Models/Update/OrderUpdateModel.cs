using Utility.Enums;

namespace Data.Models.Update
{
    public class OrderUpdateModel
    {
        public string? Description { get; set; }
        public OrderStatus Status { get; set; }
    }
}

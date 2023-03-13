namespace Data.Models.Get
{
    public class TransactionFilterModel
    {
        public Guid? CustomerId { get; set; }

        public Guid? CarOwnerId { get; set; }

        public Guid? DriverId { get; set; }

        public Guid? UserId { get; set; }

    }
}

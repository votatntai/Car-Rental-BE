using Utility.Enums;

namespace Data.Models.Get
{
    public class CarFilterModel
    {
        public string? Name { get; set; }
        public LocationReqeustModel? Location{ get; set; }
        public CarPriceRequestModel? Price { get; set; }
        public bool? HasDriver { get; set; }
        public DateTime? ReceiveTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public CarType? CarType { get; set; }
        public Guid? ModelId { get; set; }
        public TransmissionType TransmissionType { get; set; }
    }
}

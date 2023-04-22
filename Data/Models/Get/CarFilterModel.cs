using Utility.Enums;

namespace Data.Models.Get
{
    public class CarFilterModel
    {
        public string? Name { get; set; }
        public CarStatus? Status { get; set; }
        public bool? IsAvailable { get; set; }
        public LocationReqeustModel? Location{ get; set; }
        public int? Distance { get; set; }
        public CarPriceRequestModel? Price { get; set; }
        public bool? HasDriver { get; set; }
        public bool? HasShowroom { get; set; }
        public CarType? CarType { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? ProductionCompanyId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TransmissionType? TransmissionType { get; set; }
    }
}

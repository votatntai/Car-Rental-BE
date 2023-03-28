﻿using Utility.Enums;

namespace Data.Models.Get
{
    public class CarFilterModel
    {
        public string? Name { get; set; }
        public LocationReqeustModel? Location{ get; set; }
        public CarPriceRequestModel? Price { get; set; }
        public bool? HasDriver { get; set; }
        public CarType? CarType { get; set; }
        public Guid? ModelId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TransmissionType? TransmissionType { get; set; }
    }
}

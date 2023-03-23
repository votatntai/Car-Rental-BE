﻿namespace Data.Models.Create
{
    public class OrderDetailCreateModel
    {
        public Guid CarId { get; set; }

        public LocationCreateModel? DeliveryLocation { get; set; }

        public LocationCreateModel? PickUpLocation { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}

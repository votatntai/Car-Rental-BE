using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Create
{
    public class PromotionCreateModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public double Discount { get; set; }

        public DateTime ExpiryAt { get; set; }
    }
}

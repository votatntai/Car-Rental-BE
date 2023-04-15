using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Create
{
    public class ShowroomCreateModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public LocationCreateModel Location { get; set; }   
    }
}

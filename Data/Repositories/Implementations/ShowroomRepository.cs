using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class ShowroomRepository : Repository<Showroom>, IShowroomRepository
    {
        public ShowroomRepository(CarRentalContext context) : base(context)
        {
        }
    }
}

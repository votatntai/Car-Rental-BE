using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class FeedBackRepository : Repository<FeedBack>, IFeedBackRepository
    {
        public FeedBackRepository(CarRentalContext context) : base(context)
        {
        }
    }
}

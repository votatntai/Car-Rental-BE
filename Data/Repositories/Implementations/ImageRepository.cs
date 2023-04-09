using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(CarRentalContext context) : base(context)
        {
        }
    }
}

using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class ModelRepository : Repository<Model>, IModelRepository
    {
        public ModelRepository(CarRentalContext context) : base(context)
        {
        }
    }
}

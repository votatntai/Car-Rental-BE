using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class ProductionCompanyRepository : Repository<ProductionCompany>, IProductionCompanyRepository
    {
        public ProductionCompanyRepository(CarRentalContext context) : base(context)
        {
        }
    }
}

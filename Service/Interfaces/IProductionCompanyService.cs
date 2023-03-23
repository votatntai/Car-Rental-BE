using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IProductionCompanyService
    {
        Task<ListViewModel<ProductionCompanyViewModel>> GetProductionCompanies(ProductionCompanyFilterModel filter, PaginationRequestModel pagination);
        Task<ProductionCompanyViewModel> GetProductionCompany(Guid id);
        Task<ProductionCompanyViewModel> CreateProductionCompany(ProductionCompanyCreateModel model);
        Task<ProductionCompanyViewModel> UpdateProductionCompany(Guid id, ProductionCompanyUpdateModel model);
        Task<bool> DeleteProductionCompany(Guid id);
    }
}

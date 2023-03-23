using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IModelService
    {
        Task<ListViewModel<CarModelViewModel>> GetModels(ModelFilterModel filter, PaginationRequestModel pagination);
        Task<CarModelViewModel> GetModel(Guid id);
        Task<CarModelViewModel> CreateModel(ModelCreateModel model);
        Task<CarModelViewModel> UpdateModel(Guid id, ModelUpdateModel model);
        Task<bool> DeleteModel(Guid id);
    }
}

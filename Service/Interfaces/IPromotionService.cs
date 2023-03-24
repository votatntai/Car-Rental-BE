using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IPromotionService
    {
        Task<ListViewModel<PromotionViewModel>> GetPromotions(PromotionFilterModel filter, PaginationRequestModel pagination);
        Task<PromotionViewModel> GetPromotion(Guid id);
        Task<PromotionViewModel> CreatePromotion(PromotionCreateModel model);
        Task<PromotionViewModel> UpdatePromotion(Guid id, PromotionUpdateModel model);
        Task<bool> DeletePromotion(Guid id);
    }
}

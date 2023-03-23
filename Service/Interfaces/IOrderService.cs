using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<ListViewModel<OrderViewModel>> GetOrders(Guid customerId, OrderFilterModel filter, PaginationRequestModel pagination);
        Task<OrderViewModel> GetOrder(Guid id);
        Task<OrderViewModel> CreateOrder(Guid customerId, OrderCreateModel model);
        Task<OrderViewModel> UpdateOrder(Guid id, OrderUpdateModel model);
    }
}

using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<ListViewModel<OrderViewModel>> GetOrders(OrderFilterModel filter, PaginationRequestModel pagination);
    }
}

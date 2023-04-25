using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<ListViewModel<OrderViewModel>> GetOrders(Guid? userId, OrderFilterModel filter, PaginationRequestModel pagination);
        Task<OrderViewModel> GetOrder(Guid id);
        Task<ListViewModel<OrderViewModel>> GetOrdersForDriver(Guid userId, PaginationRequestModel pagination);
        Task<ListViewModel<OrderViewModel>> GetOrdersForCarOwner(Guid userId, PaginationRequestModel pagination);
        Task<IActionResult> CreateOrder(Guid customerId, OrderCreateModel model);
        Task<OrderViewModel> UpdateOrderStatus(Guid id, OrderUpdateModel model);
    }
}

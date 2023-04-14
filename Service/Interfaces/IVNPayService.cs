using Utility.Helpers.Models;

namespace Service.Interfaces
{
    public interface IVNPayService
    {
        Task<bool> AddRequest(Guid customerId, VnPayRequestModel model);
        Task<bool> AddResponse(VnPayResponseModel model);
    }
}

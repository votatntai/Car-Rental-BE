using Utility.Helpers.Models;

namespace Service.Interfaces
{
    public interface IVNPayService
    {
        Task<bool> AddRequest(VnPayRequestModel model);
        Task<bool> AddResponse(VnPayResponseModel model);
    }
}

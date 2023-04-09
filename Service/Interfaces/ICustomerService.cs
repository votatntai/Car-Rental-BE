using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Http;

namespace Service.Interfaces
{
    public interface ICustomerService
    {
        Task<ListViewModel<CustomerViewModel>> GetCustomers(CustomerFilterModel filter, PaginationRequestModel pagination);
        Task<CustomerViewModel> GetCustomer(Guid id);
        Task<CustomerViewModel> CreateCustomer(CustomerCreateModel model);
        Task<CustomerViewModel> UpdateCustomer(Guid id, CustomerUpdateModel model);
        Task<ICollection<ImageViewModel>> UpdateCustomerLicenses(Guid id, ICollection<IFormFile> files);
    }
}

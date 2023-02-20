using Data.Models.Create;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerViewModel> GetCustomer(Guid id);
        Task<CustomerViewModel> CreateCustomer(CustomerCreateModel model);
        Task<CustomerViewModel> UpdateCustomer(Guid id, CustomerUpdateModel model);
    }
}

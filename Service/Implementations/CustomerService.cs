﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Enums;

namespace Service.Implementations
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private new readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _customerRepository = unitOfWork.Customer;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
            _imageRepository = unitOfWork.Image;
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
        }

        // PUBLIC METHOD

        public async Task<ListViewModel<CustomerViewModel>> GetCustomers(CustomerFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _customerRepository.GetMany(customer => filter.Name != null ? customer.Name.Contains(filter.Name) : true)
                .Include(customer => customer.Account)
                .ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider);
            var customers = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (customers != null || customers != null && customers.Any())
            {
                return new ListViewModel<CustomerViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = customers
                };
            }
            return null!;
        }

        public async Task<CustomerViewModel> GetCustomer(Guid id)
        {
            return await _customerRepository.GetMany(customer => customer.AccountId.Equals(id))
                .Include(customer => customer.Account)
                .ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CustomerViewModel> CreateCustomer(CustomerCreateModel model)
        {
            var result = 0;
            var accountId = Guid.Empty;
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    accountId = await CreateAccount(model.Username, model.Password);
                    var walletId = await CreateWallet();

                    var customer = new Customer
                    {
                        Address = model.Address,
                        Gender = model.Gender,
                        Name = model.Name,
                        Phone = model.Phone,
                        AccountId = accountId,
                        WalletId = walletId,
                    };
                    _customerRepository.Add(customer);
                    result = await _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            };
            return result > 0 ? await GetCustomer(accountId) : null!;
        }

        public async Task<CustomerViewModel> UpdateCustomer(Guid id, CustomerUpdateModel model)
        {
            var customer = await _customerRepository.GetMany(customer => customer.AccountId.Equals(id))
                .Include(customer => customer.Account)
                .FirstOrDefaultAsync();
            if (customer != null)
            {
                if (model.Name != null) customer.Name = model.Name;
                if (model.Address != null) customer.Address = model.Address;
                if (model.Gender != null) customer.Gender = model.Gender;
                if (model.BankName != null) customer.BankName = model.BankName;
                if (model.BankAccountNumber != null) customer.BankAccountNumber = model.BankAccountNumber;
                if (model.Phone != null) customer.Phone = model.Phone;
                if (model.Password != null) customer.Account.Password = model.Password;
                if (model.Status != null) customer.Account.Status = (bool)model.Status;
                _customerRepository.Update(customer);
                var result = await _unitOfWork.SaveChanges();
                return await GetCustomer(id);
            }
            return null!;
        }

        public async Task<ICollection<ImageViewModel>> UpdateCustomerLicenses(Guid id, ICollection<IFormFile> files)
        {
            var images = new List<Image>();
            foreach (IFormFile file in files)
            {
                var imageId = Guid.NewGuid();
                var url = await _cloudStorageService.Upload(imageId, file.ContentType, file.OpenReadStream());
                var image = new Image
                {
                    Id = imageId,
                    CustomerId = id,
                    Type = ImageType.License.ToString(),
                    Url = url,
                };
                images.Add(image);
            }
            _imageRepository.AddRange(images);
            return await _unitOfWork.SaveChanges() > 0 ? _mapper.Map<List<Image>, List<ImageViewModel>>(images) : null!;
        }

        // PRIVATE METHODS
        private async Task<Guid> CreateWallet()
        {
            var id = Guid.NewGuid();
            var wallet = new Wallet
            {
                Id = id,
                Balance = 0,
                Status = WalletStatus.Active.ToString(),
            };
            _walletRepository.Add(wallet);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }

        private async Task<Guid> CreateAccount(string username, string password)
        {
            var id = Guid.NewGuid();
            var account = new Account
            {
                Id = id,
                Username = username,
                Password = password,
                Status = true
            };
            _accountRepository.Add(account);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}

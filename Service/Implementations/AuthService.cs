using Data;
using Data.Entities;
using Data.Models.Get;
using Data.Models.Views;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Constants;
using Utility.Settings;

namespace Service.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly ICustomerRepository _customerRepository;

        private readonly AppSetting _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IOptions<AppSetting> appSettings) : base(unitOfWork)
        {
            _appSettings = appSettings.Value;
            _userRepository = unitOfWork.User;
            _carOwnerRepository = unitOfWork.CarOwner;
            _driverRepository = unitOfWork.Driver;
            _customerRepository = unitOfWork.Customer;
        }

        public async Task<TokenViewModel> AuthenticatedUser(AuthRequestModel model)
        {
            var user = await _userRepository.GetMany(user => user.Account.Username.Equals(model.Username) && user.Account.Password.Equals(model.Password))
                .Include(user => user.Account)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthViewModel
                {
                    Id = user.Id,
                    Role = user.Role,
                    Status = user.Account.Status,
                });
                return new TokenViewModel
                {
                    Token = token
                };
            }
            return null!;
        }

        public async Task<TokenViewModel> AuthenticatedCustomer(AuthRequestModel model)
        {
            var user = await _customerRepository.GetMany(user => user.Account.Username.Equals(model.Username) && user.Account.Password.Equals(model.Password))
                .Include(user => user.Account)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthViewModel
                {
                    Id = user.Id,
                    Role = UserRole.Customer.ToString(),
                    Status = user.Account.Status
                });
                return new TokenViewModel
                {
                    Token = token
                };
            }
            return null!;
        }

        public async Task<TokenViewModel> AuthenticatedDriver(AuthRequestModel model)
        {
            var user = await _driverRepository.GetMany(user => user.Account.Username.Equals(model.Username) && user.Account.Password.Equals(model.Password))
                .Include(user => user.Account)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthViewModel
                {
                    Id = user.Id,
                    Role = UserRole.Driver.ToString(),
                    Status = user.Account.Status
                });
                return new TokenViewModel
                {
                    Token = token
                };
            }
            return null!;
        }

        public async Task<TokenViewModel> AuthenticatedCarOwner(AuthRequestModel model)
        {
            var user = await _carOwnerRepository.GetMany(user => user.Account.Username.Equals(model.Username) && user.Account.Password.Equals(model.Password))
                .Include(user => user.Account)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthViewModel
                {
                    Id = user.Id,
                    Role = UserRole.Driver.ToString(),
                    Status = user.Account.Status
                });
                return new TokenViewModel
                {
                    Token = token
                };
            }
            return null!;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id))
                .Include(user => user.Account)
                .Include(user => user.Wallet)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                return new UserViewModel
                {
                    Id = user.Id,
                    Role = user.Role,
                    Name= user.Name,
                    Phone= user.Phone,
                    AvartarUrl= user.AvartarUrl,
                    Gender= user.Gender,
                    Wallet = new WalletViewModel
                    {
                        Id= user.Wallet.Id,
                        Balance = user.Wallet.Balance,
                        Status = user.Wallet.Status
                    }
                };
            }
            return null!;
        }

        public async Task<CustomerViewModel> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetMany(customer => customer.Id.Equals(id))
                .Include(customer => customer.Account)
                .Include(customer => customer.Wallet)
                .FirstOrDefaultAsync();
            if (customer != null)
            {
                return new CustomerViewModel
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    AvartarUrl = customer.AvartarUrl,
                    Gender = customer.Gender,
                    Address = customer.Address,
                    BankAccountNumber = customer.BankAccountNumber,
                    BankName = customer.BankName,
                    Wallet = new WalletViewModel
                    {
                        Id = customer.Wallet.Id,
                        Balance = customer.Wallet.Balance,
                        Status = customer.Wallet.Status
                    }
                };
            }
            return null!;
        }
        public async Task<DriverViewModel> GetDriverById(Guid id)
        {
            var driver = await _driverRepository.GetMany(driver => driver.Id.Equals(id))
                .Include(driver => driver.Account)
                .Include(driver => driver.Wallet)
                .FirstOrDefaultAsync();
            if (driver != null)
            {
                return new DriverViewModel
                {
                    Id = driver.Id,
                    Name = driver.Name,
                    Phone = driver.Phone,
                    AvartarUrl = driver.AvartarUrl,
                    Gender = driver.Gender,
                    Address = driver.Address,
                    BankAccountNumber = driver.BankAccountNumber,
                    BankName = driver.BankName,
                    Wallet = new WalletViewModel
                    {
                        Id = driver.Wallet.Id,
                        Balance = driver.Wallet.Balance,
                        Status = driver.Wallet.Status
                    },
                    Star = driver.Star,
                    Location = driver.Location != null ? new LocationViewModel
                    {
                        Id = driver.Location.Id,
                        Latitude = driver.Location.Latitude,
                        Longitude = driver.Location.Longitude,
                    } : null!,
                };
            }
            return null!;
        }

        public async Task<CarOwnerViewModel> GetCarOwnerById(Guid id)
        {
            var carOwner = await _carOwnerRepository.GetMany(carOwner => carOwner.Id.Equals(id))
                .Include(carOwner => carOwner.Account)
                .Include(carOwner => carOwner.Wallet)
                .FirstOrDefaultAsync();
            if (carOwner != null)
            {
                return new CarOwnerViewModel
                {
                    Id = carOwner.Id,
                    Name = carOwner.Name,
                    Phone = carOwner.Phone,
                    AvartarUrl = carOwner.AvartarUrl,
                    Gender = carOwner.Gender,
                    Address = carOwner.Address,
                    BankAccountNumber = carOwner.BankAccountNumber,
                    BankName = carOwner.BankName,
                    Wallet = new WalletViewModel
                    {
                        Id = carOwner.Wallet.Id,
                        Balance = carOwner.Wallet.Balance,
                        Status = carOwner.Wallet.Status
                    },
                };
            }
            return null!;
        }


        public async Task<AuthViewModel>AuthById(Guid id)
        {
            if(_customerRepository.Any(customer => customer.Id.Equals(id)))
            {
                return await _customerRepository.GetMany(customer => customer.Id.Equals(id)).Select(customer => new AuthViewModel
                {
                    Id = customer.Id,
                    Role = UserRole.Customer.ToString(),
                    Status = customer.Account.Status
                }).FirstOrDefaultAsync() ?? null!;
            }
            if (_driverRepository.Any(driver => driver.Id.Equals(id)))
            {
                return await _driverRepository.GetMany(driver => driver.Id.Equals(id)).Select(driver => new AuthViewModel
                {
                    Id = driver.Id,
                    Role = UserRole.Customer.ToString(),
                    Status = driver.Account.Status
                }).FirstOrDefaultAsync() ?? null!;
            }
            if (_carOwnerRepository.Any(carOwner => carOwner.Id.Equals(id)))
            {
                return await _carOwnerRepository.GetMany(carOwner => carOwner.Id.Equals(id)).Select(carOwner => new AuthViewModel
                {
                    Id = carOwner.Id,
                    Role = UserRole.Customer.ToString(),
                    Status = carOwner.Account.Status
                }).FirstOrDefaultAsync() ?? null!;
            }
            if (_userRepository.Any(user => user.Id.Equals(id)))
            {
                return await _userRepository.GetMany(user => user.Id.Equals(id)).Select(user => new AuthViewModel
                {
                    Id = user.Id,
                    Role = UserRole.Customer.ToString(),
                    Status = user.Account.Status
                }).FirstOrDefaultAsync() ?? null!;
            }
            return null!;
        }

        // PRIVATE METHODS

        private string GenerateJwtToken(AuthViewModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", model.Id.ToString()),
                    new Claim("role", model.Role),
                    new Claim("status", model.Status.ToString()),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}

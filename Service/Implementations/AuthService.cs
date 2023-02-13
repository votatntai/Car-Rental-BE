using Data;
using Data.Models.Get;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Settings;

namespace Service.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        private readonly AppSetting _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IOptions<AppSetting> appSettings) : base(unitOfWork)
        {
            _appSettings = appSettings.Value;
            _accountRepository = unitOfWork.Account;
            _userRepository = unitOfWork.User;
        }

        public async Task<AuthViewModel> AuthenticatedUser(AuthRequestModel model)
        {
            var user = await _userRepository.GetMany(user => user.Account.Username.Equals(model.Username) && user.Account.Password.Equals(model.Password))
                .Include(user => user.Account)
                .Include(user => user.Wallet)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthViewModel
                {
                    Id = user.Id,
                    Username = user.Account.Username,
                    Status = user.Account.Status,
                    Role = user.Role
                });
                return new AuthViewModel
                {
                    Id = user.Id,
                    Username = user.Account.Username,
                    Name = user.Name,
                    Role = user.Role,
                    Status = user.Account.Status,
                    Token = token
                };
            }
            return null!;
        }

        public async Task<AuthViewModel> GetUserById(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id))
                .Include(user => user.Account)
                .Include(user => user.Wallet)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                return new AuthViewModel
                {
                    Id = user.Id,
                    Username = user.Account.Username,
                    Role = user.Role,
                    Status = user.Account.Status,
                    Name= user.Name,
                };
            }
            return null!;
        }

        private string GenerateJwtToken(AuthViewModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", model.Id.ToString()),
                    new Claim("role", model.Role.ToString()),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

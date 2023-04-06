using AutoMapper;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class DeviceTokenService : BaseService, IDeviceTokenService
    {
        private new readonly IMapper _mapper;
        private readonly IDeviceTokenRepository _deviceTokenRepository;

        public DeviceTokenService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _deviceTokenRepository = unitOfWork.DeviceToken;
        }

        public async Task<bool> CreateDeviceToken(Guid userId, DeviceTokenCreateModel model)
        {
            var deviceTokens = await _deviceTokenRepository.GetMany(dvt => dvt.AccountId.Equals(userId))
            .ToListAsync();

            if (deviceTokens.Any(token => token.Token!.Equals(model.DeviceToken))) return false;

            var deviceToken = new DeviceToken
            {
                Id = Guid.NewGuid(),
                AccountId = userId,
                CreateAt = DateTime.UtcNow.AddHours(7),
                Token = model.DeviceToken,
            };

            _deviceTokenRepository.Add(deviceToken);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class ShowroomService : BaseService, IShowroomService
    {
        private new readonly IMapper _mapper;
        private readonly IShowroomRepository _showroomRepository;
        private readonly ILocationRepository _locationRepository;
        public ShowroomService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _showroomRepository = unitOfWork.Showroom;
            _locationRepository = unitOfWork.Location;
        }

        public async Task<ListViewModel<ShowroomViewModel>> GetShowrooms(ShowroomFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _showroomRepository.GetMany(pc => filter.Name == null || pc.Name != null && pc.Name.Contains(filter.Name))
            .ProjectTo<ShowroomViewModel>(_mapper.ConfigurationProvider);
            var productionCompanies = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return productionCompanies != null || productionCompanies != null && totalRow > 0 ? new ListViewModel<ShowroomViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    TotalRow = totalRow,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                },
                Data = productionCompanies
            } : null!;
        }

        public async Task<ShowroomViewModel> GetShowroom(Guid id)
        {
            return await _showroomRepository.GetMany(pc => pc.Id.Equals(id))
                .ProjectTo<ShowroomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ShowroomViewModel> CreateShowroom(ShowroomCreateModel model)
        {
            var locationId = await CreateLocation(model.Location);
            var showroom = new Showroom
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                LocationId = locationId,
            };
            _showroomRepository.Add(showroom);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetShowroom(showroom.Id) : null!;
        }

        public async Task<ShowroomViewModel> UpdateShowroom(Guid id, ShowroomUpdateModel model)
        {
            var showroom = await _showroomRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (showroom == null)
            {
                return null!;
            }
            showroom.Name = model.Name ?? showroom.Name;
            showroom.Description = model.Description ?? showroom.Description;
            _showroomRepository.Update(showroom);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetShowroom(id) : null!;
        }

        public async Task<bool> DeleteShowroom(Guid id)
        {
            var showroom = await _showroomRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (showroom == null)
            {
                return false;
            }
            _showroomRepository.Remove(showroom);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

        // PRIVATE METHODS

        private async Task<Guid> CreateLocation(LocationCreateModel model)
        {
            var id = Guid.NewGuid();
            var location = new Location
            {
                Id = id,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
            };
            _locationRepository.Add(location);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}

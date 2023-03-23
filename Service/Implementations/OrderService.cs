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
using Utility.Enums;

namespace Service.Implementations
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private new readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _orderRepository = unitOfWork.Order;
            _orderDetailRepository = unitOfWork.OrderDetail;
            _locationRepository = unitOfWork.Location;
            _mapper = mapper;
        }

        public async Task<ListViewModel<OrderViewModel>> GetOrders(Guid customerId, OrderFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _orderRepository.GetMany(order => order.CustomerId.Equals(customerId)
            && filter.Status != null ? order.Status.Equals(filter.Status) : true)
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider);
            var orders = await query.OrderBy(order => order.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (orders != null || orders != null && orders.Any())
            {
                return new ListViewModel<OrderViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = orders
                };
            }
            return null!;
        }

        public async Task<OrderViewModel> GetOrder(Guid id)
        {
            return await _orderRepository.GetMany(order => order.Id.Equals(id))
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<OrderViewModel> CreateOrder(Guid customerId, OrderCreateModel model)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Amount = 0,
                IsPaid = model.IsPaid,
                Description = model.Description,
                PromotionId = model.PromotionId,
                RentalTime = model.RentalTime,
                Status = OrderStatus.Success.ToString(),
            };
            _orderRepository.Add(order);
            foreach (var orderDetail in model.OrderDetails)
            {
                var od = new OrderDetail
                {
                    CarId = orderDetail.CarId,
                    OrderId = order.Id,
                    DeliveryLocationId = await CreateLocation(orderDetail?.DeliveryLocation!),
                    PickUpLocationId = await CreateLocation(orderDetail?.PickUpLocation!),
                };
                _orderDetailRepository.Add(od);
            }
            return await _unitOfWork.SaveChanges() > 0 ? await GetOrder(order.Id) : null!;
        }

        public async Task<OrderViewModel> UpdateOrder(Guid id, OrderUpdateModel model)
        {
            var order = await _orderRepository.GetMany(order => order.Id.Equals(id)).FirstOrDefaultAsync();
            if (order != null)
            {
                order.Status = model.Status.ToString() ?? order.Status;
                _orderRepository.Update(order);
            }
            return await _unitOfWork.SaveChanges() > 0 ? await GetOrder(id) : null!;
        }

        private async Task<Guid> CreateLocation(LocationCreateModel model)
        {
            var location = new Location
            {
                Id = Guid.NewGuid(),
                Latitude = model.Latitude,
                Longitude = model.Longitude,
            };
            _locationRepository.Add(location);
            return await _unitOfWork.SaveChanges() > 0 ? location.Id : Guid.Empty;
        }

    }
}

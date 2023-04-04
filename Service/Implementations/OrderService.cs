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
using Extensions.MyExtentions;

namespace Service.Implementations
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private new readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService) : base(unitOfWork, mapper)
        {
            _orderRepository = unitOfWork.Order;
            _driverRepository = unitOfWork.Driver;
            _userRepository = unitOfWork.User;
            _notificationService = notificationService;
            _orderDetailRepository = unitOfWork.OrderDetail;
            _locationRepository = unitOfWork.Location;
            _mapper = mapper;
        }

        public async Task<ListViewModel<OrderViewModel>> GetOrders(Guid? userId, OrderFilterModel filter, PaginationRequestModel pagination)
        {

            var query = _orderRepository.GetAll();
            if (userId != null)
            {
                if (_orderRepository.Any(order => order.Customer.AccountId.Equals(userId)))
                {
                    query = query.AsQueryable().Where(order => order.Customer.AccountId.Equals(userId));
                }
                if (_orderRepository.Any(order => order.OrderDetails.Any(od => od.DriverId.Equals(userId))))
                {
                    query = query.AsQueryable().Where(order => order.OrderDetails.Any(od => od.DriverId.Equals(userId)));
                }
                if (_orderRepository.Any(order => order.OrderDetails.Any(od => od.Car != null ? od.Car.CarOwner.AccountId.Equals(userId) : false)))
                {
                    query = query.AsQueryable().Where(order => order.OrderDetails.Any(od => od.Car != null ? od.Car.CarOwner.AccountId.Equals(userId) : false));
                }
            }
            if (filter.Status != null)
            {
                query = query.AsQueryable().Where(order => order.Status.Equals(filter.Status.ToString()));
            }
            var orders = await query.OrderBy(order => order.CreateAt)
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
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

        public async Task<ListViewModel<OrderViewModel>> GetOrdersForCarOwner(Guid userId, PaginationRequestModel pagination)
        {
            var query = _orderRepository.GetMany(order =>
                order.OrderDetails.Any(od => od.Car != null ? od.Car.CarOwner.AccountId.Equals(userId) : false)
                && order.Status.Equals(OrderStatus.Pending.ToString()) || order.Status.Equals(OrderStatus.ManagerConfirmed.ToString()))
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

        public async Task<ListViewModel<OrderViewModel>> GetOrdersForDriver(Guid userId, PaginationRequestModel pagination)
        {
            var query = _orderRepository.GetMany(order => order.OrderDetails.Any(od => od.DriverId.Equals(userId)) &&
                order.Status.Equals(OrderStatus.CarOwnerApproved.ToString()) || order.Status.Equals(OrderStatus.Ongoing.ToString()) ||
                order.Status.Equals(OrderStatus.ArrivedAtPickUpPoint.ToString()) || order.Status.Equals(OrderStatus.ReceivedGuests.ToString()) ||
                order.Status.Equals(OrderStatus.ReceivedTheCar.ToString()))
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
                Status = OrderStatus.Pending.ToString(),
                Deposit = model.Deposit,
                CreateAt = DateTime.UtcNow,
                UnitPrice = model.UnitPrice,
                DeliveryFee = model.DeliveryFee,
                DeliveryDistance = model.DeliveryDistance,
            };
            _orderRepository.Add(order);
            foreach (var orderDetail in model.OrderDetails)
            {
                var od = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    CarId = orderDetail.CarId,
                    DeliveryTime = orderDetail.DeliveryTime,
                    PickUpTime = orderDetail.PickUpTime,
                    StartTime = orderDetail.StartTime,
                    EndTime = orderDetail.EndTime,
                    OrderId = order.Id,
                    DeliveryLocationId = await CreateLocation(orderDetail?.DeliveryLocation!),
                    PickUpLocationId = await CreateLocation(orderDetail?.PickUpLocation!),
                };
                if (orderDetail != null && orderDetail.HasDriver)
                {
                    var driver = await _driverRepository.GetAll()
                        .DriverDistanceFilter(orderDetail.PickUpLocation!.Latitude, orderDetail.PickUpLocation!.Longitude, 100)
                        .FirstOrDefaultAsync();
                    if (driver != null)
                    {
                        od.DriverId = driver.AccountId;
                    }
                }
                _orderDetailRepository.Add(od);
            }
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                var message = new NotificationCreateModel
                {
                    Title = "Đơn hàng mới",
                    Body = "Bạn có đơn hàng mới cần xác nhận",
                    Data = new NotificationDataViewModel
                    {
                        CreateAt = DateTime.Now,
                        Type = NotificationType.Order.ToString(),
                        IsRead = false,
                        Link = order.Id.ToString(),
                    }
                };
                var managers = await _userRepository.GetMany(user => user.Role.Equals(UserRole.Manager.ToString()))
                    .Select(manager => manager.AccountId).ToListAsync();
                await _notificationService.SendNotification(managers, message);
                return await GetOrder(order.Id);
            }
            return null!;
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

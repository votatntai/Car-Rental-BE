﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Extensions.MyExtentions;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Enums;
using System;

namespace Service.Implementations
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;
        private new readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService) : base(unitOfWork, mapper)
        {
            _orderRepository = unitOfWork.Order;
            _driverRepository = unitOfWork.Driver;
            _carRepository = unitOfWork.Car;
            _userRepository = unitOfWork.User;
            _notificationService = notificationService;
            _orderDetailRepository = unitOfWork.OrderDetail;
            _locationRepository = unitOfWork.Location;
            _walletRepository = unitOfWork.Wallet;
            _carOwnerRepository = unitOfWork.CarOwner;
            _mapper = mapper;
        }

        public async Task<ListViewModel<OrderViewModel>> GetOrders(Guid? userId, OrderFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _orderRepository.GetAll().AsQueryable();

            if (filter.Name != null)
            {
                query = query.Where(order => filter.Name != null ? order.Customer.Name.Contains(filter.Name) : false).AsQueryable();
            }

            if (userId != null && !_userRepository.Any(user => user.AccountId.Equals(userId)))
            {
                query = query.Where(order => order.Customer.AccountId == userId ||
                                                 order.OrderDetails.Any(od => od.DriverId == userId) ||
                                                 (order.OrderDetails.Any(od => od.Car != null && od.Car.CarOwner.AccountId == userId)));
            }

            if (filter.Status != null)
            {
                query = query.AsQueryable().Where(order => order.Status == filter.Status.ToString());
            }

            var totalRow = await query.CountAsync();
            var orders = await query
                .OrderBy(order => order.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize)
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            return orders != null || orders != null && orders.Any() ? new ListViewModel<OrderViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow
                },
                Data = orders
            } : null!;
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

        public async Task<OrderViewModel> UpdateOrderStatus(Guid id, OrderUpdateModel model)
        {
            var result = 0;
            var order = await _orderRepository.GetMany(order => order.Id.Equals(id))
                .Include(order => order.OrderDetails).ThenInclude(od => od.Car).ThenInclude(car => car != null ? car.CarOwner : null!)
                .Include(order => order.OrderDetails).ThenInclude(od => od.Driver)
                .FirstOrDefaultAsync();
            if (order != null)
            {
                if (model.Status.Equals(OrderStatus.ManagerConfirmed))
                {
                    var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
                    if (carOwner != null && carOwner.IsAutoAcceptOrder)
                    {
                        order = await CarOwnerAutoApproved(order);
                    }
                    else if (carOwner != null)
                    {
                        order = await ManagerConfirmed(order);
                    }
                }
                if (model.Status.Equals(OrderStatus.CarOwnerApproved))
                {
                    order = await CarOwnerApproved(order);
                }
                if (model.Status.Equals(OrderStatus.Canceled))
                {
                    order = await ManagerDenied(order, model.Description!);
                }
                if (model.Status.Equals(OrderStatus.Ongoing))
                {
                    order = await Ongoin(order);
                }
                if (model.Status.Equals(OrderStatus.ArrivedAtPickUpPoint))
                {
                    order = await ArrivedAtPickUpPoint(order);
                }
                if (model.Status.Equals(OrderStatus.Finished))
                {
                    order = await Finished(order);
                }
                if (model.Status.Equals(OrderStatus.Paid))
                {
                    order = await Paid(order);
                }
                _orderRepository.Update(order);
                result = await _unitOfWork.SaveChanges();
            }
            return result > 0 ? await GetOrder(id) : null!;
        }

        private async Task<Order> CarOwnerAutoApproved(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.CarOwnerApproved.ToString();
            var message = new NotificationCreateModel
            {
                Title = "Bạn có đơn đặt hàng mới",
                Body = "Đơn hàng của bạn đã được duyệt tư động",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Type = NotificationType.Order.ToString(),
                    IsRead = false,
                    Link = order.Id.ToString(),
                }
            };
            await _notificationService.SendNotification(new List<Guid> { carOwner!.AccountId }, message);
            return order;
        }

        private async Task<Order> CarOwnerApproved(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.CarOwnerApproved.ToString();
            var message = new NotificationCreateModel
            {
                Title = "Đơn hàng đã được chấp nhận",
                Body = "Đơn đặt hàng của bạn đã được chủ xe chấp nhận",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Type = NotificationType.Order.ToString(),
                    IsRead = false,
                    Link = order.Id.ToString(),
                }
            };
            await _notificationService.SendNotification(new List<Guid> { order.CustomerId }, message);
            return order;
        }

        private async Task<Order> ManagerConfirmed(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.ManagerConfirmed.ToString();
            var message = new NotificationCreateModel
            {
                Title = "Đơn hàng mới",
                Body = "Bạn có đơn hàng mới cần xác nhận",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Type = NotificationType.Order.ToString(),
                    IsRead = false,
                    Link = order.Id.ToString(),
                }
            };
            await _notificationService.SendNotification(new List<Guid> { carOwner!.AccountId }, message);
            return order;
        }

        private async Task<Order> ManagerDenied(Order order, string description)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.Canceled.ToString();
            order.Description = description;
            var message = new NotificationCreateModel
            {
                Title = "Đơn hàng đã bị từ chối",
                Body = "Đơn đặt hàng của bạn đã bị từ chối bởi bộ phận quản lý",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Type = NotificationType.Order.ToString(),
                    IsRead = false,
                    Link = order.Id.ToString(),
                }
            };
            await _notificationService.SendNotification(new List<Guid> { order.CustomerId }, message);
            return order;
        }

        private async Task<Order> Ongoin(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.Ongoing.ToString();
            var message = new NotificationCreateModel
            {
                Title = "Đơn hàng đã được tiến hành",
                Body = "Đơn hàng của bạn đang được thực hiện",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Type = NotificationType.Order.ToString(),
                    IsRead = false,
                    Link = order.Id.ToString(),
                }
            };
            var userIds = (new List<Guid> { order.CustomerId, });
            userIds.AddRange(order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwnerId : Guid.Empty).ToList());
            await _notificationService.SendNotification(userIds, message);
            return order;
        }

        private async Task<Order> ArrivedAtPickUpPoint(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.Ongoing.ToString();
            var message = new NotificationCreateModel
            {
                Title = "Tài xế đã đến điểm đón",
                Body = "Tài xế của bạn đã đến điểm đón",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Type = NotificationType.Order.ToString(),
                    IsRead = false,
                    Link = order.Id.ToString(),
                }
            };
            await _notificationService.SendNotification(new List<Guid> { order.CustomerId }, message);
            return order;
        }

        private async Task<Order> Paid(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.Ongoing.ToString();
            var cusWallet = await _walletRepository
                     .GetMany(wallet => wallet.Customer != null ? wallet.Customer.AccountId.Equals(order.CustomerId) : false).FirstOrDefaultAsync();
            if (cusWallet != null && cusWallet.Balance > order.Amount)
            {
                var amout = cusWallet.Balance - (order.Amount * 70 / 100);
                cusWallet.Balance = amout;
                _walletRepository.Update(cusWallet);
                if (await _unitOfWork.SaveChanges() > 0)
                {
                    var cusMessage = new NotificationCreateModel
                    {
                        Title = "Thanh toán thành công",
                        Body = "Bạn đã hoàn tất thanh toán với số tiền " + (order.Amount * 70 / 100) + " VNĐ",
                        Data = new NotificationDataViewModel
                        {
                            CreateAt = DateTime.UtcNow.AddHours(7),
                            Type = NotificationType.Order.ToString(),
                            IsRead = false,
                            Link = order.Id.ToString(),
                        }
                    };
                    var userIds = (new List<Guid> { order.CustomerId, });
                    await _notificationService.SendNotification(userIds, cusMessage);
                    var carOwnerIds = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwnerId : Guid.Empty).ToList();
                    var carOwnerWallet = await _walletRepository
                        .GetMany(wallet => wallet.CarOwner != null ? wallet.CarOwner.AccountId.Equals(carOwnerIds.FirstOrDefault()) : false).FirstOrDefaultAsync();
                    if (carOwnerWallet != null)
                    {
                        carOwnerWallet.Balance = carOwnerWallet.Balance + ((order.Amount * 70 / 100) - order.Amount * 10 / 100);
                        _walletRepository.Update(carOwnerWallet);
                        if (await _unitOfWork.SaveChanges() > 0)
                        {
                            var car = await _carRepository.GetMany(car => car.Id.Equals(order.OrderDetails.Select(od => od.Car!.Id).FirstOrDefault())).FirstOrDefaultAsync();
                            car!.Status = CarStatus.Ongoing.ToString();
                            var carOwnerMessage = new NotificationCreateModel
                            {
                                Title = "Đơn hàng của bạn đã được thanh toán",
                                Body = "Đã được cộng " + ((order.Amount * 70 / 100) - order.Amount * 10 / 100) + "VNĐ vào tài khoản",
                                Data = new NotificationDataViewModel
                                {
                                    CreateAt = DateTime.UtcNow.AddHours(7),
                                    Type = NotificationType.Order.ToString(),
                                    IsRead = false,
                                    Link = order.Id.ToString(),
                                }
                            };
                            await _notificationService.SendNotification(carOwnerIds, cusMessage);
                            return order;
                        }
                    }
                };
            }
            return null!;
        }

        private async Task<Order> Finished(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            foreach (var item in order.OrderDetails)
            {
                var car = await _carRepository.GetMany(car => car.Id.Equals(item.Car!.Id)).FirstOrDefaultAsync();
                if (car != null)
                {
                    car.Status = CarStatus.Idle.ToString();
                    _carRepository.Update(car);
                }
            }
            if (await _unitOfWork.SaveChanges() > 0)
            {
                order.Status = OrderStatus.Ongoing.ToString();
                var message = new NotificationCreateModel
                {
                    Title = "Đơn hàng đã kết thúc",
                    Body = "Đơn hàng đã hoàn tất thành công",
                    Data = new NotificationDataViewModel
                    {
                        CreateAt = DateTime.UtcNow.AddHours(7),
                        Type = NotificationType.Order.ToString(),
                        IsRead = false,
                        Link = order.Id.ToString(),
                    }
                };
                var userIds = (new List<Guid> { order.CustomerId, });
                var drivers = order.OrderDetails.Select(od => od!.Driver).ToList();
                if (drivers != null && drivers.Count > 0)
                {
                    var driver = drivers.FirstOrDefault();
                    driver!.Status = DriverStatus.Idle.ToString();
                    _driverRepository.Update(driver);
                    await _unitOfWork.SaveChanges();
                    userIds.AddRange(drivers.Select(driver => driver!.AccountId).ToList());
                }
                userIds.AddRange(order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwnerId : Guid.Empty).ToList());
                await _notificationService.SendNotification(userIds, message);
            }
            return order;
        }

        public async Task<OrderViewModel> GetOrder(Guid id)
        {
            return await _orderRepository.GetMany(order => order.Id == id)
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<OrderViewModel> CreateOrder(Guid customerId, OrderCreateModel model)
        {
            if (!_orderRepository.Any(od => od.CustomerId.Equals(customerId) && od.Status.Equals(OrderStatus.Pending.ToString())))
            {
                var cusWallet = await _walletRepository
                            .GetMany(wallet => wallet.Customer != null ? wallet.Customer.AccountId.Equals(customerId) : false).FirstOrDefaultAsync();
                if (cusWallet != null && cusWallet.Balance > model.Amount)
                {
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customerId,
                        Amount = model.Amount,
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
                            var car = await _carRepository.GetMany(car => car.Id.Equals(orderDetail.CarId)).FirstOrDefaultAsync();
                            if (car != null)
                            {
                                car.Status = CarStatus.Ongoing.ToString();
                                _carRepository.Update(car);
                                if (car.Driver == null)
                                {
                                    var chossenDriver = await FindRecommendDrivver(orderDetail.PickUpLocation!.Latitude, orderDetail.PickUpLocation!.Longitude);
                                    if (chossenDriver != null)
                                    {
                                        chossenDriver.Status = DriverStatus.OnGoing.ToString();
                                        od.DriverId = chossenDriver.AccountId;
                                        _driverRepository.Update(chossenDriver);
                                    }
                                }
                                else
                                {
                                    od.DriverId = car.Driver.AccountId;
                                }
                            }
                        }
                        _orderDetailRepository.Add(od);
                    }
                    var result = await _unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        cusWallet.Balance = cusWallet.Balance - (model.Amount * 30 / 100);
                        _walletRepository.Update(cusWallet);
                        if (await _unitOfWork.SaveChanges() > 0)
                        {
                            var message = new NotificationCreateModel
                            {
                                Title = "Đơn hàng mới",
                                Body = "Bạn có đơn hàng mới cần xác nhận",
                                Data = new NotificationDataViewModel
                                {
                                    CreateAt = DateTime.UtcNow.AddHours(7),
                                    Type = NotificationType.Order.ToString(),
                                    IsRead = false,
                                    Link = order.Id.ToString(),
                                }
                            };
                            var cusMessage = new NotificationCreateModel
                            {
                                Title = "Tạo đơn hàng thành công",
                                Body = "Đã trừ " + (model.Amount * 30 / 100) + "VNĐ tiền cọc cho đơn hàng",
                                Data = new NotificationDataViewModel
                                {
                                    CreateAt = DateTime.UtcNow.AddHours(7),
                                    Type = NotificationType.Order.ToString(),
                                    IsRead = false,
                                    Link = order.Id.ToString(),
                                }
                            };
                            var carOwnerMessage = new NotificationCreateModel
                            {
                                Title = "Nhận tiền cọc đơn hàng",
                                Body = "Đã nhận " + ((order.Amount * 30 / 100) - order.Amount * 10 / 100) + "VNĐ tiền cọc cho đơn mới",
                                Data = new NotificationDataViewModel
                                {
                                    CreateAt = DateTime.UtcNow.AddHours(7),
                                    Type = NotificationType.Order.ToString(),
                                    IsRead = false,
                                    Link = order.Id.ToString(),
                                }
                            };
                            var managers = await _userRepository.GetMany(user => user.Role.Equals(UserRole.Manager.ToString()))
                                .Select(manager => manager.AccountId).ToListAsync();
                            var carId = model.OrderDetails.Select(od => od.CarId).FirstOrDefault();
                            var carOwnerIds = await _carRepository.GetMany(con => con.Id.Equals(carId)).Select(car => car.CarOwnerId).ToListAsync();
                            await _notificationService.SendNotification(new List<Guid> { customerId }, cusMessage);
                            await _notificationService.SendNotification(managers, message);
                            await _notificationService.SendNotification(carOwnerIds, carOwnerMessage);
                            return await GetOrder(order.Id);
                        }
                    }
                }
            }
            return null!;
        }

        private async Task<Driver?> FindRecommendDrivver(double latitude, double longitude)
        {
            var drivers = _driverRepository.GetMany(driver => driver.Status.Equals(DriverStatus.Idle.ToString()) &&
            driver.MinimumTrip > 0 && driver.WishArea != null).DriverDistanceFilter(latitude, longitude, 5);

            var driver = await drivers.OrderByDescending(driver => driver.MinimumTrip)
                .ThenByDescending(driver => driver.Star).FirstOrDefaultAsync();
            if (driver != null)
            {
                driver.MinimumTrip = driver.MinimumTrip - 1;
                _driverRepository.Update(driver);
            }
            return await _unitOfWork.SaveChanges() > 0 ? driver : null!;
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

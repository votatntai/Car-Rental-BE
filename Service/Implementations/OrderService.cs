using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Extensions.MyExtentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Enums;

namespace Service.Implementations
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ITransactionService _transactionService;
        private new readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService,
            ITransactionService transactionService) : base(unitOfWork, mapper)
        {
            _orderRepository = unitOfWork.Order;
            _driverRepository = unitOfWork.Driver;
            _carRepository = unitOfWork.Car;
            _carOwnerRepository = unitOfWork.CarOwner;
            _userRepository = unitOfWork.User;
            _notificationService = notificationService;
            _orderDetailRepository = unitOfWork.OrderDetail;
            _locationRepository = unitOfWork.Location;
            _walletRepository = unitOfWork.Wallet;
            _promotionRepository = unitOfWork.Promotion;
            _mapper = mapper;
            _transactionService = transactionService;
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
                                                 (order.OrderDetails.Any(od => od.Car != null && od.Car.CarOwner != null && od.Car.CarOwner.AccountId == userId)));
            }

            if (filter.Status != null)
            {
                query = query.AsQueryable().Where(order => order.Status == filter.Status.ToString());
            }

            var totalRow = await query.CountAsync();
            var orders = await query
                .OrderByDescending(order => order.CreateAt)
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
                order.OrderDetails.Any(od => od.Car != null && od.Car.CarOwner != null ? od.Car.CarOwner.AccountId.Equals(userId) : false)
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
                    else
                    {
                        order = await ManagerConfirmed(order);
                    }
                }
                else
                if (model.Status.Equals(OrderStatus.CarOwnerApproved))
                {
                    order = await CarOwnerApproved(order);
                }
                else
                if (model.Status.Equals(OrderStatus.Canceled))
                {
                    order = await ManagerDenied(order, model.Description!);
                }
                else
                if (model.Status.Equals(OrderStatus.Ongoing))
                {
                    order = await Ongoing(order);
                }
                else
                if (model.Status.Equals(OrderStatus.ArrivedAtPickUpPoint))
                {
                    order = await ArrivedAtPickUpPoint(order);
                }
                else
                if (model.Status.Equals(OrderStatus.Finished))
                {
                    order = await Finished(order);
                }
                else
                if (model.Status.Equals(OrderStatus.Paid))
                {
                    order = await Paid(order);
                }
                else
                if (Enum.IsDefined(typeof(OrderStatus), model.Status.ToString()))
                {
                    order.Status = model.Status.ToString();
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
            if (await _unitOfWork.SaveChanges() > 0)
            {
                if (carOwner != null)
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
                    await _notificationService.SendNotification(new List<Guid> { carOwner!.AccountId }, message);
                }
            }
            return order;
        }

        private async Task<Order> ManagerDenied(Order order, string description)
        {
            order.Status = OrderStatus.Canceled.ToString();
            order.Description = description;
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
            }
            return order;
        }

        private async Task<Order> Ongoing(Order order)
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
            var userIds = new List<Guid> { order.CustomerId };
            foreach (var od in order.OrderDetails)
            {
                var car = await _carRepository.GetMany(car => car.Id.Equals(od.CarId)).FirstOrDefaultAsync();
                car!.Status = CarStatus.OnGoing.ToString();
                _carRepository.Update(car);
                if (od.DriverId != null)
                {
                    var driver = await _driverRepository.GetMany(driver => driver.AccountId.Equals(od.DriverId)).FirstOrDefaultAsync();
                    driver!.Status = DriverStatus.OnGoing.ToString();
                    _driverRepository.Update(driver);
                }
                if (carOwner != null)
                {
                    userIds.Add(carOwner.AccountId);
                }
            }
            if (await _unitOfWork.SaveChanges() > 0)
            {
                await _notificationService.SendNotification(userIds, message);
            }
            return order;
        }

        private async Task<Order> ArrivedAtPickUpPoint(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.ArrivedAtPickUpPoint.ToString();
            if (await _unitOfWork.SaveChanges() > 0)
            {

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
            }
            return order;
        }

        private async Task<Order> Paid(Order order)
        {
            var carOwner = order.OrderDetails.Select(od => od.Car != null ? od.Car.CarOwner : null!).FirstOrDefault();
            order.Status = OrderStatus.Paid.ToString();
            var cusWallet = await _walletRepository
                     .GetMany(wallet => wallet.Customer != null ? wallet.Customer.AccountId.Equals(order.CustomerId) : false).FirstOrDefaultAsync();
            if (cusWallet != null && cusWallet.Balance > order.Amount)
            {
                var amout = cusWallet.Balance - RoundedAmount(order.Amount * 70 / 100);
                cusWallet.Balance = amout;
                _walletRepository.Update(cusWallet);
                if (await _unitOfWork.SaveChanges() > 0)
                {
                    var cusTransaction = new TransactionCreateModel
                    {
                        Amount = RoundedAmount(order.Amount * 70 / 100),
                        Description = "Tiền thanh toán đơn hàng",
                        Status = "Đã hoàn thành",
                        Type = TransactionType.Payment.ToString(),
                    };
                    await _transactionService.CreateTransactionForCustomer(order.CustomerId, cusTransaction);
                    if (carOwner != null)
                    {
                        var carOwnerTransaction = new TransactionCreateModel
                        {
                            Amount = RoundedAmount(order.Amount - order.Amount * 20 / 100),
                            Description = "Tiền thanh toán đơn hàng",
                            Status = "Đã hoàn thành",
                            Type = TransactionType.Deposit.ToString(),
                        };
                        await _transactionService.CreateTransactionForCarOwner(carOwner.AccountId, carOwnerTransaction);
                    }
                    var cusMessage = new NotificationCreateModel
                    {
                        Title = "Thanh toán thành công",
                        Body = "Bạn đã hoàn tất thanh toán với số tiền " + RoundedAmount(order.Amount * 70 / 100) + " VNĐ",
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
                    foreach (var item in order.OrderDetails)
                    {
                        if (item.Car != null && item.Car.CarOwnerId != null)
                        {
                            var carOwnerWallet = await _walletRepository
                        .GetMany(wallet => wallet.CarOwner != null ? wallet.CarOwner.AccountId.Equals(item.Car.CarOwnerId) : false).FirstOrDefaultAsync();
                            if (carOwnerWallet != null)
                            {
                                carOwnerWallet.Balance = carOwnerWallet.Balance + RoundedAmount(order.Amount - order.Amount * 20 / 100);
                                _walletRepository.Update(carOwnerWallet);
                                if (await _unitOfWork.SaveChanges() > 0)
                                {
                                    var car = await _carRepository.GetMany(car => car.Id.Equals(order.OrderDetails.Select(od => od.Car!.Id).FirstOrDefault())).FirstOrDefaultAsync();
                                    car!.Status = CarStatus.OnGoing.ToString();
                                    if (carOwner != null)
                                    {
                                        var carOwnerMessage = new NotificationCreateModel
                                        {
                                            Title = "Đơn hàng của bạn đã được thanh toán",
                                            Body = "Đã được cộng " + RoundedAmount(order.Amount - order.Amount * 20 / 100) + " VNĐ vào tài khoản",
                                            Data = new NotificationDataViewModel
                                            {
                                                CreateAt = DateTime.UtcNow.AddHours(7),
                                                Type = NotificationType.Order.ToString(),
                                                IsRead = false,
                                                Link = order.Id.ToString(),
                                            }
                                        };
                                        await _notificationService.SendNotification(new List<Guid> { carOwner.AccountId }, carOwnerMessage);
                                    }
                                }
                            }
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
                    car.Rented += 1;
                    _carRepository.Update(car);
                }
            }
            if (await _unitOfWork.SaveChanges() > 0)
            {
                order.Status = OrderStatus.Finished.ToString();
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
                if (!drivers.Any(driver => driver == null))
                {
                    var driver = drivers.FirstOrDefault();
                    driver!.Status = DriverStatus.Idle.ToString();
                    if (driver!.Finished != null)
                    {
                        driver!.Finished += 1;
                    }
                    else
                    {
                        driver!.Finished = 1;
                    }
                    _driverRepository.Update(driver);
                    await _unitOfWork.SaveChanges();
                    userIds.AddRange(drivers.Select(driver => driver!.AccountId).ToList());
                }
                foreach (var od in order.OrderDetails)
                {
                    if (od.Car != null && od.Car.CarOwnerId != null)
                    {
                        userIds.Add((Guid)od.Car.CarOwnerId);
                    }
                }
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

        public async Task<IActionResult> CreateOrder(Guid customerId, OrderCreateModel model)
        {
            if (!_orderRepository.Any(od => od.CustomerId.Equals(customerId) && od.Status.Equals(OrderStatus.Pending.ToString())))
            {
                var cusWallet = await _walletRepository
                            .GetMany(wallet => wallet.Customer != null ? wallet.Customer.AccountId.Equals(customerId) : false).FirstOrDefaultAsync();
                if (cusWallet != null && cusWallet.Balance > model.Amount)
                {
                    var rentalTime = model.OrderDetails.Select(od => od.EndTime - od.StartTime).FirstOrDefault();
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customerId,
                        Amount = model.Amount,
                        IsPaid = model.IsPaid,
                        Description = model.Description,
                        PromotionId = model.PromotionId,
                        RentalTime = Convert.ToInt32((rentalTime).TotalDays),
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
                                car.Status = CarStatus.InOrder.ToString();
                                _carRepository.Update(car);
                                if (car.Driver == null)
                                {
                                    var chossenDriver = await FindRecommendDriver(orderDetail.PickUpLocation!.Latitude, orderDetail.PickUpLocation!.Longitude);
                                    if (chossenDriver != null)
                                    {
                                        od.DriverId = chossenDriver.AccountId;
                                        var driverMessage = new NotificationCreateModel
                                        {
                                            Title = "Đơn hàng mới",
                                            Body = "Bạn có đơn hàng mới",
                                            Data = new NotificationDataViewModel
                                            {
                                                CreateAt = DateTime.UtcNow.AddHours(7),
                                                Type = NotificationType.Order.ToString(),
                                                IsRead = false,
                                                Link = order.Id.ToString(),
                                            }
                                        };
                                        await _notificationService.SendNotification(new List<Guid> { chossenDriver.AccountId }, driverMessage);
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
                    var promotion = await _promotionRepository.GetMany(promotion => promotion.Id.Equals(model.PromotionId)).FirstOrDefaultAsync();
                    if (promotion != null)
                    {
                        promotion.Quantity = promotion.Quantity - 1;
                    }
                    var result = await _unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        cusWallet.Balance = cusWallet.Balance - RoundedAmount(model.Amount * 30 / 100);
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
                                Body = "Đã trừ " + RoundedAmount(model.Amount * 30 / 100) + " VNĐ tiền cọc cho đơn hàng",
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
                            var cusTransaction = new TransactionCreateModel
                            {
                                Amount = RoundedAmount(order.Amount * 30 / 100),
                                Description = "Tiền cọc đơn hàng",
                                Status = "Đã hoàn thành",
                                Type = TransactionType.Payment.ToString(),
                            };
                            await _transactionService.CreateTransactionForCustomer(order.CustomerId, cusTransaction);
                            await _notificationService.SendNotification(new List<Guid> { customerId }, cusMessage);
                            await _notificationService.SendNotification(managers, message);
                            return new JsonResult(await GetOrder(order.Id));
                        }
                    }
                }
                return new StatusCodeResult(StatusCodes.Status402PaymentRequired);
            }
            return new StatusCodeResult(StatusCodes.Status409Conflict);
        }

        private async Task<Driver?> FindRecommendDriver(double latitude, double longitude)
        {
            var drivers = _driverRepository.GetMany(driver => driver.Status.Equals(DriverStatus.Idle.ToString()) && driver.WishArea != null)
                .DriverDistanceFilter(latitude, longitude, 10);

            var driver = await drivers.OrderByDescending(driver => driver.MinimumTrip)
                .ThenByDescending(driver => driver.Star).FirstOrDefaultAsync();
            if (driver != null && driver.MinimumTrip > 0)
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

        private int RoundedAmount(double amount)
        {
            return Convert.ToInt32(Math.Round(amount));
        }

    }
}

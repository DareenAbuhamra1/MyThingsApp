using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;
using MyThings.Infrastructure.Repositories;

namespace MyThings.Infrastructure.Services;

public class OrderService : IOrderService
{

    private readonly IReadUnitOfWork _readUnitOfWork;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;
    private readonly ITimeEstimationService _timeEstimationService;

    public OrderService(IReadUnitOfWork readUnitOfWork, IUnitOfWork unitOfWork, IOrderReadRepository orderReadRepository, IDeliveryFeeService deliveryFeeService, ITimeEstimationService timeEstimationService)
    {
        _readUnitOfWork = readUnitOfWork;
        _unitOfWork = unitOfWork;
        _orderReadRepository = orderReadRepository;
        _deliveryFeeService = deliveryFeeService;
        _timeEstimationService = timeEstimationService;
    }
    //fix this 
    public async Task<ServiceResponse<IReadOnlyList<OrderInfoDto>>> GetAllCustomerOrdersAsync(int customerId)
    {
        var orders = await _orderReadRepository.GetAllCustomerOrdersAsync(customerId);

        var result = new List<OrderInfoDto>();

        foreach (var o in orders)
        {
            result.Add(new OrderInfoDto
            {
                OrderId = o.Id,
                Status = o.Status, // Cast from entity status to DTO enum
                PartnerName = o.Partner?.Name ?? "Unknown Partner",
                Area = o.Partner?.Location.Area ?? "N/A",
                PlacedDate = DateOnly.FromDateTime(o.CreatedAt),
                PlacedTime = TimeOnly.FromDateTime(o.CreatedAt),
                SubTotal = o.SubTotal,
                ServiceFee = o.ServiceFee,
                DeliveryFees = o.DeliveryFees,
                SavingAmount = o.SavingAmount ?? 0m,
                TotalPrice = o.TotalPayment,
                DeliveryLocation = $"{o.DeliveryLocation?.Street}, {o.DeliveryLocation?.Area}, {o.DeliveryLocation?.City.ToString()}" ?? "No address provided",
                PaymentMethod = o.PaymentType?.ToString() ?? "Undefined",
                DriverName = $"{o.Driver?.FirstName} {o.Driver?.LastName}".Trim() == "" ? "Waiting for assignment" : $"{o.Driver?.FirstName} {o.Driver?.LastName}",

                // Mapping the nested OrderItems list
                OrderItems = o.OrderLines?.Select(ol => new OrderItem
                {
                    OrderItemId = ol.ProductId,
                    OrderItemName = ol.ProductName,
                    OrderItemPrice = ol.Price,
                    Quantity = ol.Quantity,

                    // Mapping the nested Options list
                    OrderItemOptions = ol.OrderLineOptions?.Select(olo => new OrderItemOption
                    {
                        OrderItemOptionId = olo.ProductOptionId,
                        OrderItemOptionName = olo.Option,
                        OrderItemOptionQuantity = olo.Quantity,
                        OrderItemOptionPrice = olo.Price
                    }).ToList() ?? new List<OrderItemOption>()
                }).ToList() ?? new List<OrderItem>()
            });
        }

        return ServiceResponse<IReadOnlyList<OrderInfoDto>>.Ok(result);
    }
    public async Task<ServiceResponse<OrderCartResponseDto>> AddOrderCartAsync(OrderCartDto orderCart)
    {
        var partner = await _readUnitOfWork.Partners.GetByIdAsync(orderCart.PartnerId);
        if (partner == null)
            return ServiceResponse<OrderCartResponseDto>.Failure("The partner is not found", 404);

        var deliveryLocation = await _readUnitOfWork.Locations.GetByIdAsync(orderCart.DeliveryLocationId);
        if (deliveryLocation == null)
            return ServiceResponse<OrderCartResponseDto>.Failure("The Location is not found", 404);


        var product = await _readUnitOfWork.Products.GetByIdAsync(orderCart.OrderLine.ProductId);
        if (product == null)
            return ServiceResponse<OrderCartResponseDto>.Failure("The selected product is not found", 404);

        var partnerLocation = await _readUnitOfWork.Locations.GetByIdAsync(partner.LocationId);
        if (partnerLocation == null)
            return ServiceResponse<OrderCartResponseDto>.Failure("The store location is not found", 404);

        int deliveryRuleId = partner.DeliveryRuleId;

        var pendingOrder = await _orderReadRepository.GetCustomerPendingOrderAsync(orderCart.CustomerId);
        if (pendingOrder == null)
        {
            var newCart = new Order
            {
                CustomerId = orderCart.CustomerId,
                DeliveryLocationId = orderCart.DeliveryLocationId,
                DomainId = orderCart.DomainId,
                PartnerId = orderCart.PartnerId,
                Status = OrderStatusEnum.Pending,
                SavingAmount = 0.00m,
                DeliveryRuleId = deliveryRuleId,
                PaymentType = OrderPaymentTypeEnum.Cash,
                CreatedAt = DateTime.UtcNow,
                OrderLines = new List<OrderLine>()
            };

            var line = new OrderLine
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = orderCart.OrderLine.Quantity,
                Note = orderCart.OrderLine.Note ?? "",
                OrderLineOptions = new List<OrderLineOption>()
            };

            decimal SubTotal = product.Price * orderCart.OrderLine.Quantity;

            if (orderCart.OrderLine.OrderLineOptions is not null)
            {
                foreach (var olp in orderCart.OrderLine.OrderLineOptions)
                {
                    //might introduce a getMultipleByIds instead of going to the db multiple times
                    var ProductOption = await _readUnitOfWork.ProductOptions.GetByIdAsync(olp.ProductOptionId)
                        ?? throw new Exception("Product Option not found");

                    var lineOption = new OrderLineOption
                    {
                        ProductOptionId = olp.ProductOptionId,
                        Option = ProductOption.Option,
                        Price = ProductOption.Price,
                        Quantity = olp.Quantity,
                    };

                    line.OrderLineOptions.Add(lineOption); // link orderLineOptions to OrderLine
                    SubTotal += ProductOption.Price * olp.Quantity;
                }
            }

            newCart.OrderLines.Add(line);//link orderline to order

            newCart.SubTotal = SubTotal;
            newCart.ServiceFee = Math.Round(SubTotal * 0.1m, 3);

            var DeliveryFees = await _deliveryFeeService.CalculateDeliveryFee(newCart.PartnerId, newCart.DeliveryLocationId, deliveryRuleId, SubTotal);

            newCart.DeliveryFees = DeliveryFees;
            newCart.TotalPayment = newCart.SubTotal + newCart.ServiceFee + DeliveryFees;

            await _unitOfWork.Orders.AddAsync(newCart);
            await _unitOfWork.CompleteAsync();

            return ServiceResponse<OrderCartResponseDto>.Ok(new OrderCartResponseDto
            {
                OrderId = newCart.Id,
                DeliveryLocation = $"{deliveryLocation.Street} {deliveryLocation.Area}",
                Status = newCart.Status,
                CustomerId = newCart.CustomerId,
                PartnerId = newCart.PartnerId,
                PartnerName = partner.Name,
                SubTotal = newCart.SubTotal,
                DeliveryFees = newCart.DeliveryFees,
                TotalPrice = newCart.TotalPayment,
                OrderLine = new OrderLineCartResponse
                {
                    ProductId = line.ProductId,
                    ProductName = line.ProductName,
                    Quantity = line.Quantity,
                    OrderLineOptions = line.OrderLineOptions?.Select(opt => new OrderLineOptionsCartResponse
                    {
                        ProductOptionId = opt.ProductOptionId,
                        ProductOption = opt.Option,
                        Quantity = opt.Quantity
                    }).ToList()
                }
            });
        }
        else
        {
            if (product.PartnerId != orderCart.PartnerId)
            {
                return ServiceResponse<OrderCartResponseDto>.Failure("Bad request", 400);
            }
            var line = new OrderLineCartDto
            {
                OrderId = pendingOrder.Id,
                ProductId = orderCart.OrderLine.ProductId,
                Quantity = orderCart.OrderLine.Quantity,
                OrderLineOptions = orderCart.OrderLine.OrderLineOptions?.Select(
                    opt => new OrderLineOptionsCartDto
                    {
                        ProductOptionId = opt.ProductOptionId,
                        Quantity = opt.Quantity
                    }).ToList()
            };
            var response = await AddOrderLineAsync(line);

            return response;
        }
    }

    public async Task<ServiceResponse<OrderCartResponseDto>> AddOrderLineAsync(OrderLineCartDto orderLine)
    {
        var order = await _orderReadRepository.GetOrderByOrderIdAsync(orderLine.OrderId);
        if (order == null) return ServiceResponse<OrderCartResponseDto>.Failure("The order is not found", 404);

        var Product = await _readUnitOfWork.Products.GetByIdAsync(orderLine.ProductId);
        if (Product == null) return ServiceResponse<OrderCartResponseDto>.Failure("The Product is not found", 404);

        var line = new OrderLine
        {
            OrderId = order.Id,
            ProductId = Product.Id,
            Quantity = orderLine.Quantity,
            ProductName = Product.Name,
            Price = Product.Price,
            OrderLineOptions = new List<OrderLineOption>()
        };
        order.SubTotal += Product.Price * orderLine.Quantity;

        if (orderLine.OrderLineOptions is not null)
        {
            foreach (var olp in orderLine.OrderLineOptions)
            {
                var ProductOption = await _readUnitOfWork.ProductOptions.GetByIdAsync(olp.ProductOptionId);

                if (ProductOption == null)
                    return ServiceResponse<OrderCartResponseDto>.Failure("Product Option not found", 404);


                var orderLineOption = new OrderLineOption
                {
                    ProductOptionId = olp.ProductOptionId,
                    Quantity = olp.Quantity,
                    Price = ProductOption.Price,
                    Option = ProductOption.Option
                };
                line.OrderLineOptions.Add(orderLineOption);
                order.SubTotal += ProductOption.Price * olp.Quantity;
            }
        }
        order.DeliveryFees = await _deliveryFeeService.CalculateDeliveryFee(order.PartnerId, order.DeliveryLocationId, order.DeliveryRuleId, order.SubTotal);
        order.ServiceFee = Math.Round(order.SubTotal * 0.1m, 3);
        order.TotalPayment = order.SubTotal + order.ServiceFee + order.DeliveryFees;

        order.OrderLines.Add(line);

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        var responseDto = new OrderCartResponseDto
        {
            OrderId = order.Id,
            PartnerName = order.Partner?.Name ?? "Unknown",
            DeliveryLocation = $"{order.DeliveryLocation?.Street ?? ""} {order.DeliveryLocation?.Area ?? ""}",
            Status = order.Status,
            CustomerId = order.CustomerId,
            PartnerId = order.PartnerId,
            SubTotal = order.SubTotal,
            DeliveryFees = order.DeliveryFees,
            TotalPrice = order.TotalPayment,
            OrderLine = new OrderLineCartResponse
            {
                ProductId = line.ProductId,
                ProductName = line.ProductName,
                Quantity = line.Quantity,
                OrderLineOptions = line.OrderLineOptions?.Select(opt => new OrderLineOptionsCartResponse
                {
                    ProductOptionId = opt.ProductOptionId,
                    ProductOption = opt.Option,
                    Quantity = opt.Quantity
                }).ToList()
            }
        };

        return ServiceResponse<OrderCartResponseDto>.Ok(responseDto);
    }
    public async Task<ServiceResponse<OrderPlacementResponseDto>> PlaceOrderAsync(int orderId)
    {
        var readOrder = await _orderReadRepository.GetOrderByOrderIdAsync(orderId);
        if (readOrder == null) return ServiceResponse<OrderPlacementResponseDto>.Failure("The Order is not found", 404);

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null) return ServiceResponse<OrderPlacementResponseDto>.Failure("The Order is not found", 404);

        if (order.Status == OrderStatusEnum.Pending)
        {
            order.Status = OrderStatusEnum.Placed;
            order.UpdatedAt = DateTime.UtcNow;
            var locations = new PartnerCustomerLocDto
            {
                PartnerLat = readOrder.Partner.Location.Latitude,
                PartnerLon = readOrder.Partner.Location.Longitude,
                CustomerLat = readOrder.DeliveryLocation.Latitude,
                CustomerLon = readOrder.DeliveryLocation.Longitude
            };

            var deliveryEstimate = await _timeEstimationService.EstimateDeliveryIntervalAsync(locations);
            order.StartEstimation = deliveryEstimate.StartEstimate;
            order.EndEstimation = deliveryEstimate.EndEstimate;
            order.PlacementTime = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();
        }
        var responseDto = new OrderPlacementResponseDto
        {
            DeliveryLocationId = order.DeliveryLocationId,
            Status = order.Status,
            CustomerId = order.CustomerId,
            SubTotal = order.SubTotal,
            ServiceFee = order.ServiceFee,
            DeliveryFees = order.DeliveryFees,
            SavingAmount = order.SavingAmount ?? 0m,
            DeliveryRuleId = order.DeliveryRuleId,
            TotalPayment = order.TotalPayment,
            PaymentType = order.PaymentType ?? OrderPaymentTypeEnum.Cash,
            StartEstimation = order.StartEstimation ?? TimeOnly.MinValue,
            EndEstimation = order.EndEstimation ?? TimeOnly.MinValue,
            PartnerId = order.PartnerId,
            DomainId = order.DomainId,
            Note = order.Note ?? "",
            OrderLines = order.OrderLines.Select(ol => new OrderLineDto
            {
                OrderId = order.Id,
                ProductId = ol.ProductId,
                ProductName = ol.ProductName,
                Price = ol.Price,
                Quantity = ol.Quantity,
                Note = ol.Note ?? "",

                OrderLineOptions = ol.OrderLineOptions?.Select(opt => new OrderLineOptionDto
                {
                    OrderLineId = ol.Id,
                    ProductOptionId = opt.ProductOptionId,
                    Option = opt.Option,
                    Price = opt.Price,
                    Quantity = opt.Quantity
                }).ToList() ?? new List<OrderLineOptionDto>()
            }).ToList()
        };
        return ServiceResponse<OrderPlacementResponseDto>.Ok(responseDto);
    }
    private async Task<OrderInfoDto> GetOrderByIdAsync(Order order)
    {

        var result = new OrderInfoDto
        {
            OrderId = order.Id,
            Status = order.Status,
            PartnerName = order.Partner.Name,
            Area = order.DeliveryLocation.Area,
            SubTotal = order.SubTotal,
            ServiceFee = order.ServiceFee,
            DeliveryFees = order.DeliveryFees,
            SavingAmount = order.SavingAmount ?? 0m,
            TotalPrice = order.TotalPayment,
            DeliveryLocation = $"{order.DeliveryLocation.Street}, {order.DeliveryLocation.Area}, {order.DeliveryLocation.City.ToString()}",
            PaymentMethod = order.PaymentType?.ToString() ?? "Undefined",
            DriverName = $"{order.Driver?.FirstName} {order.Driver?.LastName}",
            OrderItems = order.OrderLines
                .Select(ol => new OrderItem
                {
                    OrderItemId = ol.Id,
                    OrderItemName = ol.ProductName,
                    OrderItemPrice = ol.Price,
                    Quantity = ol.Quantity,
                    OrderItemOptions = (ol.OrderLineOptions ?? new List<OrderLineOption>())
                        .Select(olp => new OrderItemOption
                        {
                            OrderItemOptionId = olp.Id,
                            OrderItemOptionName = olp.Option,
                            OrderItemOptionPrice = olp.Price,
                            OrderItemOptionQuantity = olp.Quantity
                        }).ToList()
                }).ToList()
        };
        return result;
    }

    public async Task<ServiceResponse<bool>> ReceiveOrderAsync(int orderId, int partnerId, OrderStatusEnum status)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

        if (order == null) return ServiceResponse<bool>.Failure("Order not found", 404);

        //403 The server understands the request, but refuses to authorize it
        if (order.PartnerId != partnerId) return ServiceResponse<bool>.Failure("Unauthorized: This order does not belong to your store", 403);

        //400: Bad request
        if (order.Status != OrderStatusEnum.Placed) return ServiceResponse<bool>.Failure($"Order is already in {order.Status}", 400);

        if (status == OrderStatusEnum.Accepted)
        {
            var Partner = await _unitOfWork.Partners.GetByIdAsync(partnerId);
            if (Partner == null) return ServiceResponse<bool>.Failure("Partner not found", 404);

            var Location = await _unitOfWork.Locations.GetByIdAsync(Partner.LocationId);
            if (Location == null) return ServiceResponse<bool>.Failure("Location not found", 404);

            order.AcceptedTime = DateTime.UtcNow;
        }

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>> GetPartnerPlacedOrdersAsync(int partnerId)
    {
        var orders = await _orderReadRepository.GetPartnerPlacedOrdersAsync(partnerId);

        if (orders == null) return ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>.Failure("Orders not found", 404);

        var mappedDto = orders.Select(o => new PartnerOrderInfoDto
        {
            OrderId = o.Id,
            Status = o.Status,
            SubTotal = o.SubTotal,
            TotalPrice = o.TotalPayment,
            OrderItems = o.OrderLines
                   .Select(ol => new PartnerOrderItem
                   {
                       OrderItemId = ol.Id,
                       OrderItemName = ol.ProductName,
                       OrderItemPrice = ol.Price,
                       Quantity = ol.Quantity,
                       OrderItemOptions = ol.OrderLineOptions
                            .Select(olp => new PartnerOrderItemOption
                            {
                                ItemOptionId = olp.Id,
                                ItemOptionName = olp.Option,
                                ItemOptionQuantity = olp.Quantity,
                                ItemOptionPrice = olp.Price
                            }).ToList()
                   }).ToList()
        });


        return ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>.Ok(mappedDto.ToList());
    }

    public async Task<ServiceResponse<PartnerOrderInfoDto>> GetPartnerPlacedOrderAsync(int orderId, int partnerId)
    {
        var order = await _orderReadRepository.GetPartnerPlacedOrderAsync(orderId, partnerId);

        if (order == null) return ServiceResponse<PartnerOrderInfoDto>.Failure("Order not found", 404);

        var mappedDto = new PartnerOrderInfoDto
        {
            OrderId = order.Id,
            Status = order.Status,
            SubTotal = order.SubTotal,
            TotalPrice = order.TotalPayment,
            OrderItems = order.OrderLines
                   .Select(ol => new PartnerOrderItem
                   {
                       OrderItemId = ol.Id,
                       OrderItemName = ol.ProductName,
                       OrderItemPrice = ol.Price,
                       Quantity = ol.Quantity,
                       OrderItemOptions = ol.OrderLineOptions
                            .Select(olp => new PartnerOrderItemOption
                            {
                                ItemOptionId = olp.Id,
                                ItemOptionName = olp.Option,
                                ItemOptionQuantity = olp.Quantity,
                                ItemOptionPrice = olp.Price
                            }).ToList()
                   }).ToList()
        };

        return ServiceResponse<PartnerOrderInfoDto>.Ok(mappedDto);
    }

    public async Task<ServiceResponse<List<DriverOrderInfo>>> GetNearestOrders(int driverId)
    {
        var driver = await _readUnitOfWork.Drivers.GetByIdAsync(driverId);

        if (driver == null) return ServiceResponse<List<DriverOrderInfo>>.Failure("Driver not found", 404);

        if (driver.Latitude == null || driver.Longitude == null)
            return ServiceResponse<List<DriverOrderInfo>>.Failure("Driver Location is null", 404);

        var Orders = await _orderReadRepository.FindNearestOrdersAsync((decimal)driver.Latitude, (decimal)driver.Longitude);

        return ServiceResponse<List<DriverOrderInfo>>.Ok(Orders);
    }

    public async Task<ServiceResponse<bool>> AssignDriverToOrder(int orderId, int driverId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            return ServiceResponse<bool>.Failure("The order is not found", 404);
        if (order.DriverId != null)
            return ServiceResponse<bool>.Failure("The order is already assigned to a driver", 400);
        if (order.Status != OrderStatusEnum.Accepted)
            return ServiceResponse<bool>.Failure("The order is not accepted yet", 400);

        var driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
        if (driver == null)
            return ServiceResponse<bool>.Failure("The driver is not found", 404);

        if (driver.IsOnline == false)
            return ServiceResponse<bool>.Failure("The driver is not online", 400);
        if (driver.IsAssigned == true)
            return ServiceResponse<bool>.Failure("The driver is already assigned to an order", 400);

        order.DriverId = driverId;
        order.Status = OrderStatusEnum.Assigned;
        order.UpdatedAt = DateTime.UtcNow;

        driver.IsAssigned = true;
        driver.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Orders.Update(order);
        _unitOfWork.Drivers.Update(driver);

        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);

    }

    public async Task<ServiceResponse<bool>> SetOrderToReadyForPickupAsync(int orderId, int partnerId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order is null) return ServiceResponse<bool>.Failure("The order doesn't exist", 404);
        //403 Forbidden
        if (order.PartnerId != partnerId) return ServiceResponse<bool>.Failure("The order doesn't belong to partner", 403);

        if (!(order.Status == OrderStatusEnum.Assigned || order.Status == OrderStatusEnum.Accepted))
            return ServiceResponse<bool>.Failure("Order must be Accepted or Assigned first", 400);

        order.Status = OrderStatusEnum.ReadyForPickUp;
        order.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Orders.Update(order);
        var rowsAffected = await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);

    }

    public async Task<ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>> GetPartnerPreparingOrdersAsync(int partnerId)
    {
        var orders = await _orderReadRepository.GetPartnerPreparingOrdersAsync(partnerId);
        if (orders == null) return ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>.Ok(new List<PartnerOrderInfoDto>());

        var mappedDto = orders.Select(o => new PartnerOrderInfoDto
        {
            OrderId = o.Id,
            Status = o.Status,
            SubTotal = o.SubTotal,
            TotalPrice = o.TotalPayment,
            OrderItems = o.OrderLines.Select(
                ol => new PartnerOrderItem
                {
                    OrderItemId = ol.Id,
                    OrderItemName = ol.ProductName,
                    OrderItemPrice = ol.Price,
                    Quantity = ol.Quantity,
                    OrderItemOptions = ol.OrderLineOptions.Select(
                        olp => new PartnerOrderItemOption
                        {
                            ItemOptionId = olp.Id,
                            ItemOptionName = olp.Option,
                            ItemOptionPrice = olp.Price,
                            ItemOptionQuantity = olp.Quantity
                        }).ToList()
                }).ToList()
        });
        return ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>.Ok(mappedDto.ToList());
    }

    public async Task<ServiceResponse<bool>> PickOrderAsync(int orderId, int driverId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order is null) return ServiceResponse<bool>.Failure("The order is not found", 404);

        if (order.Status != OrderStatusEnum.ReadyForPickUp)
            return ServiceResponse<bool>.Failure("The order must be ready for pickup", 400);

        if (order.DriverId != driverId) return ServiceResponse<bool>.Failure("The order doesn't belong to driver", 403);

        order.Status = OrderStatusEnum.PickedUp;
        order.UpdatedAt = DateTime.UtcNow;
        order.PickedUpTime = DateTime.UtcNow;

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<bool>> DeliverOrderAsync(int orderId, int driverId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order is null) return ServiceResponse<bool>.Failure("The order is not found", 404);

        if (order.Status != OrderStatusEnum.PickedUp)
            return ServiceResponse<bool>.Failure("The order is not picked up yet", 400);

        if (order.DriverId != driverId) return ServiceResponse<bool>.Failure("The order doesn't belong to the driver", 403);

        order.Status = OrderStatusEnum.Delivered;
        order.UpdatedAt = DateTime.UtcNow;
        order.DeliveredTime = DateTime.UtcNow;

        var driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
        if (driver is null) return ServiceResponse<bool>.Failure("The driver is not found", 404);
        driver.IsAssigned = false;

        _unitOfWork.Drivers.Update(driver);
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<IReadOnlyList<AdminOrderDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderReadRepository.GetAllOrdersAsync();

        if (orders is null) return ServiceResponse<IReadOnlyList<AdminOrderDto>>.Ok(new List<AdminOrderDto>());

        var listOfOrders = new List<AdminOrderDto>();

        foreach (var o in orders)
        {
            listOfOrders.Add(
                new AdminOrderDto
                {
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                    PartnerName = o.Partner.Name,
                    PartnerLocation = $"{o.Partner.Location.Street},{o.Partner.Location.Area},{o.Partner.Location.City.ToString()}, {o.Partner.Location.Country.ToString()}",
                    DeliveryLocation = $"{o.DeliveryLocation.Street},{o.DeliveryLocation.Area},{o.DeliveryLocation.City.ToString()}, {o.DeliveryLocation.Country.ToString()}",
                    Domain = o.Domain.Name,
                    Status = o.Status.ToString(),
                    DriverName = o.Driver != null ? $"{o.Driver.FirstName} {o.Driver.LastName}" : null,
                    SubTotal = o.SubTotal,
                    ServiceFee = o.ServiceFee,
                    DeliveryFees = o.DeliveryFees,
                    TotalPayment = o.TotalPayment,
                    PaymentType = o.PaymentType != null ? o.PaymentType : null,
                    StartEstimation = o.StartEstimation,
                    EndEstimation = o.EndEstimation,
                    AcceptedTime = o.AcceptedTime,
                    PlacementTime = o.PlacementTime,
                    DeliveredTime = o.DeliveredTime,
                    PickedUpTime = o.PickedUpTime,
                }
            );
        }
        return ServiceResponse<IReadOnlyList<AdminOrderDto>>.Ok(listOfOrders);
    }

    public async Task<ServiceResponse<bool>> CancelOrderAsync(int orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

        if (order is null) return ServiceResponse<bool>.Failure("The order is not found", 404);

        order.Status = OrderStatusEnum.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<OrderCartViewDto>> GetCartAsync(int customerId)
    {
        var order = await _orderReadRepository.GetCustomerCartAsync(customerId);

        if (order == null) return ServiceResponse<OrderCartViewDto>.Failure("The order is not found", 404);


        var response = new OrderCartViewDto
        {
            OrderId = order.Id,
            PartnerName = order.Partner?.Name ?? "Unknown",
            DeliveryLocation =
                $"{order.DeliveryLocation?.Street ?? ""} {order.DeliveryLocation?.Area ?? ""}",
            Status = order.Status,
            CustomerId = order.CustomerId,
            PartnerId = order.PartnerId,
            SubTotal = order.SubTotal,
            ServiceFee = order.TotalPayment - order.SubTotal - order.DeliveryFees,
            DeliveryFees = order.DeliveryFees,
            TotalPrice = order.TotalPayment,
            OrderLines = order.OrderLines.Select(
                    ol => new OrderLineView
                    {
                        ProductId = ol.ProductId,
                        ProductName = ol.ProductName,
                        Quantity = ol.Quantity,
                        OrderLineOptions = ol.OrderLineOptions?.Select(
                            olp => new OrderLineOptionsView
                            {
                                ProductOptionId = olp.ProductOptionId,
                                ProductOption = olp.Option,
                                Quantity = olp.Quantity,
                            }).ToList() ?? new List<OrderLineOptionsView>()
                    }
                ).ToList()
        };

        return ServiceResponse<OrderCartViewDto>.Ok(response);
    }

    public async Task<ServiceResponse<bool>> DeleteCartAsync(int orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order != null)
        {
            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.CompleteAsync();
        }
        return ServiceResponse<bool>.Ok(true);

    }

    public async Task<ServiceResponse<PageResponse<OrderForPaginationDto>>> GetOrderHistoryAsync(OrderHistoryQueryDto query, int partnerId)
    {
        var ordersQuery = _orderReadRepository.GetPartnerOrders(partnerId);
        if (ordersQuery != null)
        {
            if (query.Status.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.Status == query.Status.Value);
            }
            if (query.From.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedAt >= query.From.Value);
            }
            if (query.To.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedAt <= query.To.Value);
            }
            ordersQuery = ordersQuery.OrderByDescending(o => o.CreatedAt);

            var totalCount = await ordersQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(
                (double)totalCount / query.PageSize
            );

            var pageOrders = await ordersQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var data = pageOrders.Select(
                o => new OrderForPaginationDto
                {
                    OrderId = o.Id,
                    Status = o.Status,
                    SubTotal = o.SubTotal,
                    ServiceFee = o.ServiceFee,
                    DeliveryFees = o.DeliveryFees,
                    TotalPayment = o.TotalPayment,
                    StartEstimation = o.StartEstimation,
                    EndEstimation = o.EndEstimation,
                    PlacementTime = o.PlacementTime,
                    AcceptedTime = o.AcceptedTime,
                    PickedUpTime = o.PickedUpTime,
                    DeliveredTime = o.DeliveredTime,
                    Note = o.Note,
                }
            ).ToList();

            var response = new PageResponse<OrderForPaginationDto>
            {
                Data = data,
                Page = totalPages,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
            };

            return ServiceResponse<PageResponse<OrderForPaginationDto>>.Ok(response);
        }
        return ServiceResponse<PageResponse<OrderForPaginationDto>>.Ok(
            new PageResponse<OrderForPaginationDto>
            {
                Data = new List<OrderForPaginationDto>(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = 0,
                TotalPages = 0
            });
    }

    public async Task<ServiceResponse<DriverAssignedOrder?>> GetDriverAssignedOrderAsync(int driverId)
    {
        var orderQuery = _orderReadRepository.GetDriverAssignedOrder(driverId);

        var assingedOrder = await orderQuery.Select(
            o => new DriverAssignedOrder
            {
                OrderId = o.Id,
                PartnerName = o.Partner.Name,
                PartnerLocation = $"{o.Partner.Location.Street} {o.Partner.Location.Area} {o.Partner.Location.City.ToString()}",
                CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                CustomerPhone = o.Customer.Phone,
                DeliveryLocation = $"{o.DeliveryLocation.Street} {o.DeliveryLocation.Area} {o.DeliveryLocation.City.ToString()}",
                IsReadyForPickup = o.Status == OrderStatusEnum.ReadyForPickUp ? true : false,
                Status = o.Status,
                SubTotal = o.SubTotal,
                ServiceFee = o.ServiceFee,
                DeliveryFee = o.DeliveryFees,
                TotalPayment = o.TotalPayment,
                StartEstimation = o.StartEstimation.Value,
                EndEstimation = o.EndEstimation.Value,
            }
        ).FirstOrDefaultAsync();

        if (assingedOrder == null) return ServiceResponse<DriverAssignedOrder?>.Ok(null);

        return ServiceResponse<DriverAssignedOrder?>.Ok(assingedOrder);
    }

    public async Task<ServiceResponse<OrderDetailedDto>> GetOrderDetailsAsync(int orderId)
    {
        var detailedOrderQuery = _orderReadRepository.GetOrderDetails(orderId);

        var detailedOrder = await detailedOrderQuery.Select(
            o => new OrderDetailedDto
            {
                PartnerName = o.Partner.Name,
                PartnerLocation = $"{o.Partner.Location.Street} {o.Partner.Location.Area} {o.Partner.Location.City.ToString()}",
                CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                CustomerLocation = $"{o.DeliveryLocation.Street} {o.DeliveryLocation.Area} {o.DeliveryLocation.City.ToString()}",
                DriverName = o.Driver != null ? $"{o.Driver.FirstName} {o.Driver.LastName}" : null,
                SubTotal = o.SubTotal,
                ServiceFee = o.ServiceFee,
                DeliveryFee = o.DeliveryFees,
                TotalPayment = o.TotalPayment,
                PaymentType = o.PaymentType != null ? o.PaymentType.ToString()! : "Unknown",
                AcceptedTime = o.AcceptedTime,
                DeliveredTime = o.DeliveredTime,
                PickedUpTime = o.PickedUpTime,
                PlacementTime = o.PlacementTime,
                OrderItems = o.OrderLines.Select(
                    ol => new OrderLineDetils
                    {
                        OrderItemId = ol.Id,
                        OrderItemName = ol.ProductName,
                        OrderItemPrice = ol.Price,
                        Quantity = ol.Quantity,
                        OrderItemOptions = ol.OrderLineOptions.Select(
                            olo => new OrderOptionDetails
                            {
                                OrderOptionId = olo.ProductOptionId,
                                OrderOptionName = olo.Option,
                                OrderOptionQuantity = olo.Quantity,
                                OrderOptionPrice = olo.Price
                            }).ToList()
                    }
                ).ToList()
            }
        ).FirstOrDefaultAsync();

        if (detailedOrder == null)
        {
            return ServiceResponse<OrderDetailedDto>.Failure("Order not found", 404);
        }

        return ServiceResponse<OrderDetailedDto>.Ok(detailedOrder);
    }

    public async Task<ServiceResponse<PageResponse<OrderForPaginationDto>>> GetOrderHistoryAdminAsync(OrderAdminQueryDto query)
    {
        var ordersQuery = _orderReadRepository.GetAllOrders();

        if (ordersQuery != null)
        {
            if (query.Status != null)
            {
                var statuses = query.Status
                .Split(',')
                .Select(s => Enum.Parse<OrderStatusEnum>(s, true))
                .ToList();

                ordersQuery = ordersQuery.Where(o => statuses.Contains(o.Status));
            }
            if (query.From.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedAt >= query.From.Value);
            }
            if (query.To.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedAt <= query.To.Value);
            }

            ordersQuery = ordersQuery.OrderByDescending(o => o.CreatedAt);

            var totalCount = await ordersQuery
                .CountAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / query.PageSize
            );

            var pageOrders = await ordersQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

            var data = pageOrders.Select(
                o => new OrderForPaginationDto
                {
                    OrderId = o.Id,
                    Status = o.Status,
                    SubTotal = o.SubTotal,
                    ServiceFee = o.ServiceFee,
                    DeliveryFees = o.DeliveryFees,
                    TotalPayment = o.TotalPayment,
                    StartEstimation = o.StartEstimation,
                    EndEstimation = o.EndEstimation,
                    PlacementTime = o.PlacementTime,
                    AcceptedTime = o.AcceptedTime,
                    PickedUpTime = o.PickedUpTime,
                    DeliveredTime = o.DeliveredTime,
                    Note = o.Note,
                }
            ).ToList();
            var response = new PageResponse<OrderForPaginationDto>
            {
                Data = data,
                Page = totalPages,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
            };

            return ServiceResponse<PageResponse<OrderForPaginationDto>>.Ok(response);
        }
        return ServiceResponse<PageResponse<OrderForPaginationDto>>.Ok(
            new PageResponse<OrderForPaginationDto>
            {
                Data = new List<OrderForPaginationDto>(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = 0,
                TotalPages = 0
            });

    }

    public async Task<ServiceResponse<PageResponse<AdminOrderResponse>>> GetOrdersWithDetailsAsync(AdminOrderDetails query)
    {
        try
        {

            var orderQuery = _orderReadRepository.GetAllOrders();

            if (query.OrderIds != null)
            {
                var orderIds = query.OrderIds
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                orderQuery = orderQuery.Where(o => orderIds.Contains(o.Id));
            }
            if (query.From != null)
            {
                orderQuery = orderQuery.Where(o => o.CreatedAt > query.From);
            }
            if (query.To != null)
            {
                orderQuery = orderQuery.Where(o => o.CreatedAt < query.To);
            }

            if (query.PartnerIds != null)
            {
                var partnerIds = query.PartnerIds
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                orderQuery = orderQuery.Where(o => partnerIds.Contains(o.PartnerId));
            }
            if (query.OrderStatuses != null)
            {
                var statuses = query.OrderStatuses
                    .Split(',')
                    .Select(s => Enum.Parse<OrderStatusEnum>(s, true))
                    .ToList();

                orderQuery = orderQuery.Where(o => statuses.Contains(o.Status));
            }

            orderQuery = orderQuery
                .Include(o => o.DeliveryRule)
                .Include(o => o.Driver)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.OrderLineOptions);

            var totalCount = await orderQuery.CountAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / query.PageSize
            );

            var response = await orderQuery.Select(o => new AdminOrderResponse
            {
                CustomerId = o.CustomerId,
                CustomerFullName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                CustomerPhone = o.Customer.Phone,
                CustomerLocation = $"{o.DeliveryLocation.Street}, {o.DeliveryLocation.Area}, {o.DeliveryLocation.City.ToString()} ",
                PartnerId = o.PartnerId,
                PartnerName = o.Partner.Name,
                PartnerLocation = $"{o.Partner.Location.Street}, {o.Partner.Location.Area}, {o.Partner.Location.City.ToString()} ",
                CommissionRate = o.Partner.CommissionRate,
                DriverId = o.DriverId.HasValue ? o.DriverId.Value : null,
                DriverFullName = o.Driver == null
                                ? null : $"{o.Driver.FirstName} {o.Driver.LastName}",
                OrderId = o.Id,
                DomainId = o.DomainId,
                Status = o.Status.ToString(),
                Note = o.Note ?? "",
                PlacementTime = o.PlacementTime,
                AcceptedTime = o.AcceptedTime,
                PickedUpTime = o.PickedUpTime,
                DeliveredTime = o.DeliveredTime,
                SubTotal = o.SubTotal,
                DeliveryFee = o.DeliveryFees,
                ServiceFee = o.ServiceFee,
                Total = o.TotalPayment,
                PartnerCommissionAmount = o.Partner.CommissionRate * o.SubTotal,
                DeliveryRuleId = o.DeliveryRuleId,
                BaseFee = o.DeliveryRule.BaseFee,
                PerKmFee = o.DeliveryRule.PerKmFee,
                MinForFreeDelivery = o.DeliveryRule.MinTotalForFreeDelivery,
                OrderLines = o.OrderLines.Select(ol => new AdminOrderLine
                {
                    OrderLineId = ol.Id,
                    ProductId = ol.ProductId,
                    ProductName = ol.ProductName,
                    Price = ol.Price,
                    Quantity = ol.Quantity,
                    OrderLineOptions = ol.OrderLineOptions.Select(olp => new AdminOrderLineOptions
                    {
                        OrderLineOptionId = olp.Id,
                        ProductOptionId = olp.ProductOptionId,
                        Option = olp.Option,
                        Price = olp.Price,
                        Quantity = olp.Quantity,
                    }).ToList() ?? new List<AdminOrderLineOptions>()
                }).ToList()
            }).ToListAsync();

            return ServiceResponse<PageResponse<AdminOrderResponse>>.Ok(
                new PageResponse<AdminOrderResponse>
                {
                    Data = response,
                    PageSize = query.PageSize,
                    TotalPages = totalPages,
                    Page = query.PageNumber
                }
            );
        }
        catch (Exception e)
        {
            return ServiceResponse<PageResponse<AdminOrderResponse>>.Failure($"Exception in GetOrdersWithDetailsAsync {e.Message}", 400);
        }

    }

    public async Task<ServiceResponse<DataTable>> GetOrdersForExcelAsync()
    {
        try
        {
            var orders = await _orderReadRepository.GetAllOrders().ToListAsync();

            if (orders != null)
            {
                DataTable data = new DataTable();
                data.TableName = "All_Orders";

                data.Columns.Add("Delivery Location");
                data.Columns.Add("Partner Location");
                data.Columns.Add("Status");
                data.Columns.Add("Domain Id");
                data.Columns.Add("Customer FullName");
                data.Columns.Add("Partner Name");
                //data.Columns.Add("Driver Name");
                data.Columns.Add("SubTotal");
                data.Columns.Add("Service Fee");
                data.Columns.Add("Delivery Fee");
                data.Columns.Add("Total Payment");
                data.Columns.Add("Created At");
                data.Columns.Add("Placement Time");
                data.Columns.Add("Accepted Time");
                data.Columns.Add("Picked Up Time");
                data.Columns.Add("Delivered Time");

                foreach(var o in orders)
                {
                    data.Rows.Add(
                        $"{o.DeliveryLocation.Street}, {o.DeliveryLocation.Area}", 
                        $"{o.Partner.Location.Street}, {o.Partner.Location.Area}",
                        o.Status.ToString(),
                        o.DomainId,
                        $"{o.Customer.FirstName} {o.Customer.LastName}",
                        o.Partner.Name,
                        o.SubTotal,
                        o.ServiceFee,
                        o.DeliveryFees,
                        o.TotalPayment,
                        o.CreatedAt,
                        o.PlacementTime,
                        o.AcceptedTime,
                        o.PickedUpTime,
                        o.DeliveredTime
                    );
                }
                return ServiceResponse<DataTable>.Ok(data);
            }

            return ServiceResponse<DataTable>.Failure($"No orders found", 404);
        }
        catch (Exception e)
        {
            return ServiceResponse<DataTable>.Failure($"Exception in GetOrdersForExcelAsync {e.Message}", 500);
        }
    }
}


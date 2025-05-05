using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Aggregates;
using Domain.Exceptions;
using Service.Abstractions;
using Services.Specifications;
using Shared.OrderModule;

namespace Services;

internal sealed class OrderService : IOrderService
{
    private readonly ICartService _cartService;
    //private readonly IServiceManager _serviceManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;

    public OrderService(ICartService cartService,
                        IPaymentService paymentService,
                        IUnitOfWork unitOfWork,
                        IMapper mapper)
    {
        _cartService = cartService;
        _paymentService = paymentService;
        //_serviceManager = serviceManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderToReturnDto> CreateOrderAsync(OrderToCreationViewModel model, string userEmail)
    {
        //mapping address 

        var mappedShippingAddress = _mapper.Map<OrderAddress>(model.ShippingAddress);

        //1)get cart for user 

        var cart = await _cartService.GetCustomerCartAsync(model?.CartId!);

        //2) get all product from this cart 

        var productRepository = _unitOfWork.GetRepository<Product, int>();
        var orderItems = new List<OrderItem>();

        if (cart?.Items?.Any() == true)
        {
            foreach (var item in cart.Items)
            {
                var product = await productRepository.GetByIdAsync(item.Id);

                if (product is null)
                    throw new ProductNotFoundException(item.Id);


                var productInOrderItem = new ProductInOrderItem
                {
                    Id = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl

                };

                orderItems.Add(new OrderItem
                {
                    Product = productInOrderItem,
                    Quantity = item.Quantity,
                    Price = product.Price,
                });



            }
        }

        //get sum of all items in order 
        //[p1 * 5 , p2 * 6 , p3 * 2]  //price * Quantity

        var subTotal = orderItems.Sum(product => product.Price * product.Quantity);


        //get delivery method to subTotal

        var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                              .GetByIdAsync(model.DeliveryMethodId);

        if (deliveryMethod is null)
            throw new DeliverMethodNotFoundException(model.DeliveryMethodId);

        var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();


        #region old 
        //if (!string.IsNullOrEmpty(cart.PaymentIntentId))
        //{
        //    var OrderSpec = new OrderWithPaymentIntentSpec(cart.PaymentIntentId);
        //    var ExistingOrder = await OrderRepo.GetByIdWithSpecAsync(OrderSpec);
        //    OrderRepo.Remove(ExistingOrder);

        //}
        //var CardDto = await _paymentService.CreateOrUpdatePaymentIntent(cart.Id);

        #endregion

        var order = new Order
        {
            UserEmail = userEmail,
            ShippingAddress = mappedShippingAddress,
            OrderItems = orderItems,
            DeliveryMethod = deliveryMethod,
            Subtotal = subTotal,
            //PaymentIntentId = CardDto.PaymentIntentId


        };

        //i will modifying it later again  

        await OrderRepo.InsertAsync(order);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<OrderToReturnDto>(order);

    }
    public async Task<OrderToReturnDto> GetOrderForUserByIdAsync(Guid id)
    {
        var order = await _unitOfWork.GetRepository<Order, Guid>()
                                     .GetByIdWithSpecAsync(new OrderWithItemsAndDeliveryMethodSpec(id));

        if (order is null)
            throw new OrderNotFoundException(id);

        return _mapper.Map<OrderToReturnDto>(order);

    }
    public async Task<IReadOnlyList<OrderToReturnDto>> GetOrdersForUserByEmailAsync(string userEmail)
    {
        var orders = await _unitOfWork.GetRepository<Order, Guid>()
                                    .GetAllWithSpecAsync(new OrderWithItemsAndDeliveryMethodSpec(userEmail));

        return _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
    }

    public async Task<IReadOnlyList<DeliveryMethodToReturnDto>> GetAllDeliveryMethodsAsync()
      => _mapper.Map<IReadOnlyList<DeliveryMethodToReturnDto>>(
          await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync()
          );


}

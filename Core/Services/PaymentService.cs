using Domain.Contracts;
using Domain.Entities.Aggregates;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Service.Abstractions;
using Shared;
using Stripe;
using Product = Domain.Entities.Product;
using AutoMapper;
using Services.Specifications;
using Domain.Enums;
using Shared.OrderModule;


namespace Services;

internal  sealed class PaymentService  : IPaymentService
{
    
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        //private readonly ICartRepository _cartRepository;


    public PaymentService(
            IUnitOfWork unitOfWork,
            ICartRepository cartRepository,
            IConfiguration configuration,
            IMapper mapper)
    {
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _configuration = configuration;
            _mapper = mapper;
    }

        //public IConfiguration Configuration { get; }

    public async Task<CustomerCartDto?> CreateOrUpdatePaymentIntent(string CartId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSetting:Secretkey"];

            var Card = await _cartRepository.GetCartAsync(CartId);

            if (Card is null) return null;

            var shippingPrice = 0m;
            if (Card.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(Card.DeliveryMethodId.Value);
                Card.DeliveryPrice = deliveryMethod.Price;
                shippingPrice = deliveryMethod.Price;
            }

            if (Card.Items.Count() > 0)
            {
                foreach (var item in Card.Items)
                {
                    var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }


            var paymentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(Card.PaymentIntentId))  // create new paymentintentID
            {
                var CreateOptions = new PaymentIntentCreateOptions()
            {
                Amount = (long)Card.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)(shippingPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>() { "card" }

            };

                paymentIntent = await paymentService.CreateAsync(CreateOptions);

                Card.PaymentIntentId = paymentIntent.Id;
                Card.ClientSecret = paymentIntent.ClientSecret;
            }
            else// Update  existing  paymentIntent
            {
                var UpdateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Card.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)(shippingPrice * 100),
                };

                await paymentService.UpdateAsync(Card.PaymentIntentId, UpdateOptions);

                //Card.PaymentIntentId = paymentIntent.Id;
                //Card.ClientSecret = paymentIntent.ClientSecret;
            }

            await _cartRepository.CreateOrUpdateCartAsync(Card);

            return _mapper.Map<CustomerCartDto>(Card) ;
        }

    public async Task<OrderToReturnDto> UpdatePaymentIntentForSucceededOrFailed(string paymentIntent, bool flag)
    {   
        var spec = new OrderWithPaymentIntentSpec(paymentIntent);

        //should Have await
        var Order =  await _unitOfWork.GetRepository<Order, Guid>().GetByIdWithSpecAsync(spec);

        if(flag)
        {
            Order.PaymentStatus = OrderPaymentStatus.PaymentReceived;
        }
        else
        {
            Order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
        }

        _unitOfWork.GetRepository<Order, Guid>().Update(Order);

        await _unitOfWork.CompleteAsync();

        return  _mapper.Map<OrderToReturnDto>(Order) ;
    }

}

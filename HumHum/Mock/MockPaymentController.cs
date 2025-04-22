using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Aggregates;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared;
using Shared.OrderModule;
using Shared.Stripe;
using Stripe;
using Stripe.Checkout;

namespace HumHum.Mock
{
    public class MockPaymentController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IOptionsMonitor<StripeSettings> _stripeSetting;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MockPaymentController(IServiceManager serviceManager,
            IOptionsMonitor<StripeSettings> stripeSetting,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _serviceManager = serviceManager;
            _stripeSetting = stripeSetting;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> CreateOrUpdatePaymentIntent(OrderToCreationViewModel orderToCreate)
        {

            var UEmail = _serviceManager.UserServices.UserEmail;
            var res = await _serviceManager.OrderService.CreateOrderAsync(orderToCreate, UEmail!);
            Console.WriteLine(orderToCreate);
            return View();
        }



        #region test payment 


        [HttpPost]
        public async Task<IActionResult> CreateOrderSession(OrderToCreationViewModel model)
        {
            Console.WriteLine(model);
            var userEmail = _serviceManager.UserServices.UserEmail;


            var user = await _serviceManager.UserServices.GetCurrentUserAsync();


            var createdOrder = await _serviceManager.OrderService.CreateOrderAsync(model, userEmail!);

            Console.WriteLine(createdOrder.Subtotal);
            Console.WriteLine(createdOrder.Total);



            var currency = "usd"; // Currency code
            var successUrl = $"https://localhost:7155/MockPayment/OrderCreatedSuccessfully/{createdOrder.Id}";
            var cancelUrl = "https://localhost:7155/MockPayment/cancel";

            StripeConfiguration.ApiKey = _stripeSetting.CurrentValue.SecretKey;

            var options = new SessionCreateOptions
            {

                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                    {

                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = currency,
                                UnitAmount = (long)((createdOrder.Total) * 100), // Amount in smallest currency unit (e.g., cents)
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {

                                    Name = user?.UserName,
                                    Description = "Product Description"

                                },

                            },
                            Quantity = 1,
                        }
                    },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };


            var service = new SessionService();
            var session = service.Create(options);


            return Redirect(session.Url);

            throw new NotImplementedException();
        }


        [HttpGet]
        public async Task<IActionResult> OrderCreatedSuccessfully(Guid id)
        {



            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(id);


            if (order is not null)
            {

                var cartId = _serviceManager.UserServices.Id;

                order.PaymentStatus = OrderPaymentStatus.PaymentReceived;


                var customerCartDto =
                    new CustomerCartDto(cartId!, new List<CartItemDto>(), null, null, null, null);

                var removeOldCart = _serviceManager.CartService
                    .UpdateCustomerCartAsync(customerCartDto);

            }

            await _unitOfWork.CompleteAsync();






            return View();
        }










        #endregion







    }
}

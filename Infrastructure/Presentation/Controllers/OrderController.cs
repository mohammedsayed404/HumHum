using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.OrderModule;

namespace Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public OrderController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderToCreationViewModel model)
        {
            var UserEmail = _serviceManager.UserServices.UserEmail;
            if (UserEmail == null) return BadRequest();

            //var model = new OrderToCreationViewModel(cartId,)
            await _serviceManager.OrderService.CreateOrderAsync(model, UserEmail);

            return View();
        }
    }
}

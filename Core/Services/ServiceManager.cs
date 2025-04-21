using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared.Cloudinary;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IPhotoService> _lazyPhotoService;
    private readonly Lazy<IProductService> _lazyProductService;
    private readonly Lazy<IProductCategoryService> _lazyProductCategoryService;
    private readonly Lazy<ICartService> _lazyCartService;

    private readonly Lazy<IRestaurantService> _lazyRestaurantService;

    private readonly Lazy<IOrderService> _lazyOrderService;

    private readonly Lazy<IPaymentService> _lazyPaymentService;

    private readonly Lazy<IUserServices> _lazyUserServices;

    public ServiceManager(
        IOptionsMonitor<CloudinarySettings> config,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICartRepository cartRepository,
        //IServiceManager serviceManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager
        )
    {


        _lazyPhotoService = new(() => new PhotoService(config));

        _lazyProductService = new(() => new ProductService(unitOfWork, mapper, PhotoService));

        _lazyProductCategoryService = new(() => new ProductCategoryService(unitOfWork, mapper, PhotoService));

        _lazyCartService = new(() => new CartService(cartRepository, mapper));


        _lazyRestaurantService = new(() => new RestaurantService(unitOfWork, mapper, PhotoService));

        _lazyOrderService = new(() => new OrderService(CartService, PaymentService, unitOfWork, mapper));

        _lazyPaymentService = new(() => new PaymentService(unitOfWork, cartRepository, configuration, mapper));


        _lazyUserServices = new(() => new UserServices(httpContextAccessor, unitOfWork, mapper, userManager));

    }

    public IPhotoService PhotoService => _lazyPhotoService.Value;

    public IProductService ProductService => _lazyProductService.Value;

    public ICartService CartService => _lazyCartService.Value;

    public IRestaurantService RestaurantService => _lazyRestaurantService.Value;

    public IUserServices UserServices => _lazyUserServices.Value;

    public IOrderService OrderService => _lazyOrderService.Value;

    public IPaymentService PaymentService => _lazyPaymentService.Value;

    public IProductCategoryService ProductCategoryService => _lazyProductCategoryService.Value;
}

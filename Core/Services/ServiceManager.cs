using AutoMapper;
using Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared.Cloudinary;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IPhotoService> _lazyPhotoService;
    private readonly Lazy<IProductService> _lazyProductService;
    private readonly Lazy<ICartService> _lazyCartService;

    private readonly Lazy<IRestaurantService> _lazyRestaurantService;

    private readonly Lazy<IOrderService> _lazyOrderService;

    private readonly Lazy<IPaymentService> _lazyPaymentService;


    public ServiceManager(
        IOptionsMonitor<CloudinarySettings> config,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICartRepository cartRepository,
        IConfiguration configuration
        )
    {


        _lazyPhotoService = new(() => new PhotoService(config));

        _lazyProductService = new(() => new ProductService(unitOfWork, mapper, PhotoService));

        _lazyCartService = new(() => new CartService(cartRepository, mapper));


        _lazyRestaurantService = new(() => new RestaurantService(unitOfWork, mapper, PhotoService));

        _lazyOrderService = new(() => new OrderService(CartService, unitOfWork, mapper));

        _lazyPaymentService = new(() => new PaymentService(unitOfWork, cartRepository, configuration, mapper));


    }

    public IPhotoService PhotoService => _lazyPhotoService.Value;

    public IProductService ProductService => _lazyProductService.Value;

    public ICartService CartService => _lazyCartService.Value;


    public IRestaurantService RestaurantService => _lazyRestaurantService.Value;

    public IOrderService OrderService => _lazyOrderService.Value;

    public IPaymentService PaymentService => _lazyPaymentService.Value;


}

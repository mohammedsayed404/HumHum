using AutoMapper;
using Domain.Contracts;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared.Cloudinary;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IPhotoService> _lazyPhotoService;
    private readonly Lazy<IProductService> _lazyProductService;

    public ServiceManager(
        IOptionsMonitor<CloudinarySettings> config,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {

        _lazyPhotoService = new(() => new PhotoService(config));

        _lazyProductService = new(() => new ProductService(unitOfWork, mapper));




    }

    public IPhotoService PhotoService => _lazyPhotoService.Value;

    public IProductService ProductService => _lazyProductService.Value;
}

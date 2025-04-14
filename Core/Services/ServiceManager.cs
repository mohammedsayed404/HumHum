using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IPhotoService> _lazyPhotoService;

    public ServiceManager(
        IOptionsMonitor<CloudinarySettings> config)
    {

        _lazyPhotoService = new Lazy<IPhotoService>(() => new PhotoService(config));






    }

    public IPhotoService PhotoService => _lazyPhotoService.Value;
}

using AutoMapper;
using Microsoft.Extensions.Options;
using Shared.Cloudinary;
namespace Services.MappingProfiles;

internal sealed class PhotoResolver<TSource, TDestination>
       : IValueResolver<TSource, TDestination, string>
{
    private readonly IOptionsMonitor<CloudinarySettings> _config;

    public PhotoResolver(IOptionsMonitor<CloudinarySettings> config)
    {
        _config = config;
    }
    public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
    {

        dynamic src = source;

        if (!String.IsNullOrWhiteSpace(src.ImageUrl))
            return $"{_config.CurrentValue.CloudinaryBaseUrl}{src.ImageUrl}";

        return string.Empty;

    }
}



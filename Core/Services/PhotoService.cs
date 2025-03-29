using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared;

namespace Services;

internal sealed class PhotoService : IPhotoService
{

    public PhotoService(IOptionsMonitor<CloudinarySettings> config)
    {

    }

    public Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        throw new NotImplementedException();
    }
}

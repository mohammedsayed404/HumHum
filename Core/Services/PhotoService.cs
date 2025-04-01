using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared.Cloudinary;

namespace Services;

internal sealed class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    private readonly IOptionsMonitor<CloudinarySettings> _config;

    public PhotoService(IOptionsMonitor<CloudinarySettings> config)
    {
        var account = new Account
        {
            Cloud = config.CurrentValue.CloudName,
            ApiKey = config.CurrentValue.ApiKey,
            ApiSecret = config.CurrentValue.ApiSecret,

        };

        _cloudinary = new Cloudinary(account);
        _config = config;
    }

    public async Task<PhotoUploadedResult> AddPhotoAsync(IFormFile file, string folderName = "HumHum")
    {
        var uploadResult = new ImageUploadResult();

        if (file?.Length > 0)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
                                 .Height(500).Width(500).Crop("fill")
                                 .Gravity("face"),
                Folder = folderName
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        if (uploadResult.Error is not null)
            return null!; // we can log error best of this . [i will cover it later ]



        string imageUrl = uploadResult.SecureUrl.AbsoluteUri;

        string imageName = string.Empty;


        var CloudinaryBaseUrl = _config.CurrentValue.CloudinaryBaseUrl;

        if (imageUrl.StartsWith(CloudinaryBaseUrl))
            imageName = imageUrl.Substring(CloudinaryBaseUrl.Length);


        return new PhotoUploadedResult(imageName, uploadResult.PublicId);


    }

    public async Task<bool> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error is not null)
            return false;

        return true;
    }
}

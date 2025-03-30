using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Shared.Cloudinary;

namespace Service.Abstractions;

public interface IPhotoService
{
    Task<PhotoUploadedResult> AddPhotoAsync(IFormFile file, string folderName = "HumHum");
    Task<DeletionResult> DeletePhotoAsync(string publicId);

}

namespace Domain.Exceptions.Photo;

public sealed class PhotoUploadedException : PhotoException
{
    public PhotoUploadedException()
        : base($"can't upload this image to product  pls try again later")
    {

    }
}

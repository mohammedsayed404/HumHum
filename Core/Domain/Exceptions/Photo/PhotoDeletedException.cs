namespace Domain.Exceptions.Photo;

public sealed class PhotoDeletedException : PhotoException
{
    public PhotoDeletedException(string publicId)
        : base($"can't delete this image {publicId} pls try again later")
    {

    }
}

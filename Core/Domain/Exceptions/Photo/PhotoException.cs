namespace Domain.Exceptions.Photo;

public abstract class PhotoException : Exception
{
    public PhotoException(string message) : base(message)
    {

    }
}

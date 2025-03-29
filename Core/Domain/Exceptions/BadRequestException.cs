namespace Domain.Exceptions;

public abstract class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {

    }
}

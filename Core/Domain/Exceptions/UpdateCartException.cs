namespace Domain.Exceptions;

public sealed class UpdateCartException : Exception
{
    public UpdateCartException(string id) :
        base($"Can't Updated Cart with id : {id}")
    {

    }
}

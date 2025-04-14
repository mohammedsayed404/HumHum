namespace Domain.Exceptions;

public sealed class CartNotFoundException : NotFoundException
{
    public CartNotFoundException(string id)
        : base($"cart with id {id} is not found ")
    {
    }
}

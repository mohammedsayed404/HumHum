namespace Domain.Exceptions;

public sealed class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid id)
        : base($"The Order with Id : {id} was Not Found.")
    {
    }
}

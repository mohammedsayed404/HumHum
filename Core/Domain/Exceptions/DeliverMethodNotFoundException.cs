namespace Domain.Exceptions;

public sealed class DeliverMethodNotFoundException : NotFoundException
{
    public DeliverMethodNotFoundException(int id)
        : base($"The Delivery Method  with Id {id} was Not Found.")
    {
    }
}

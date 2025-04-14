namespace Domain.Exceptions;

public sealed class RestaurantNotFoundException : NotFoundException
{
    public RestaurantNotFoundException(int id)
        : base($"The Restaurant  with Id {id} was Not Found.")
    {
    }
}

namespace Domain.Exceptions;

public sealed class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(int id)
        : base($"The Product with Id : {id} was Not Found.")
    {

    }
}

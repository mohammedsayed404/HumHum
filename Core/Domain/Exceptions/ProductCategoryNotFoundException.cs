namespace Domain.Exceptions;

public sealed class ProductCategoryNotFoundException : NotFoundException
{
    public ProductCategoryNotFoundException(int id)
        : base($"The Category  with Id {id} was Not Found.")
    {
    }
}

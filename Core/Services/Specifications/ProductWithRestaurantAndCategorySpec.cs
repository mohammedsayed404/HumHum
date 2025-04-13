using Domain.Contracts;
using Domain.Entities;

namespace Services.Specifications;

internal class ProductWithRestaurantAndCategorySpec : SpecificationsBase<Product>
{
    public ProductWithRestaurantAndCategorySpec(int id)
        : base(product => product.Id == id)
    {
        ApplyIncludes();
    }



    public ProductWithRestaurantAndCategorySpec()
        : base()
    {
        ApplyIncludes();
    }


    private void ApplyIncludes()
    {
        AddIncludes(product => product.Restaurant);
        AddIncludes(product => product.Category);
    }
}

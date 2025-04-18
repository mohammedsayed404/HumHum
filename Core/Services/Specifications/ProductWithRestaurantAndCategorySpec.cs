using Domain.Contracts;
using Domain.Entities;
using Shared;

namespace Services.Specifications;

internal class ProductWithRestaurantAndCategorySpec : SpecificationsBase<Product>
{
    public ProductWithRestaurantAndCategorySpec(int id)
        : base(product => product.Id == id)
    {
        ApplyIncludes();
    }



    public ProductWithRestaurantAndCategorySpec(ProductParameterRequest request)
        : base(product =>

            (request.RestaurantId.HasValue == true ? product.RestaurantId == request.RestaurantId : true) &&
            (request.CategoryId.HasValue == true ? product.CategoryId == request.CategoryId : true) &&
            (String.IsNullOrWhiteSpace(request.Search) || product.Name.Contains(request.Search))

        )
    {



        ApplyIncludes();
    }


    private void ApplyIncludes()
    {
        AddIncludes(product => product.Restaurant);
        AddIncludes(product => product.Category);
    }
}

using Domain.Contracts;
using Domain.Entities;

namespace Services.Specifications;

internal sealed class ProductWithTopRatingSpec : SpecificationsBase<Product>
{
    public ProductWithTopRatingSpec(int count) : base()
    {
        //context.products.select().orderByDescending(p=>p.Rate).Take(count)
        AddIncludes(p => p.Restaurant);
        ApplyOrderByDescending(p => p.Rate);
        ApplyTake(count);
    }
}


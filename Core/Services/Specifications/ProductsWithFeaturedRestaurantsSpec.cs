using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductsWithFeaturedRestaurantsSpec : SpecificationsBase<Product>
    {
        public ProductsWithFeaturedRestaurantsSpec(int count)
        {
            AddIncludes(p => p.Restaurant);
            ApplyOrderByDescending(p => p.Rate);
            ApplyTake(count);

        }
    }
}

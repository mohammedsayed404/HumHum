using Domain.Contracts;
using Domain.Entities;

namespace Services.Specifications
{
    internal class ProductForCertainRestorantSpec : SpecificationsBase<Product>
    {
        public ProductForCertainRestorantSpec(int restaurantId) : base(p => p.RestaurantId == restaurantId)
        {
            // .Where(p=>p.resturantID == id)
            AddIncludes(p => p.Category);
            AddIncludes(p => p.Restaurant);
        }
    }
}

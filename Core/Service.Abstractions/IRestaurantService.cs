using Shared;

namespace Service.Abstractions;

public interface IRestaurantService
{
    ///Create ,  , Delete and so on 


    public Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync();
    //public Task<IReadOnlyList<ProductToReturnDto>> GetRestaurantProductsAsync();
    public Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsOfRestorantById(int RestorantId);
























    //public Task<ProductToReturnDto> GetProductByIdAsync(int id);

    //public Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync();

    //public Task<IReadOnlyList<ProductCategoryToReturnDto>> GetAllCategoriesAsync();


    //public Task<int> CreateProductAsync(ProductToCreationViewModel model);

    //public Task<int> UpdateProductAsync(ProductToUpdateViewModel model);

    //public Task<int> DeleteProductAsync(int id);

}

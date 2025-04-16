using Shared;
using Shared.ViewModels;

namespace Service.Abstractions;

public interface IRestaurantService
{
    ///Create ,  , Delete and so on 


    public Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync();
    //public Task<IReadOnlyList<ProductToReturnDto>> GetRestaurantProductsAsync();
    public Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsOfRestorantById(int RestorantId);
    public Task<RestaurantToReturnDto> GetRestaurantByIdAsync(int id);
    public Task<int> CreateRestaurantAsync(RestaurantToCreationViewModel model);
    public Task<int> UpdateRestaurantAsync(RestaurantToUpdateViewModel model);
    public Task<int> DeleteRestaurantAsync(int id);






















    //public Task<ProductToReturnDto> GetProductByIdAsync(int id);

    //public Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync();

    //public Task<IReadOnlyList<ProductCategoryToReturnDto>> GetAllCategoriesAsync();


    //public Task<int> CreateProductAsync(ProductToCreationViewModel model);

    //public Task<int> UpdateProductAsync(ProductToUpdateViewModel model);

    //public Task<int> DeleteProductAsync(int id);

}

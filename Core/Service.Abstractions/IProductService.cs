using Shared;
using Shared.ViewModels;

namespace Service.Abstractions;

public interface IProductService
{

    public Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsAsync(ProductParameterRequest request);

    public Task<ProductToReturnDto> GetProductByIdAsync(int id);

    public Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync();

    public Task<IReadOnlyList<ProductCategoryToReturnDto>> GetAllCategoriesAsync();

    public Task<int> CreateProductAsync(ProductToCreationViewModel model);

    public Task<int> UpdateProductAsync(ProductToUpdateViewModel model);

    public Task<int> DeleteProductAsync(int id);

    public Task<IReadOnlyList<ProductToReturnDto>> GetTopRatingProductsAsync(int count);
    public Task<IReadOnlyList<ProductWithRestaurantToReturnDto>> GetProductsWithFeaturedRestaurantsAsync(int count);

}

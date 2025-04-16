using HumHum.ViewModels;
using Shared;

namespace Service.Abstractions;

public interface IProductService
{

    public Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsAsync();

    public Task<ProductToReturnDto> GetProductByIdAsync(int id);

    public Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync();

    public Task<IReadOnlyList<ProductCategoryToReturnDto>> GetAllCategoriesAsync();

    public Task<int> CreateProductAsync(ProductToCreationViewModel model);

    public Task<int> UpdateProductAsync(ProductToUpdateViewModel model);

    public Task<int> DeleteProductAsync(int id);

    public Task<IReadOnlyList<ProductToReturnDto>> GetTopRatingProductsAsync(int count);

}

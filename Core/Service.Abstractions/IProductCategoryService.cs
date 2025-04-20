using Shared;
using Shared.ViewModels;

namespace Service.Abstractions;

public interface IProductCategoryService
{
    public Task<ProductCategoryToReturnDto> GetProductCategoryByIdAsync(int id);
    public Task<int> CreateProductCategoryAsync(ProductCategoryToCreationViewModel model);
    public Task<int> UpdateProductCategoryAsync(ProductCategoryToUpdateViewModel model);
    public Task<int> DeleteProductCategoryAsync(int id);
}

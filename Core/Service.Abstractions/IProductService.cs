using Shared;

namespace Service.Abstractions;

public interface IProductService
{

    public Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsAsync();


}

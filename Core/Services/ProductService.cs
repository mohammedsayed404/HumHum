using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Service.Abstractions;
using Services.Specifications;
using Shared;

namespace Services;

internal sealed class ProductService : IProductService
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.GetRepository<Product, int>()
                                        .GetAllWithSpecAsync(new ProductWithRestaurantAndCategorySpec());


        var mappedProducts = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

        return mappedProducts;
    }
}
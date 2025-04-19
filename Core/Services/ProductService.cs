using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Photo;
using Service.Abstractions;
using Services.Specifications;
using Shared;
using Shared.Cloudinary;
using Shared.ViewModels;

namespace Services;

internal sealed class ProductService : IProductService
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly IPhotoService _photoService;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _photoService = photoService;
    }

    public async Task<IReadOnlyList<ProductCategoryToReturnDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();

        var mappedCategories = _mapper.Map<IReadOnlyList<ProductCategoryToReturnDto>>(categories);

        return mappedCategories;
    }

    public async Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsAsync(ProductParameterRequest request)
    {
        var products = await _unitOfWork.GetRepository<Product, int>()
                                        .GetAllWithSpecAsync(new ProductWithRestaurantAndCategorySpec(request));


        var mappedProducts = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

        return mappedProducts;
    }

    public async Task<ProductToReturnDto> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.GetRepository<Product, int>()
                                  .GetByIdWithSpecAsync(new ProductWithRestaurantAndCategorySpec(id));

        return product is not null ? _mapper.Map<ProductToReturnDto>(product) :
            throw new ProductNotFoundException(id);
    }


    public async Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync()
    {
        var restaurants = await _unitOfWork.GetRepository<Restaurant, int>().GetAllAsync();

        var mappedRestaurants = _mapper.Map<IReadOnlyList<RestaurantToReturnDto>>(restaurants);

        return mappedRestaurants;
    }

    public async Task<int> CreateProductAsync(ProductToCreationViewModel model)
    {
        var ImageUploaded = await _photoService.AddPhotoAsync(model.Image, nameof(Product));

        if (ImageUploaded is null)
            throw new PhotoUploadedException(); // or we can return -1

        var mappedProduct = _mapper.Map<Product>(model);

        mappedProduct.ImageUrl = ImageUploaded.ImageName;
        mappedProduct.PublicImageId = ImageUploaded.PublicId;

        await _unitOfWork.GetRepository<Product, int>().InsertAsync(mappedProduct);
        try
        {
            return await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            //need more work like delete photo from cloud. [will cover it later  ]
            Console.WriteLine(ex.Message);
            throw; //will make it later 
        }


    }

    public async Task<int> DeleteProductAsync(int id)
    {
        var productRepo = _unitOfWork.GetRepository<Product, int>();

        var product = await productRepo.GetByIdAsync(id);

        if (product is null)
            throw new ProductNotFoundException(id);

        productRepo.Remove(product);

        try
        {
            //we are using soft delete so i don't delete it from cloud ok zehahha 😂
            // await _serviceManager.PhotoService.DeletePhotoAsync(product.PublicImageId);
            return await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }


    }

    public async Task<int> UpdateProductAsync(ProductToUpdateViewModel model)
    {

        bool deletedImage = false;

        if (model.Image is not null)
        {

            deletedImage = await _photoService.DeletePhotoAsync(model.PublicImageId!);

            if (!deletedImage)
                throw new PhotoDeletedException(model.PublicImageId!);

        }


        PhotoUploadedResult ImageUploaded = default!;

        if (model.Image is not null)
        {
            ImageUploaded = await _photoService.AddPhotoAsync(model.Image, nameof(Product));

            if (ImageUploaded is null)
                throw new PhotoUploadedException();


        }


        var mappedProduct = _mapper.Map<Product>(model);

        if (ImageUploaded is not null)
        {
            mappedProduct.ImageUrl = ImageUploaded.ImageName;
            mappedProduct.PublicImageId = ImageUploaded.PublicId;

        }

        _unitOfWork.GetRepository<Product, int>().Update(mappedProduct);
        try
        {
            return await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }

    public async Task<IReadOnlyList<ProductToReturnDto>> GetTopRatingProductsAsync(int count)
    {
        var products = await _unitOfWork
            .GetRepository<Product, int>()
            .GetAllWithSpecAsync(new ProductWithTopRatingSpec(count));

        var mappedProducts = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

        return mappedProducts;
    }

    public async Task<IReadOnlyList<ProductWithRestaurantToReturnDto>> GetProductsWithFeaturedRestaurantsAsync(int count)
    {
        var products = await _unitOfWork
            .GetRepository<Product, int>()
            .GetAllWithSpecAsync(new ProductsWithFeaturedRestaurantsSpec(count));



        var mappedProducts = _mapper.Map<IReadOnlyList<ProductWithRestaurantToReturnDto>>(products);

        return mappedProducts;
    }
}
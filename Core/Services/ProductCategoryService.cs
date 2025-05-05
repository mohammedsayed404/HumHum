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

internal sealed class ProductCategoryService: IProductCategoryService
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly IPhotoService _photoService;

    public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _photoService = photoService;
    }

    public async Task<ProductCategoryToReturnDto> GetProductCategoryByIdAsync(int id)
    {
        var productCategory = await _unitOfWork.GetRepository<ProductCategory, int>().GetByIdAsync(id);

        return productCategory is not null ? _mapper.Map<ProductCategoryToReturnDto>(productCategory) :
            throw new ProductCategoryNotFoundException(id);
    }
    public async Task<int> CreateProductCategoryAsync(ProductCategoryToCreationViewModel model)
    {
        var ImageUploaded = await _photoService.AddPhotoAsync(model.Image, nameof(ProductCategory));

        if (ImageUploaded is null)
            throw new PhotoUploadedException();

        var mappedProductCategory = _mapper.Map<ProductCategory>(model);

        mappedProductCategory.ImageUrl = ImageUploaded.ImageName;
        mappedProductCategory.PublicImageId = ImageUploaded.PublicId;

        await _unitOfWork.GetRepository<ProductCategory, int>().InsertAsync(mappedProductCategory);
        try
        {
            return await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            //need more work like delete photo from cloud. [will cover it later 
            Console.WriteLine(ex.Message);
            throw; //will make it later 
        }
    }

    public async Task<int> UpdateProductCategoryAsync(ProductCategoryToUpdateViewModel model)
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


        var mappedProductCategory = _mapper.Map<ProductCategory>(model);

        if (ImageUploaded is not null)
        {
            mappedProductCategory.ImageUrl = ImageUploaded.ImageName;
            mappedProductCategory.PublicImageId = ImageUploaded.PublicId;

        }

        _unitOfWork.GetRepository<ProductCategory, int>().Update(mappedProductCategory);
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

    public async Task<int> DeleteProductCategoryAsync(int id)
    {
        var productCategoryRepo = _unitOfWork.GetRepository<ProductCategory, int>();

        var productCategory = await productCategoryRepo.GetByIdAsync(id);

        if (productCategory is null)
            throw new ProductCategoryNotFoundException(id);

        productCategoryRepo.Remove(productCategory);

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
}

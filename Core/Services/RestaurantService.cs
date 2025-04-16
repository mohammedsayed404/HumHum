
﻿using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Photo;
using Service.Abstractions;
using Services.Specifications;
using Shared;
using Shared.Cloudinary;
using Shared.ViewModels;

namespace Services
{
    internal sealed class RestaurantService : IRestaurantService
    {

        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IPhotoService _photoService;

        public RestaurantService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }


        //public async Task<IReadOnlyList<ProductCategoryToReturnDto>> GetAllCategoriesAsync()
        //{
        //    var categories = await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();

        //    var mappedCategories = _mapper.Map<IReadOnlyList<ProductCategoryToReturnDto>>(categories);

        //    return mappedCategories;
        //}

        public async Task<IReadOnlyList<RestaurantToReturnDto>> GetAllRestaurantsAsync()
        {
            var resturants = await _unitOfWork.GetRepository<Restaurant, int>().GetAllAsync();

            var mappedresturants = _mapper.Map<IReadOnlyList<RestaurantToReturnDto>>(resturants);

            return mappedresturants;
        }

        //public async Task<IReadOnlyList<ProductToReturnDto>> GetRestaurantProductsAsync()
        //{
        //    var restaurantProducts = await _unitOfWork.GetRepository<Product, int>().GetAllAsync();

        //    var mappedrestaurantProducts = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(restaurantProducts);

        //    return mappedrestaurantProducts;
        //}

        public async Task<IReadOnlyList<ProductToReturnDto>> GetAllProductsOfRestorantById(int restaurantId)
        {
            var Products = await _unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(new ProductForCertainRestorantSpec(restaurantId));

            var mappedrestaurantProducts = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(Products);

            return mappedrestaurantProducts;
        }

        public async Task<int> CreateRestaurantAsync(RestaurantToCreationViewModel model)
        {
            var ImageUploaded = await _photoService.AddPhotoAsync(model.Image, nameof(Restaurant));

            if (ImageUploaded is null)
                throw new PhotoUploadedException();

            var mappedRestaurant = _mapper.Map<Restaurant>(model);

            mappedRestaurant.ImageUrl = ImageUploaded.ImageName;
            mappedRestaurant.PublicImageId = ImageUploaded.PublicId;

            await _unitOfWork.GetRepository<Restaurant, int>().InsertAsync(mappedRestaurant);

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

        public async Task<int> UpdateRestaurantAsync(RestaurantToUpdateViewModel model)
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
                ImageUploaded = await _photoService.AddPhotoAsync(model.Image, nameof(Restaurant));

                if (ImageUploaded is null)
                    throw new PhotoUploadedException();


            }


            var mappedRestaurant = _mapper.Map<Restaurant>(model);

            if (ImageUploaded is not null)
            {
                mappedRestaurant.ImageUrl = ImageUploaded.ImageName;
                mappedRestaurant.PublicImageId = ImageUploaded.PublicId;

            }

            _unitOfWork.GetRepository<Restaurant, int>().Update(mappedRestaurant);
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

        public async Task<int> DeleteRestaurantAsync(int id)
        {
            var restaurantRepo = _unitOfWork.GetRepository<Restaurant, int>();

            var restaurant = await restaurantRepo.GetByIdAsync(id);

            if (restaurant is null)
                throw new RestaurantNotFoundException(id);

            restaurantRepo.Remove(restaurant);

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

        public async Task<RestaurantToReturnDto> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _unitOfWork.GetRepository<Restaurant, int>().GetByIdAsync(id);

            return restaurant is not null ? _mapper.Map<RestaurantToReturnDto>(restaurant) :
                throw new RestaurantNotFoundException(id);
        }
    }
}

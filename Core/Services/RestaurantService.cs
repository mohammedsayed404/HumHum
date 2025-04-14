
﻿using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Service.Abstractions;
using Services.Specifications;
using Shared;

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
    }

}

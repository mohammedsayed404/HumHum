using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Service.Abstractions;
using Services.Specifications;
using Shared;
using Shared.ViewModels;
using System.Security.Claims;

namespace Services;

internal sealed class UserServices : IUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserServices(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    //public string? cartid => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.d);

    public async Task<AddressToReturnDto> GetUserAddressAsync(string userId)
    {

        var address = await _unitOfWork.GetRepository<Address, int>()
                                       .GetByIdWithSpecAsync(new AddressForUserSpec(userId));

        var mappedAddress = _mapper.Map<AddressToReturnDto>(address);

        return mappedAddress;

    }

    public async Task<int> UpdateUserAddressAsync(AddressToUpdateViewModel model)
    {
        var addressRepository = _unitOfWork.GetRepository<Address, int>();

        var mappedAddress = _mapper.Map<Address>(model);

        addressRepository.Update(mappedAddress);


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






}

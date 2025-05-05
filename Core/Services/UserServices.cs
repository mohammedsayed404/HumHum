using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    private readonly UserManager<ApplicationUser> _userManager;

    public UserServices(IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public async Task<ApplicationUserToReturnDto> GetCurrentUserAsync()
    {
        var user = await _userManager.FindByIdAsync(Id!);

        return _mapper.Map<ApplicationUserToReturnDto>(user);
    }

    public async Task<ApplicationUserToReturnDto> GetCurrentUserWithAddressAsync()
    {

        var user = await _userManager.Users.Include(user => user.Address)
                                            .FirstOrDefaultAsync(user => user.Id == Id);

        return _mapper.Map<ApplicationUserToReturnDto>(user);

    }



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

    public async Task<bool> UpdateUserInformationAsync(ApplicationUserToUpdateViewModule model)
    {

        var user = await _userManager.FindByIdAsync(model.Id);

        var mappedUser = _mapper.Map(model, user);

        var result = await _userManager.UpdateAsync(user!);

        return result.Succeeded;


    }
}

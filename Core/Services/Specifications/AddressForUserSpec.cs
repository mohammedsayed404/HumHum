using Domain.Contracts;
using Domain.Entities.Identity;

namespace Services.Specifications;

internal sealed class AddressForUserSpec : SpecificationsBase<Address>
{

    public AddressForUserSpec(string userId)
        : base(address => address.ApplicationUserId == userId)
    {

    }

}

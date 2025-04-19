using FluentValidation;
using Shared.ViewModels;

namespace HumHum.Validators
{
    public class RestaurantToUpdateValidator : AbstractValidator<RestaurantToUpdateViewModel>
    {
        public RestaurantToUpdateValidator()
        {
            RuleFor(restaurant => restaurant.Name).NotEmpty().NotNull()
            .WithMessage("The Name Of Restaurant is Required")
            .MaximumLength(50);

            RuleFor(restaurant => restaurant.Image).NotEmpty().NotNull();
        }
    }
}

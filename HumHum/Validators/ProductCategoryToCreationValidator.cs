using FluentValidation;
using Shared.ViewModels;

namespace HumHum.Validators
{
    public sealed class ProducCategorytToCreationValidator : AbstractValidator<ProductCategoryToCreationViewModel>
    {

        public ProducCategorytToCreationValidator()
        {
            RuleFor(product => product.Name).NotEmpty().NotNull()
                .WithMessage("The Name Of Product Category is Required")
                .MaximumLength(50);

            RuleFor(productCategory => productCategory.Image).NotEmpty().NotNull();
        }
    }

}

using FluentValidation;
using Shared.ViewModels;

namespace HumHum.Validators
{
    public sealed class ProducCategorytToUpdatealidator : AbstractValidator<ProductCategoryToUpdateViewModel>
    {

        public ProducCategorytToUpdatealidator()
        {
            RuleFor(product => product.Name).NotEmpty().NotNull()
                .WithMessage("The Name Of Product Category is Required")
                .MaximumLength(50);

            RuleFor(productCategory => productCategory.Image).NotEmpty().NotNull();
        }
    }
}

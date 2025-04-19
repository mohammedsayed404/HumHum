using FluentValidation;
using Shared.ViewModels;

namespace HumHum.Validators;

public sealed class ProductToCreationValidator : AbstractValidator<ProductToCreationViewModel>
{

    public ProductToCreationValidator()
    {
        RuleFor(product => product.Name).NotEmpty().NotNull()
            .WithMessage("The Name Of Product is Required")
            .MaximumLength(50);


        RuleFor(product => product.Price)
            .InclusiveBetween(1, int.MaxValue)
            .WithMessage("The Price Must Be Grater than 0");


        RuleFor(product => product.Description)
             .NotEmpty().NotNull().MaximumLength(2000);


        RuleFor(product => product.Image).NotEmpty().NotNull();

        RuleFor(product => product.CategoryId).NotEmpty().NotEqual(0);


        RuleFor(product => product.RestaurantId).NotEmpty().NotEqual(0);

    }
}



///Example from  docs 

//public class CustomerValidator : AbstractValidator<Customer>
//{
//    public CustomerValidator()
//    {
//        RuleFor(x => x.Surname).NotEmpty();
//        RuleFor(x => x.Forename).NotEmpty().WithMessage("Please specify a first name");
//        RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
//        RuleFor(x => x.Address).Length(20, 250);
//        RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
//    }

//    //private bool BeAValidPostcode(string postcode)
//    //{
//    //    // custom postcode validating logic goes here
//    //}

//    //var customer = new Customer();
//    //  var validator = new CustomerValidator();

//    //  // Execute the validator.
//    //  ValidationResult results = validator.Validate(customer);

//    //  // Inspect any validation failures.
//    //  bool success = results.IsValid;
//    //  List<ValidationFailure> failures = results.Errors;

//}
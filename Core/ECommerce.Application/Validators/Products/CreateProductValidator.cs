using ECommerce.Application.Features.Commands.CreateProduct;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

            RuleFor(c => c.Price)
                .NotNull()
                .InclusiveBetween(0.1, 10000)
                .WithMessage("Price must be between 0.1 and 10000");

            RuleFor(c => c.Stock)
                .NotNull()
                .InclusiveBetween(0, 1000)
                .WithMessage("Stock must be between 0 and 1000");
        }
    }
}

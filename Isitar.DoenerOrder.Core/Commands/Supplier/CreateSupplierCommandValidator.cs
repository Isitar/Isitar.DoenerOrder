using FluentValidation;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
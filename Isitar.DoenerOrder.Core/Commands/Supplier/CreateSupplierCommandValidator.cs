using System.Data;
using FluentValidation;

namespace Isitar.DoenerOrder.Commands.Supplier
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
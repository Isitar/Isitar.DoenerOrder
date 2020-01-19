using FluentValidation;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
    {
        public DeleteSupplierCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
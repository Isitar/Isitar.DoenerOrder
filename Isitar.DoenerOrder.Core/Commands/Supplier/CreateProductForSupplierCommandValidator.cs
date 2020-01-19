using System.Linq;
using FluentValidation;
using Isitar.DoenerOrder.Core.Data;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class CreateProductForSupplierCommandValidator : AbstractValidator<CreateProductForSupplierCommand>
    {
        public CreateProductForSupplierCommandValidator(DoenerOrderContext context)
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .Must(supplierId => context.Suppliers.Any(s => s.Id == supplierId))
                .WithMessage("Supplier does not exist");
            RuleFor(x => x.Label)
                .NotEmpty();
            RuleFor(x => x.Price)
                .NotNull();
        }
    }
}
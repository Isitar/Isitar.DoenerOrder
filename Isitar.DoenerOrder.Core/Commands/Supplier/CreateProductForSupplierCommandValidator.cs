using FluentValidation;
using Isitar.DoenerOrder.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class CreateProductForSupplierCommandValidator : AbstractValidator<CreateProductForSupplierCommand>
    {
        public CreateProductForSupplierCommandValidator(DoenerOrderContext context)
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .Must(x => context.Suppliers.Find(x) != null);
            RuleFor(x => x.Label)
                .NotEmpty();
            RuleFor(x => x.Price)
                .NotNull();
        }
    }
}
using System.Linq;
using FluentValidation;
using Isitar.DoenerOrder.Core.Data;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
    {
        public DeleteSupplierCommandValidator(DoenerOrderContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(supplierId => dbContext.Suppliers.Any(s => s.Id == supplierId))
                .WithMessage("Supplier does not exist");
        }
    }
}
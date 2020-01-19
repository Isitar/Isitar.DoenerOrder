using System.Linq;
using FluentValidation;
using Isitar.DoenerOrder.Core.Data;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class DeleteProductForSupplierCommandValidator : AbstractValidator<DeleteProductForSupplierCommand>
    {
        public DeleteProductForSupplierCommandValidator(DoenerOrderContext dbContext)
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .Must(supplierId => dbContext.Suppliers.Any(s => s.Id == supplierId))
                .WithMessage("Supplier does not exist");
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .Must((command, productId) =>
                    dbContext.Products.Any(p => p.Id == productId && p.SupplierId == command.SupplierId))
                .WithMessage("Product does not exist.");
        }
    }
}
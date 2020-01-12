using System;
using System.Linq;
using FluentValidation;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class UpdateProductForSupplierCommandValidator : AbstractValidator<UpdateProductForSupplierCommand>
    {
        private DoenerOrderContext dbContext;

        public UpdateProductForSupplierCommandValidator(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .Must(supplierId => dbContext.Suppliers.Any(s => s.Id == supplierId))
                .WithMessage("Supplier does not exist");
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .Must((command, productId) =>
                    dbContext.Products.Any(p => p.Id == productId && p.SupplierId == command.SupplierId))
                .WithMessage("Product does not exist.");

            RuleFor(x => x.Label)
                .NotEmpty();
            RuleFor(x => x.Price)
                .NotNull();
        }
    }
}
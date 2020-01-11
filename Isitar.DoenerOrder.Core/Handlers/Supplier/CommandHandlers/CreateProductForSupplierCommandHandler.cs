using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers
{
    public class CreateProductForSupplierCommandHandler : IRequestHandler<CreateProductForSupplierCommand, ProductResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public CreateProductForSupplierCommandHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductResponse> Handle(CreateProductForSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await dbContext.Suppliers.FindAsync(request.SupplierId);
            var product = await dbContext.Products.AddAsync(new Data.DAO.Product
            {
                Label = request.Label.Trim(),
                Price = request.Price,
                Supplier = supplier,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ProductResponse
            {
                Data = ProductDto.FromProduct(product.Entity),
                Success =  true,
            };
        }
    }
}
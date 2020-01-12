using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers
{
    public class
        UpdateProductForSupplierCommandHandler : IRequestHandler<UpdateProductForSupplierCommand, ProductResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public UpdateProductForSupplierCommandHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductResponse> Handle(UpdateProductForSupplierCommand request,
            CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FindAsync(request.ProductId);
            if (null == product)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>
                    {
                        {nameof(request.ProductId), new[] {"No product found"}};
                    }
                };
            }

            product.Label = request.Label;
            product.Price = request.Price;
            await dbContext.SaveChangesAsync();
            return new ProductResponse
            {
                Success = true,
                Data = ProductDto.FromProduct(product),
            };
        }
    }
}
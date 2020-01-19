using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.QueryHandlers
{
    public class GetProductForSupplierByIdQueryHandler : IRequestHandler<GetProductForSupplierByIdQuery, ProductResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public GetProductForSupplierByIdQueryHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductResponse> Handle(GetProductForSupplierByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.SingleOrDefaultAsync(
                p => p.SupplierId == request.SupplierId && p.Id == request.ProductId,
                cancellationToken: cancellationToken);

            if (null == product)
            {
                if (!await dbContext.Suppliers.AnyAsync(s => s.Id == request.SupplierId))
                {
                    return new ProductResponse
                    {
                        Success = false,
                        ErrorMessages = new Dictionary<string, IList<string>>
                        {
                            {nameof(request.SupplierId), new List<string> {"Could not find supplier"}}
                        },
                    };
                }

                return new ProductResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>()
                    {
                        {nameof(request.ProductId), new List<string> {"Product does not exist for supplier"}}
                    },
                };
            }

            return new ProductResponse
            {
                Success = true,
                Data = ProductDto.FromProduct(product)
            };
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier
{
    public class
        GetAllProductsForSupplierQueryHandler : IRequestHandler<GetAllProductsForSupplierQuery, ProductsResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public GetAllProductsForSupplierQueryHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductsResponse> Handle(GetAllProductsForSupplierQuery request,
            CancellationToken cancellationToken)
        {
            var products = await dbContext.Products.Where(p => p.SupplierId == request.SupplierId)
                .Select(p => ProductDto.FromProduct(p))
                .ToListAsync(cancellationToken: cancellationToken);
            return new ProductsResponse
            {
                Success = true,
                Data = products,
            };
        }
    }
}
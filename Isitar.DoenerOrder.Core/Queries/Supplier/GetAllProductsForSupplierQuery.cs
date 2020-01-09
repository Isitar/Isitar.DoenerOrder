using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;

namespace Isitar.DoenerOrder.Core.Queries.Supplier
{
    public class GetAllProductsForSupplierQuery : IRequest<ProductsResponse>
    {
        public int SupplierId { get; set; }
    }
}
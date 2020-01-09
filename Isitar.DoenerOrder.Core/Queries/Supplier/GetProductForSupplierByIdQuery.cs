using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;

namespace Isitar.DoenerOrder.Core.Queries.Supplier
{
    public class GetProductForSupplierByIdQuery : IRequest<ProductResponse>
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
    }
}
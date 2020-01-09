using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class CreateProductForSupplierCommand : IRequest<ProductResponse>
    {
        public int SupplierId { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
    }
}
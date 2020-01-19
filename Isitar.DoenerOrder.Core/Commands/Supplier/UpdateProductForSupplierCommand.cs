using Isitar.DoenerOrder.Core.Responses;
using Isitar.DoenerOrder.Core.Responses.Product;
using MediatR;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class UpdateProductForSupplierCommand : IRequest<IntegerResponse>
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
    }
}
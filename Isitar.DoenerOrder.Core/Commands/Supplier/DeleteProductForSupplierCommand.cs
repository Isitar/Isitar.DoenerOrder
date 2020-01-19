using Isitar.DoenerOrder.Core.Responses;
using MediatR;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class DeleteProductForSupplierCommand : IRequest<BooleanResponse>
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
    }
}
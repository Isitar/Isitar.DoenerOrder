using Isitar.DoenerOrder.Core.Responses;
using MediatR;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class DeleteSupplierCommand : IRequest<BooleanResponse>
    {
        public int Id { get; set; }
    }
}
using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;

namespace Isitar.DoenerOrder.Core.Queries.Supplier
{
    public class GetSupplierByIdQuery : IRequest<SupplierResponse>
    {
        public int Id { get; set; }
    }
}
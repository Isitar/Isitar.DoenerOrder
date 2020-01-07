using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class UpdateSupplierCommand : IRequest<SupplierResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
using Isitar.DoenerOrder.Core.Responses;
using MediatR;

namespace Isitar.DoenerOrder.Core.Commands.Supplier
{
    public class CreateSupplierCommand : IRequest<IntegerResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
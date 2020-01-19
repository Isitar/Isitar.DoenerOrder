using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers
{
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, BooleanResponse>
    {
        private DoenerOrderContext context;

        public DeleteSupplierCommandHandler(DoenerOrderContext context)
        {
            this.context = context;
        }

        public async Task<BooleanResponse> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await context.Suppliers.FindAsync(request.Id);
            if (null == supplier)
            {
                return new BooleanResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>
                        {{nameof(request.Id), new[] {"Supplier not found"}}},
                };
            }

            context.Remove(supplier);
            await context.SaveChangesAsync(cancellationToken);
            return new BooleanResponse
            {
                Success = true,
            };
        }
    }
}
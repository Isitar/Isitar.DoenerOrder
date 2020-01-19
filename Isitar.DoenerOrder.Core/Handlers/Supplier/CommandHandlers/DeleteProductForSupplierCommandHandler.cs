using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers
{
    public class DeleteProductForSupplierCommandHandler: IRequestHandler<DeleteProductForSupplierCommand, BooleanResponse>
    {
        private readonly DoenerOrderContext context;

        public DeleteProductForSupplierCommandHandler(DoenerOrderContext context)
        {
            this.context = context;
        }

        public async Task<BooleanResponse> Handle(DeleteProductForSupplierCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(request.ProductId);
            if (null == product || product.SupplierId != request.SupplierId)
            {
                return new BooleanResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>
                    {
                        {nameof(request.ProductId), new[] {"No product found"}},
                    }
                };
            }

            context.Products.Remove(product);
            var changes = await context.SaveChangesAsync(cancellationToken);
            return new BooleanResponse
            {
                Success = changes > 0,
            };
        }
    }
}
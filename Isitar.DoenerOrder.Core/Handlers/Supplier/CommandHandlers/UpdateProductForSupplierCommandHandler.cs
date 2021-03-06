using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers
{
    public class
        UpdateProductForSupplierCommandHandler : IRequestHandler<UpdateProductForSupplierCommand, IntegerResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public UpdateProductForSupplierCommandHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IntegerResponse> Handle(UpdateProductForSupplierCommand request,
            CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FindAsync(request.ProductId);
            if (null == product || product.SupplierId != request.SupplierId)
            {
                return new IntegerResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>
                    {
                        {nameof(request.ProductId), new[] {"No product found"}},
                    }
                };
            }

            product.Label = request.Label;
            product.Price = request.Price;
            await dbContext.SaveChangesAsync();
            return new IntegerResponse
            {
                Success = true,
                Data = product.Id,
            };
        }
    }
}
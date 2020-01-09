using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier
{
    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierResponse>
    {
        private DoenerOrderContext dbContext;

        public GetSupplierByIdQueryHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SupplierResponse> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await dbContext.Suppliers.FindAsync(request.Id);
            if (null == supplier)
            {
                return new SupplierResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>
                    {
                        {nameof(request.Id), new List<string> {"Could not find supplier"}},
                    },
                };
            }

            return new SupplierResponse
            {
                Success = true,
                Data = SupplierDto.FromSupplier(supplier),
            };
        }
    }
}
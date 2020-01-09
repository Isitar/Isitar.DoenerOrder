using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Queries.Supplier;
using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier
{
    public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, SuppliersResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public GetAllSuppliersQueryHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SuppliersResponse> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await dbContext.Suppliers.Select(s => SupplierDto.FromSupplier(s)).ToListAsync();
            return new SuppliersResponse
            {
                Data = suppliers,
                Success = true,
            };
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public UpdateSupplierCommandHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await dbContext.Suppliers.FindAsync(request.Id);
            if (null == supplier)
            {
                return new SupplierResponse
                {
                    Success = false,
                    ErrorMessages = new Dictionary<string, IList<string>>
                    {
                        {nameof(request.Id), new List<string> {"Could not find supplier"}}
                    },
                };
            }

            supplier.Name = request.Name;
            supplier.Email = request.Email;
            supplier.Phone = request.Phone;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new SupplierResponse
            {
                Success = true,
                Data = SupplierDTO.FromSupplier(supplier)
            };
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public CreateSupplierCommandHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SupplierResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await dbContext.Suppliers.AddAsync(new Data.DAO.Supplier
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone?.Trim()
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new SupplierResponse
            {
                Data = SupplierDto.FromSupplier(supplier.Entity),
                Success =  true,
            };
        }
    }
}
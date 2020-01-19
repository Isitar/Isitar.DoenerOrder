using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using Isitar.DoenerOrder.Core.Responses;
using Isitar.DoenerOrder.Core.Responses.Supplier;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, IntegerResponse>
    {
        private readonly DoenerOrderContext dbContext;

        public CreateSupplierCommandHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IntegerResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await dbContext.Suppliers.AddAsync(new Data.DAO.Supplier
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone?.Trim()
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new IntegerResponse
            {
                Data = supplier.Entity.Id,
                Success =  true,
            };
        }
    }
}
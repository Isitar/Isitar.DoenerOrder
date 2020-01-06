using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data;
using MediatR;

namespace Isitar.DoenerOrder.Core.Handlers.Supplier
{
    public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, int>
    {
        private readonly DoenerOrderContext dbContext;

        public CreateSupplierHandler(DoenerOrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await dbContext.Suppliers.AddAsync(new Data.DAO.Supplier
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone?.Trim()
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return supplier.Entity.Id;
        }
    }
}
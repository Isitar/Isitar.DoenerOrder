using System.Collections;
using MediatR;

namespace Isitar.DoenerOrder.Queries.Supplier
{
    public class GetAllSuppliersQuery : IRequest<IEnumerable<SupplierResponse>>
    {
        
    }
}
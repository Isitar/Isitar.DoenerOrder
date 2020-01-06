using System.Collections.Generic;
using MediatR;

namespace Isitar.DoenerOrder.Core.Queries.Supplier
{
    public class GetAllSuppliersQuery : IRequest<IEnumerable<Data.DAO.Supplier>>
    {
        
    }
}
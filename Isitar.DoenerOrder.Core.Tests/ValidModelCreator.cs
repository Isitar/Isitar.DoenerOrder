using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Data.DAO;

namespace Isitar.DoenerOrder.Core.Tests
{
    public class ValidModelCreator
    {
        internal  static Data.DAO.Supplier Supplier() =>
            new Data.DAO.Supplier
            {
                Name = "Valid Supplier",
                Email = "something@something.ch",
                Phone = "+41 79 456 45 45",
            };
        
        internal static Data.DAO.Product Product(int supplierId) =>
            new Product
            {
                Label = "Product 123",
                Price = 12.5m,
                SupplierId = supplierId
            };
    }
}
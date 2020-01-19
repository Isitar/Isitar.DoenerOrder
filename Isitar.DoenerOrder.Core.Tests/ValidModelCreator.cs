using Isitar.DoenerOrder.Core.Commands.Supplier;

namespace Isitar.DoenerOrder.Core.Tests
{
    public class ValidModelCreator
    {
        public static CreateSupplierCommand CreateSupplierCommand() =>
            new CreateSupplierCommand
            {
                Name = "Valid Supplier",
                Email = "something@something.ch",
                Phone = "+41 79 456 45 45",
            };
        
        public static CreateProductForSupplierCommand CreateProductForSupplierCommand(int supplierId) =>
            new CreateProductForSupplierCommand
            {
                Label = "Product 123",
                Price = 12.5m,
                SupplierId = supplierId
            };
    }
}
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Handlers.Supplier;
using Xunit;

namespace Isitar.DoenerOrder.Core.Tests.Supplier
{
    public class SupplierProductTests
    {
        [Fact]
        public async Task CreateProductForSupplier()
        {
            await using var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(CreateProductForSupplier));
            var createSupplierCommand = ValidModelCreator.CreateSupplierCommand();
            var createSupplierHandler = new CreateSupplierCommandHandler(context);
            var supplierResponse = await createSupplierHandler.Handle(createSupplierCommand, CancellationToken.None);


            var createProductForSupplierCommand = new CreateProductForSupplierCommand
            {
                Label = "Product 123",
                Price = 12.5m,
                SupplierId = supplierResponse.Data.Id
            };
            var createProductForSupplierHandler = new CreateProductForSupplierCommandHandler(context);
            var response1 =
                await createProductForSupplierHandler.Handle(createProductForSupplierCommand, CancellationToken.None);
            Assert.True(response1.Success);
            Assert.Equal(createProductForSupplierCommand.Label, response1.Data.Label);
            Assert.Equal(createProductForSupplierCommand.Price, response1.Data.Price);
            Assert.Equal(createProductForSupplierCommand.SupplierId, response1.Data.SupplierId);
            
        }
    }
}
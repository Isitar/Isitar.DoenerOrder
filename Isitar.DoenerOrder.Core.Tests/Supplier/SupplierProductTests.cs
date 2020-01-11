using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
using Isitar.DoenerOrder.Core.Handlers.Supplier;
using Isitar.DoenerOrder.Core.Handlers.Supplier.CommandHandlers;
using Isitar.DoenerOrder.Core.Handlers.Supplier.QueryHandlers;
using Isitar.DoenerOrder.Core.Queries.Supplier;
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

            var supplierId = supplierResponse.Data.Id;

            var createProductForSupplierCommand = new CreateProductForSupplierCommand
            {
                Label = "Product 123",
                Price = 12.5m,
                SupplierId = supplierId
            };
            var createProductForSupplierHandler = new CreateProductForSupplierCommandHandler(context);
            var response1 =
                await createProductForSupplierHandler.Handle(createProductForSupplierCommand, CancellationToken.None);
            Assert.True(response1.Success);
            Assert.Equal(createProductForSupplierCommand.Label, response1.Data.Label);
            Assert.Equal(createProductForSupplierCommand.Price, response1.Data.Price);
            Assert.Equal(createProductForSupplierCommand.SupplierId, response1.Data.SupplierId);

            var productId = response1.Data.Id;
            
            var queryOne = new GetProductForSupplierByIdQuery
                {SupplierId = supplierId, ProductId = productId};
            var queryOneHandler = new GetProductForSupplierByIdQueryHandler(context);
            var queryOneResponse = await queryOneHandler.Handle(queryOne, CancellationToken.None);
            Assert.True(queryOneResponse.Success);
            Assert.Equal(response1.Data.Id, queryOneResponse.Data.Id);
            Assert.Equal(response1.Data.Label, queryOneResponse.Data.Label);
            Assert.Equal(response1.Data.Price, queryOneResponse.Data.Price);
            Assert.Equal(response1.Data.SupplierId, queryOneResponse.Data.SupplierId);

            var queryAll = new GetAllProductsForSupplierQuery {SupplierId = supplierId};
            var queryAllHandler = new GetAllProductsForSupplierQueryHandler(context);
            var queryAllResponse = await queryAllHandler.Handle(queryAll, CancellationToken.None);
            Assert.True(queryAllResponse.Success);
            var relevantProduct = queryAllResponse.Data.SingleOrDefault(p => p.Id == productId);
            Assert.NotNull(relevantProduct);
            Assert.Equal(response1.Data.Id, relevantProduct.Id);
            Assert.Equal(response1.Data.Label, relevantProduct.Label);
            Assert.Equal(response1.Data.Price, relevantProduct.Price);
            Assert.Equal(response1.Data.SupplierId, relevantProduct.SupplierId);
        }
    }
}
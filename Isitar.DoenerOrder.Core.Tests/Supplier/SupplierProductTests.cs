using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isitar.DoenerOrder.Core.Commands.Supplier;
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
            var res = await context.AddAsync(ValidModelCreator.Supplier());
            var supplierId = res.Entity.Id;

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
            var productId = response1.Data;

            var queryOne = new GetProductForSupplierByIdQuery
                {SupplierId = supplierId, ProductId = productId};
            var queryOneHandler = new GetProductForSupplierByIdQueryHandler(context);
            var queryOneResponse = await queryOneHandler.Handle(queryOne, CancellationToken.None);
            Assert.True(queryOneResponse.Success);
            Assert.Equal(productId, queryOneResponse.Data.Id);
            Assert.Equal(createProductForSupplierCommand.Label, queryOneResponse.Data.Label);
            Assert.Equal(createProductForSupplierCommand.Price, queryOneResponse.Data.Price);
            Assert.Equal(createProductForSupplierCommand.SupplierId, queryOneResponse.Data.SupplierId);

            var queryAll = new GetAllProductsForSupplierQuery {SupplierId = supplierId};
            var queryAllHandler = new GetAllProductsForSupplierQueryHandler(context);
            var queryAllResponse = await queryAllHandler.Handle(queryAll, CancellationToken.None);
            Assert.True(queryAllResponse.Success);
            var relevantProduct = queryAllResponse.Data.SingleOrDefault(p => p.Id == productId);
            Assert.NotNull(relevantProduct);
            Assert.Equal(productId, relevantProduct.Id);
            Assert.Equal(createProductForSupplierCommand.Label, relevantProduct.Label);
            Assert.Equal(createProductForSupplierCommand.Price, relevantProduct.Price);
            Assert.Equal(createProductForSupplierCommand.SupplierId, relevantProduct.SupplierId);
        }

        [Fact]
        public async Task UpdateProductForSupplier()
        {
            await using var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(UpdateProductForSupplier));
            var supplierRes = await context.AddAsync(ValidModelCreator.Supplier());
            var supplierId = supplierRes.Entity.Id;
            var productRes = await context.AddAsync(ValidModelCreator.Product(supplierId));
            var productId = productRes.Entity.Id;
            
            //test update
            var updateProductCmd1 = new UpdateProductForSupplierCommand
            {
                SupplierId = supplierId,
                ProductId = productId,
                Label = "Something new",
                Price = 55.5m,
            };
            var updateProductForSupplierCommandHandler = new UpdateProductForSupplierCommandHandler(context);
            var response1 =
                await updateProductForSupplierCommandHandler.Handle(updateProductCmd1, CancellationToken.None);
            Assert.True(response1.Success);
            var getUpdatedProductQuery1 = new GetProductForSupplierByIdQuery
            {
                SupplierId = supplierId,
                ProductId = productId,
            };
            var queryOneHandler = new GetProductForSupplierByIdQueryHandler(context);
            var queryResp1 =
                await queryOneHandler.Handle(getUpdatedProductQuery1,
                    CancellationToken.None);
            Assert.True(queryResp1.Success);
            Assert.Equal(supplierId, queryResp1.Data.SupplierId);
            Assert.Equal(productId, queryResp1.Data.Id);
            Assert.Equal("Something new", queryResp1.Data.Label);
            Assert.Equal(55.5m, queryResp1.Data.Price);

            // test another update
            var updateProductCmd2 = new UpdateProductForSupplierCommand
            {
                SupplierId = supplierId,
                ProductId = productId,
                Label = "another thing",
                Price = 66.45798m,
            };
            var response2 =
                await updateProductForSupplierCommandHandler.Handle(updateProductCmd2, CancellationToken.None);
            Assert.True(response2.Success);

            var queryResp2 =
                await queryOneHandler.Handle(
                    new GetProductForSupplierByIdQuery {SupplierId = supplierId, ProductId = productId},
                    CancellationToken.None);
            Assert.Equal(supplierId, queryResp2.Data.SupplierId);
            Assert.Equal(productId, queryResp2.Data.Id);
            Assert.Equal("another thing", queryResp2.Data.Label);
            Assert.Equal(66.45798m, queryResp2.Data.Price);

            // test no update if invalid data provided
            var reqInvalidSupplier = new UpdateProductForSupplierCommand
            {
                SupplierId = supplierId + 1,
                ProductId = productId,
                Price = 50.0m,
                Label = "should not be changed",
            };
            var response3 =
                await updateProductForSupplierCommandHandler.Handle(reqInvalidSupplier, CancellationToken.None);
            Assert.False(response3.Success);
            var queryResponse3 = await queryOneHandler.Handle(
                new GetProductForSupplierByIdQuery {SupplierId = supplierId, ProductId = productId,},
                CancellationToken.None);
            Assert.True(queryResponse3.Success);
            Assert.Equal(supplierId, queryResponse3.Data.SupplierId);
            Assert.Equal(productId, queryResponse3.Data.Id);
            Assert.Equal("another thing", queryResponse3.Data.Label);
            Assert.Equal(66.45798m, queryResponse3.Data.Price);
        }

        [Fact]
        public async Task DeleteProductForSupplier()
        {
            var context = DatabaseHelper.CreateInMemoryDatabaseContext(nameof(DeleteProductForSupplier));
            var supplierResp = await context.AddAsync(ValidModelCreator.Supplier());
            var supplierId = supplierResp.Entity.Id;
            
            var productId1 = (await context.AddAsync(ValidModelCreator.Product(supplierId))).Entity.Id;
            var productId2 = (await context.AddAsync(ValidModelCreator.Product(supplierId))).Entity.Id;
            var productId3 = (await context.AddAsync(ValidModelCreator.Product(supplierId))).Entity.Id;

            var deleteProduct2Cmd = new DeleteProductForSupplierCommand
                {ProductId = productId2, SupplierId = supplierId};
            var deleteProductHandler = new DeleteProductForSupplierCommandHandler(context);
            var result  = await deleteProductHandler.Handle(deleteProduct2Cmd, CancellationToken.None);
            Assert.True(result.Success);

            var queryProduct2 = new GetProductForSupplierByIdQuery {ProductId = productId2, SupplierId = supplierId};
            var queryOneHandler = new GetProductForSupplierByIdQueryHandler(context);
            var queryProduct2Result = await queryOneHandler.Handle(queryProduct2, CancellationToken.None);
            Assert.False(queryProduct2Result.Success);
            
            var queryProduct1 = new GetProductForSupplierByIdQuery{ ProductId =  productId1, SupplierId = supplierId};
            var queryProduct3 = new GetProductForSupplierByIdQuery{ ProductId =  productId3, SupplierId = supplierId};
            var queryProduct1Result = await queryOneHandler.Handle(queryProduct1, CancellationToken.None);
            Assert.True(queryProduct1Result.Success);
            var queryProduct3Result = await queryOneHandler.Handle(queryProduct3, CancellationToken.None);
            Assert.True(queryProduct3Result.Success);
            
            var result2  = await deleteProductHandler.Handle(deleteProduct2Cmd, CancellationToken.None);
            Assert.False(result2.Success);
        }
    }
}